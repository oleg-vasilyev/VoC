using System.Collections.Generic;

using Newtonsoft.Json;

namespace BusinessModel
{
    public class DetectServerResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("results")]
        public List<DetectServerModel> Results { get; set; }
    }
    public class DetectServerModel
    {
        [JsonProperty("language_code")]
        public string LanguageCode { get; set; }

        [JsonProperty("language_name")]
        public string LanguageName { get; set; }

        [JsonProperty("probability")]
        public double Probability { get; set; }

        [JsonProperty("percentage")]
        public double Percentage { get; set; }

        [JsonProperty("reliable_result")]
        public bool ReliableResult { get; set; }

    }
}
