using System.Collections.Generic;

namespace VoC.ExternalService
{
    public class TranslationServiceResponse
    {
        public TranslationServiceResponse()
        {
            Response = new List<Item>();
        }
        public List<Item> Response { get; set; }
    }

    public class Item
    {
        public string LanguageCode { get; set; }
        public double Probability { get; set; }
    }
}