using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoC.DataAccess
{
    public partial class MainContext : DbContext
    {
        public MainContext()
         : base(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VoC;Integrated Security=True")
			
        {

				}

        public DbSet<Language> Languages { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<UserHistory> UserHistory { get; set; }
        public DbSet<WordTranslations> Translations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

    }
}
