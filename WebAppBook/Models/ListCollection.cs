using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppBook.Models
{
    public static class ListCollection
    {
        public static List<Genre> genres = new List<Genre>();
        public static List<Language> languages = new List<Language>();
        public static List<Translator> translators = new List<Translator>();

        public static void SetAllValue(ICollection<Genre> genres , ICollection<Language> languages , ICollection<Translator> translators)
        {
            ListCollection.genres = genres.ToList();
            ListCollection.languages = languages.ToList();
            ListCollection.translators = translators.ToList();
        }
        public static void SetAllValue(ICollection<Genre> genres)
        {
            ListCollection.genres = genres.ToList();
        }

        public static void SetDefault()
        {
            genres.Clear();
            languages.Clear();
            translators.Clear();
        }
    }
}