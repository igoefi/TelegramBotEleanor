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
using TelegramBot.Classes;

namespace TelegramBot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BotLogic _bot;
        public MainWindow()
        {
            InitializeComponent();
            DBUserHelper.SelectUserVoice(1,"1");
        }

        private void BtnClickStartBot(object sender, RoutedEventArgs e)
        {
            _bot = new BotLogic();
        }
    }
}
