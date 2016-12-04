using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoC.DataAccess
{
    [Table("UserHistory")]
    public class UserHistory
    {
        [Key]
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public int RequestCounter { get; set; }

        public DateTime LastRequest { get; set; }

        public TimeSpan AverageTime { get; set; }
    }
}
