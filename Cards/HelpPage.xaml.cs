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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cards
{
    /// <summary>
    /// Stránka nápovědy aplikace.
    /// </summary>
    public partial class HelpPage : Page
    {
        private MainWindow _mw;

        /// <summary>
        /// Vytvoří novou instanci stránky pro nápovědu.
        /// </summary>
        /// <param name="mw">Instance okna, ve kterém je stránka vložena</param>
        public HelpPage(MainWindow mw)
        {
            _mw = mw;
            InitializeComponent();
            helpTextBox.Text = CardsController.HelpText;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            _mw.Main.Content = new MainPage(_mw);
        }
    }
}
