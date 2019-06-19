using Microsoft.EntityFrameworkCore;

namespace EnglishRussianDictionary
{
    public class DBEntity : DbContext
    {
        internal DbSet<WordTranslate> Dictionary { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
        }        
    }

    internal class WordTranslate
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Translate { get; set; }
    }
}
