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
using TelegramBot.Classes.Helper;
using TelegramBot.Classes.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TelegramBot.Pages.AdminPanel
{
    /// <summary>
    /// Логика взаимодействия для PageSendMessage.xaml
    /// </summary>
    public partial class PageSendMessage : Page
    {
        private List<string> _placesSource = new List<string>
        {
            "На Тушинской",
            "На Технополисе",
            "На Лодочной",
            "На Стратонавтов"
        };

        public PageSendMessage()
        {
            InitializeComponent();

            var alerts = DataBaseCC.ConnectContext.Alerts.ToList();
            var alertsText = new List<string>();

            foreach(var alert in alerts)
                alertsText.Add(alert.Name);

            CmbBoxAlerts.ItemsSource = alertsText;
            CmbBoxPlaceName.ItemsSource = _placesSource;
        }

        private void BtnClickGoBack(object sender, RoutedEventArgs e) =>
            FrameNav.FrameNavigation.GoBack();

        private void BtnClickSendMessage(object sender, RoutedEventArgs e)
        {
            string message = string.Empty;

            message += CmbBoxPlaceName.Text + " ";

            if(CmbBoxAlerts.SelectedIndex > -1)
            {
                message += DataBaseCC.ConnectContext.Alerts.FirstOrDefault(c => c.Name == CmbBoxAlerts.SelectedItem.ToString()).Text;
            }
            else if (!string.IsNullOrWhiteSpace(TxbAlert.Text))
            {
                message = TxbAlert.Text;
            }
            else
            {
                MessageBox.Show("Вы не выбрали тревогу");
                return;
            }

            MessageSaver.Message = message;

            try
            {
                BotLogic.SendMessage(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbBoxChangedPlace(object sender, SelectionChangedEventArgs e)
        {
            if (TxbAlert == null) return;
            TxbAlert.Text = (string)CmbBoxPlaceName.SelectedItem;
        }
    }
}
