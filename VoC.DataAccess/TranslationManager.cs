using BusinessModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VoC.ExternalService;

namespace VoC.DataAccess
{
    public class TranslationManager
    {

        public List<TranslationModel> GetLanguagesProbabilities(string word)
        {
            List<TranslationModel> translations = new List<TranslationModel>();

            using (SQLiteConnection connection = new SQLiteConnection(DbInit.DbInit.ConnectionString))
            {

                word = word.ToLower();
                connection.Open();


                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM Words where Word = @word";
                command.Parameters.AddWithValue("word", word);
                var result = command.ExecuteReader();

                if (result.Read())
                {
                    var id = result["Id"].ToString();
                    command = connection.CreateCommand();
                    command.CommandText = "Select l.LanguageCode, p.Probability from Probabilities as p left join Languages as l on p.LanguageId = l.Id where p.WordId = @wordId";
                    command.Parameters.AddWithValue("wordId", id);

                    var languges = command.ExecuteReader();
                    while (languges.Read())
                    {
                        translations.Add(new TranslationModel() { LanguageCode = languges["LanguageCode"].ToString(), Probability = languges["Probability"].ToString() });
                    }

                }
                else
                {
                    var serviceResult = this.GetTranslationsFromService(word);
                    translations = serviceResult.Response.Select(m => new TranslationModel() { LanguageCode = m.LanguageCode, Probability = Math.Round(m.Probability, 2).ToString() }).ToList();

                    command = connection.CreateCommand();
                    command.CommandText = "Insert into Words (Word)values(@word)";
                    command.Parameters.AddWithValue("word", word);
                    var wordId = command.ExecuteNonQuery(CommandBehavior.KeyInfo);

                    if (serviceResult.Response.Count > 0)
                    {
                        command = connection.CreateCommand();
                        command.CommandText = "Select * from Languages where LanguageCode in (" + string.Join(",", serviceResult.Response.Select(m => "'" + m.LanguageCode + "'")) + ")";
                        result = command.ExecuteReader();

                        string newProbabilities = "Insert into Probabilities (Probability, LanguageId, WordId) values";
                        List<string> parameters = new List<string>();
                        while (result.Read())
                        {
                            var language = serviceResult.Response.FirstOrDefault(m => m.LanguageCode == result["LanguageCode"].ToString());
                            parameters.Add(string.Format("('{0}',{1},{2})", language.Probability, result["Id"], wordId));
                        }
                        newProbabilities += string.Join(",", parameters);
                        command = connection.CreateCommand();
                        command.CommandText = newProbabilities;
                        command.ExecuteNonQuery();
                    }
                }
            }

            return translations;
        }

        private TranslationServiceResponse GetTranslationsFromService(string word)
        {
            return TranslationService.GetTranslations(word);
        }
    }
}
