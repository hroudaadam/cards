using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Cards
{
    /// <summary>
    /// Stránka hlavní nabídky.
    /// </summary>
    public partial class MainPage : Page
    {
        private MainWindow _mw;

        /// <summary>
        /// Kolekce načtených kvízů, které mají soubor ve složce aplikace.
        /// </summary>
        public ObservableCollection<Quiz> Files { get; set; }

        /// <summary>
        /// Vytvoří novou instanci stránky pro hlavní nabídku.
        /// </summary>
        /// <param name="mw">Instance okna, ve kterém je stránka vložena</param>
        public MainPage(MainWindow mw)
        {
            this._mw = mw;
            Files = CardsController.GetFilesInFolder();
            
            InitializeComponent();
            DataContext = this;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            PathDialogWindow dialog = new PathDialogWindow();
            if (dialog.ShowDialog() == true)    // načtení cesty souboru v dialogu
            {
                Quiz newQuiz = new Quiz(dialog.Path);
                newQuiz.AddNewCard(); // v novém kvízu bude jedna prázdá karta

                _mw.Main.Content = new QuizPage(_mw, newQuiz, State.Create);
            }            
        }

        private void EditViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (filesListBox.SelectedItem == null)  // není zvolený žádný kvíz
            {
                MessageBox.Show("Nebyl vybrán žádný kvíz", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Quiz selectedQuiz = (Quiz)filesListBox.SelectedItem;

                try
                {
                    selectedQuiz.Import();  // při upravování a prohlížení je třeba nejprve kvíz načíst ze soubory
                }
                catch (Exception)
                {
                    MessageBox.Show("Soubor se nepodařilo načíst", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if ((sender as Button).Name == "editButton")    // rozlišení, zda se jedná o prohlížení nebo úpravu
                {
                    _mw.Main.Content = new QuizPage(_mw, selectedQuiz, State.Edit);
                }
                else
                {
                    _mw.Main.Content = new QuizPage(_mw, selectedQuiz, State.View);
                }                
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            _mw.Main.Content = new HelpPage(_mw);
        }
    }
}
