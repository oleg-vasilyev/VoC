using BusinessModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VoC.DataAccess
{
    public class Provider : IDisposable
    {
        const string key = "d48050435de0f4f951de4b82cfd8acc0";
        const string unknown = "unknown";
        private MainContext context;
        private object contextDisposeLocker = new object();
        public Provider()
        {
            context = new MainContext();
        }

        public void AddNewUser(Guid userId, string username)
        {
            context.UserHistory.Add(new UserHistory()
            {
                AverageTime = TimeSpan.Zero,
                Username = username,
                LastRequest = DateTime.Now,
                RequestCounter = 0,
                UserId = userId
            });
            context.SaveChanges();
        }

        public WordModel CheckWord(string wordValue, Guid userId)
        {
            WordModel model = null;

            var words = context.Words.Include("Translations").Include("Translations.LanguageAccessory").Where(m => m.WordValue == wordValue);

            this.RegistrationUserActivity(userId);

            if (words.Any())
            {
                var word = words.First();
                model = new WordModel()
                {
                    WordValue = wordValue,
                    Probabilities = new List<TranslationModel>(word.Translations
                                                                   .Select(m => new TranslationModel()
                                                                   {
                                                                       Language = m.LanguageAccessory.Name,
                                                                       Probability = m.Probability
                                                                   }))
                };

            }

            return model;
        }

        public WordModel AddWords(string word)
        {
            Word wordModel = new Word()
            {
                WordValue = word
            };

            var model = new WordModel()
            {
                WordValue = word,
                Probabilities = new List<TranslationModel>()
            };


            string value = TranslationApiResponse(word);

            DetectServerResult result = JsonConvert.DeserializeObject<DetectServerResult>(value);

            context.Words.Add(wordModel);
            context.SaveChanges();

            var codes = result.Results.Select(m => m.LanguageCode);

            var languageList = context.Languages.Where(m => codes.Contains(m.Code)).ToList();

            languageList.ForEach(delegate (Language lang)
            {
                context.Translations.Add(new WordTranslations()
                {
                    Expression = wordModel,
                    LanguageAccessory = lang,
                    Probability = result.Results.Where(m => m.LanguageCode == lang.Code).First().Percentage
                });

                model.Probabilities.Add(new TranslationModel()
                {
                    Language = lang.Name,
                    Probability = result.Results.Where(m => m.LanguageCode == lang.Code).First().Percentage
                });
            });
            context.SaveChanges();

            return model;
        }

        public List<UserHistory> GetTopTen()
        {
            var users = context.UserHistory.OrderByDescending(m => m.RequestCounter).Skip(0).Take(10).ToList();
            return users;
        }

        private string TranslationApiResponse(string word)
        {
            var request = (HttpWebRequest)WebRequest.Create(@"http://apilayer.net/api/detect?access_key=d48050435de0f4f951de4b82cfd8acc0&query=" + word);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }

        public void Dispose()
        {
            if (context != null)
            {
                lock (contextDisposeLocker)
                {
                    if (context != null)
                    {
                        context.Dispose();
                    }
                }
            }
        }


        private void RegistrationUserActivity(Guid userId)
        {
            var user = context.UserHistory.Where(m => m.UserId == userId).First();
            user.RequestCounter++;
            var average = (user.AverageTime + (DateTime.Now - user.LastRequest));
            user.AverageTime = new TimeSpan(average.Ticks / 2);
            user.LastRequest = DateTime.Now;
            context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }
    }
}
