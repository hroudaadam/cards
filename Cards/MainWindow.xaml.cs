using System.Windows;
using System.Windows.Input;

namespace Cards
{  
    /// <summary>
    /// Hlavní okno aplikace.
    /// </summary>S
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new MainPage(this);
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
        }
    }
}
