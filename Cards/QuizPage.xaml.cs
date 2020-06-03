using System;
using System.Windows;
using System.Windows.Controls;

namespace Cards
{
    /// <summary>
    /// Stavy stránky: Prohlížení, Úprava, Vytváření
    /// </summary>
    public enum State { View, Edit, Create }

    /// <summary>
    /// Stránka pro prohlížení, úpravu, vytváření kvízu.
    /// </summary>
    public partial class QuizPage : Page
    {      
        private Quiz _quiz; // načtený kvíz
        private MainWindow _mw; // okno, do kterého je stránka vložena
        private State _state; // stav stránky

        /// <summary>
        /// Vytvoří instanci stránky.
        /// </summary>
        /// <param name="mw">Okno, do kterého je stránka vložena</param>
        /// <param name="quiz">Načtený kvíz</param>
        /// <param name="state">Stav stránky</param>
        public QuizPage(MainWindow mw, Quiz quiz, State state)
        {
            _quiz = quiz;
            _mw = mw;
            _state = state;

            InitializeComponent();
            ModifyControls();   // zviditelnění ovládacích prvků dle stavu stránky         
            DataContext = _quiz;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            _mw.Main.Content = new MainPage(_mw);   // vložení stránky hlavní nabídky zpět do hlavního okna
        }

        private void CardTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // přizpůsobení velikosti textu počtu znaků v ovládacím prvku
            textBox.FontSize = 1200 / (textBox.Text.Length + 50);
            if (textBox.FontSize < 12) textBox.FontSize = 12;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _quiz.PreviousCard();
            }
            catch (IndexOutOfCardsRangeException ex)
            {
                MessageBox.Show(ex.Message, "Varování", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            TurnCard(true); // zajištění, aby u předchozí karty byla zobrazena otázka
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _quiz.NextCard();
            }
            catch (IndexOutOfCardsRangeException ex)
            {
                switch (_state)
                {
                    case State.View:    // pokud je ve stavu prohlížení, zobrazit pouze chybu
                        {
                            MessageBox.Show(ex.Message, "Varování", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            break;
                        }
                    case State.Edit:    // pokud je ve stavu úpravy, zobrazit možosti přidání nové karty
                        {
                            MessageBoxResult result = MessageBox.Show("Chcete přidat novou kartu?", "Nová karta", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                _quiz.AddNewCard();
                                _quiz.NextCard();
                            }
                            break;
                        }
                    case State.Create:  // pokud je ve stavu vytváření, vždy vytvořit novou kartu
                        {
                            _quiz.AddNewCard();
                            _quiz.NextCard();
                            break;
                        }
                }
            }
            TurnCard(true);
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            TurnCard(false);    // zobrazení otázky, pokud je právě zobrazena odpověď a naopak
        }

        /// <summary>
        /// Otočení karty - zobrazení otázky nebo zobrazení odpovědi.
        /// </summary>
        /// <param name="hideAnswer">Logiká hodnota, zda má být vždy zobrazena pouze otázka</param>
        private void TurnCard(bool hideAnswer)
        {
            // zobrazení odpovědi
            if ((answerTextBox.Visibility == Visibility.Hidden) && (!hideAnswer))
            {
                answerTextBox.Visibility = Visibility.Visible;
                questionTextBox.Visibility = Visibility.Hidden;
                showButton.Content = "Otočeno";
            }
            // zobrazení otázky
            else
            {
                answerTextBox.Visibility = Visibility.Hidden;
                questionTextBox.Visibility = Visibility.Visible;
                showButton.Content = "Otočit";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _quiz.Save();
                MessageBox.Show("Kvíz byl uložen.", "Infromace", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidCardsException ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }    
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }        

        /// <summary>
        /// Zviditelnění ovládacích prvků dle stavu stránky.
        /// </summary>
        private void ModifyControls()
        {
            if (_state == State.View)
            {                
                saveButton.Visibility = Visibility.Hidden;
                deleteButton.Visibility = Visibility.Hidden;
                questionTextBox.IsReadOnly = true;
                answerTextBox.IsReadOnly = true;
            }                
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {            
            _quiz.DeleteCurrentCard();
        }
    }
}
