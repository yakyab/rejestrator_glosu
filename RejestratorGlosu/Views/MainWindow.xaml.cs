using RejestratorGlosu.ViewModels;
using System.Windows;

namespace RejestratorGlosu
{
    /// <summary>
    /// Główne okno aplikacji RejestratorGlosu.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Inicjalizuje nową instancję klasy MainWindow.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
