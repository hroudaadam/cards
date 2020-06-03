using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace Cards
{
    /// <summary>
    /// Dialog pro načtení cesty k souboru.
    /// </summary>
    public partial class PathDialogWindow : Window
    {
        /// <summary>
        /// Vytvoření instance dialogu pro načtení cesty k souboru.
        /// </summary>
        public PathDialogWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Vrací validní a unikátní cestu.
        /// </summary>
        public string Path { get; private set; }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            string tmpPath = System.IO.Path.Combine(CardsController.ApplicationFolderPath, $"{pathTextBox.Text}.bin");

            if (!ValidatePath(pathTextBox.Text)) // zajišťuje, že je název souboru validní
            {
                warningTextBlock.Text = "Neplatný název!";
            }
            else if (File.Exists(tmpPath))  // zajišťuje unikátnost názvu souboru
            {
                warningTextBlock.Text = "Kvíz se stejným jménem již existuje!";
            }
            else   // cesta je v pořádku
            {
                DialogResult = true;
                Path = tmpPath;
                this.Close();
            }
        }

        /// <summary>
        /// Kontroluje, zda je název souboru validní.
        /// </summary>
        /// <param name="fileName">Název souboru</param>
        /// <returns>True pokud je název souboru validní, jinak False.</returns>
        private bool ValidatePath(string fileName)
        {
            char[] unvalidChars = "/\\<>*?:|".ToCharArray();
            foreach (char c in unvalidChars)
            {
                if (fileName.Contains(c))
                {
                    return false;
                }
            }
            if (fileName.Trim() == "")
            {
                return false;
            }
            return true;
        }
    }
}
