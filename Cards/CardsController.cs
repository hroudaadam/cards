using System.Collections.ObjectModel;
using System.IO;

namespace Cards
{
    /// <summary>
    /// Ovládání a nastavení aplikace.
    /// </summary>
    public static class CardsController
    {
        /// <summary>
        /// Vrací cestu ke složce aplikace.
        /// </summary>
        public static string ApplicationFolderPath
        {
            get
            {
                string path = @"Saves";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        /// <summary>
        /// Vrací kolekci kvízů, které mají soubor ve složce aplikace.
        /// </summary>
        public static ObservableCollection<Quiz> GetFilesInFolder()
        {
            ObservableCollection<Quiz> tmp = new ObservableCollection<Quiz>();

            string[] allPaths = (Directory.GetFiles(ApplicationFolderPath, "*.bin"));
            foreach (string path in allPaths)
            {
                tmp.Add(new Quiz(path));
            }

            return tmp;
        }

        /// <summary>
        /// Nápověda k aplikaci.
        /// </summary>
        public const string HelpText =
            "Tato aplikace slouží jako studijní pomůcka, je možné ji využít při učení slovíček v cizím jazyce, chemického názvosloví a podobně.\n" +
            "Umožňuje tyto kvízy prohlížet, upravovat a samozřejmě i vytvářet.\nPodmínkou každého uloženého kvízu je, že musí obsahovat alespoň jednu kartičku, která musí mít vyplněné obě strany (otázku i odpověď).";
    }
}
