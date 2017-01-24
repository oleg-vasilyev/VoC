using System;
using System.Linq;

namespace VoC.ExternalService
{
    public static class TranslationService
    {
        private readonly static string[] SupportedLangugaesCode;
        private readonly static Random random;

        static TranslationService()
        {
            SupportedLangugaesCode = new string[] { "en", "pt", "se", "ru", "bg" };
            random = new Random();
        }

        public static TranslationServiceResponse GetTranslations(string word)
        {
            var response = new TranslationServiceResponse();

            int languageAmount = random.Next(1, SupportedLangugaesCode.Length);
            var enableLanguages = SupportedLangugaesCode.ToList();

            for (int i = 0; i < languageAmount; i++)
            {
                var selectedLangugeId = random.Next(0, enableLanguages.Count-1);
                response.Response.Add(new Item() { LanguageCode = enableLanguages[selectedLangugeId], Probability = Math.Round(random.NextDouble() * 100,2) });
                enableLanguages.RemoveAt(selectedLangugeId);
            }

            return response;
        }
    }
}
