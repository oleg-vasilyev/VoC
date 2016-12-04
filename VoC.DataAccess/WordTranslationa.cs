using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoC.DataAccess
{
    public class WordTranslations
    {
        [Key]
        public int Id { get; set; }

        public int WordId { get; set; }

        [ForeignKey("WordId")]
        public Word Expression { get; set; }

        public int LanguageId { get; set; }

        [ForeignKey("LanguageId")]
        public Language LanguageAccessory { get; set; }

        public double Probability { get; set; }
    }
}
