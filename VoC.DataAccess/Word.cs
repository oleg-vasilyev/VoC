using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoC.DataAccess
{
    [Table("Words")]
    public class Word
    {
        public Word()
        {
            Translations = new HashSet<WordTranslations>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string WordValue { get; set; }

        public HashSet<WordTranslations> Translations { get; set; }

    }
}
