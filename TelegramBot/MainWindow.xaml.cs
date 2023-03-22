using System.Windows;
using System.Windows.Input;
using TelegramBot.Classes.Helper;
using TelegramBot.Pages;

namespace TelegramBot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrameNav.FrameNavigation = FrmMain;
            FrmMain.Navigate(new PageLogin());
        }

        private void BtnClickExit(object sender, RoutedEventArgs e) =>
            Application.Current.Shutdown();

        private void BtnClickHide(object sender, RoutedEventArgs e) =>
            Application.Current.MainWindow.WindowState = WindowState.Minimized;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) =>
            DragMove();

        private void BtnClickFullOpen(object sender, RoutedEventArgs e) =>
            Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Normal
            ? WindowState.Maximized : WindowState.Normal;
    }
}
