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
using TelegramBot.Classes.Helper;

namespace TelegramBot.Pages.AdminPanel
{
    /// <summary>
    /// Логика взаимодействия для PageBotSettings.xaml
    /// </summary>
    public partial class PageBotSettings : Page
    {
        public PageBotSettings()
        {
            InitializeComponent();
            TxbToken.Text = AdminProfileSaver.Admin.Token != null ? AdminProfileSaver.Admin.Token : string.Empty;
        }
        
        private void BtnClickSaveSettings(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)TxbToken.Template.FindName("TB", TxbToken);
            var token = textBox.Text;
            if (string.IsNullOrWhiteSpace(token) || !BotLogic.IsTokenCorrect(token)) 
            { 
                MessageBox.Show("Uncorrect token");
                return;
            }
            DBAdminHelper.SetToken(token);
            FrameNav.FrameNavigation.GoBack();
        }
    }
}
