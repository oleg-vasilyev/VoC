using System.Collections.Generic;

namespace BusinessModel
{
    public class WordModel
    {
        public string WordValue { get; set; }

        public List<TranslationModel> Probabilities { get; set; }
    }
}
