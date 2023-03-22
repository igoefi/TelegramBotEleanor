using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TelegramBot.Classes;
using TelegramBot.Classes.Helper;
using TelegramBot.Classes.JSON;
using TelegramBot.Classes.VoiceControllers;

namespace TelegramBot.Pages.AdminPanel
{
    /// <summary>
    /// Логика взаимодействия для PageVoiceSettings.xaml
    /// </summary>
    public partial class PageVoiceSettings : Page
    {
        public PageVoiceSettings()
        {
            InitializeComponent();

            var voicesName = new List<string>();
            var voices = MakeVoiceController.GetAllVoices();
            foreach (var voice in voices)
                voicesName.Add(voice.VoiceInfo.Name);
            CmbBoxAllVoices.ItemsSource = voicesName;

            UpdateSelectedVoices();
        }

        private void BtnClickGoBack(object sender, RoutedEventArgs e) =>
            FrameNav.FrameNavigation.GoBack();

        private void BtnClickAddVoice(object sender, RoutedEventArgs e)
        {
            var voiceName = (string)CmbBoxAllVoices.SelectedItem;
            if (string.IsNullOrEmpty(voiceName)) return;

            var voice = MakeVoiceController.FindVoiceByName(voiceName);
            if (voice == null) return;

            DBAdminHelper.AddSetVoice(voice, 1);
            UpdateSelectedVoices();
        }

        private void UpdateSelectedVoices()
        {
            var savedVoicesName = JSONSerializeController.DeserializeObject<VoicesJSON>(AdminProfileSaver.Admin.VoicesJSON);
            if (savedVoicesName != null)
                CmbBoxSavedVoices.ItemsSource = savedVoicesName.Voices.Keys;
        }

        private void BtnClickSaveChanges(object sender, RoutedEventArgs e)
        {
            var voiceName = (string)CmbBoxSavedVoices.SelectedItem;
            if (string.IsNullOrEmpty(voiceName)) return;
            //DBAdminHelper.AddSetVoice(MakeVoiceController.FindVoiceByName(voiceName), int.Parse(TxbVoiceCost.Text));
            MessageBox.Show("Save completed");
        }

        private void CmbBoxChangedAddVoice(object sender, SelectionChangedEventArgs e)
        {
            var voice = MakeVoiceController.FindVoiceByName((string)CmbBoxAllVoices.SelectedItem);
            if (voice == null) return;
            RnVoiceName.Text = voice.VoiceInfo.Name;
            RnVoiceCulture.Text = voice.VoiceInfo.Culture.EnglishName;
        }

        private void CmbBoxChangedChangeVoice(object sender, SelectionChangedEventArgs e)
        {
            var voice = MakeVoiceController.FindVoiceByName((string)CmbBoxSavedVoices.SelectedItem);
            if (voice == null) return;
            RnSelectedVoiceName.Text = voice.VoiceInfo.Name;
            RnSelectedVoiceCulture.Text = voice.VoiceInfo.Culture.EnglishName;
            //TxbVoiceCost.Text = JSONSerializeController.DeserializeObject<VoicesJSON>(AdminProfileSaver.Admin.VoicesJSON)
            //    .Voices[voice.VoiceInfo.Name].ToString();
        }

        private void BtnClickDeleteVoice(object sender, RoutedEventArgs e)
        {
            var voiceName = (string)CmbBoxSavedVoices.SelectedItem;
            if (string.IsNullOrEmpty(voiceName)) return;
            DBAdminHelper.DeleteVoice(voiceName);
            UpdateSelectedVoices();

            RnSelectedVoiceCulture.Text = null;
            RnSelectedVoiceName.Text = null;
        }
    }
}
