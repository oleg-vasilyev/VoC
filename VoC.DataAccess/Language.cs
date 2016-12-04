using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoC.DataAccess
{
    [Table("Languages")]
    public class Language
    {
        public Language()
        {
            Translations = new HashSet<WordTranslations>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public HashSet<WordTranslations> Translations { get; set; }
    }
}
