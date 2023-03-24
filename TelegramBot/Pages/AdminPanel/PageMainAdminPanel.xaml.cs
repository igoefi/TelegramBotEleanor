using System.Windows;
using System.Windows.Controls;
using TelegramBot.Classes;
using TelegramBot.Classes.Helper;

namespace TelegramBot.Pages.AdminPanel
{
    /// <summary>
    /// Логика взаимодействия для PageMainAdminPanel.xaml
    /// </summary>
    public partial class PageMainAdminPanel : Page
    {
        BotLogic _bot;
        public PageMainAdminPanel()
        {
            InitializeComponent();
        }

        private void BtnClickLaunchBot(object sender, RoutedEventArgs e)
        {
            var token = AdminProfileSaver.Admin.Token;
            if (token == null)
            {
                MessageBox.Show("Set bot token in settings");
                return;
            }
            _bot = new BotLogic();
            _bot.StartBot(token);
            MessageBox.Show("Bot started");
        }

        private void BtnClickGoSettings(object sender, RoutedEventArgs e) =>
            FrameNav.FrameNavigation.Navigate(new PageBotSettings());

        private void BtnClickGoVoiceSettings(object sender, RoutedEventArgs e) =>
            FrameNav.FrameNavigation.Navigate(new PageVoiceSettings());

        private void BtnClickGoSendMessage(object sender, RoutedEventArgs e) =>
            FrameNav.FrameNavigation.Navigate(new PageSendMessage());
    }
}
