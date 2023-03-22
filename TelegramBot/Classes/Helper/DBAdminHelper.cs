using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows;
using TelegramBot.Classes.JSON;
using TelegramBot.Classes.Models;

namespace TelegramBot.Classes.Helper
{
    public class DBAdminHelper
    {
        public static Admins FindAdmin(string login, string password) =>
            DataBaseCC.ConnectContext.Admins.FirstOrDefault(c => c.Login == login && c.Password == password);

        public static string CreateAdmin(string referalCode, string login, string password)
        {
            var referal = DataBaseCC.ConnectContext.ReferalCodes.FirstOrDefault(c => c.Code == referalCode);
            if (referal == null) return "Код недействителен";
            MessageBox.Show("1");
            if (referal.IsUsed) return "Код недействителен";

            if (DataBaseCC.ConnectContext.Admins.FirstOrDefault(c => c.Login == login) != null)
                return "Логин уже занят";

            DataBaseCC.ConnectContext.Admins.Add(new Admins() { Login = login, Password = password });

            IEnumerable<ReferalCodes> codes = DataBaseCC.ConnectContext.ReferalCodes.Where(c => c.Id == referal.Id).
                                AsEnumerable().Select(
                            c =>
                            {
                                c.IsUsed = true;
                                return c;
                            });

            foreach (var admin in codes)
                DataBaseCC.ConnectContext.Entry(admin).State = EntityState.Modified;

            DataBaseCC.ConnectContext.SaveChangesAsync();
            return null;
        }

        public static void SetToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return;

            IEnumerable<Admins> codes = DataBaseCC.ConnectContext.Admins.Where(c => c.ID == AdminProfileSaver.Admin.ID).
                    AsEnumerable().Select(
                c =>
                {
                    c.Token = token;
                    return c;
                });

            foreach (var admin in codes)
                DataBaseCC.ConnectContext.Entry(admin).State = EntityState.Modified;

            DataBaseCC.ConnectContext.SaveChangesAsync();
        }

        public static void AddSetVoice(InstalledVoice voice, int cost)
        {
            var voices = JSONSerializeController.DeserializeObject<VoicesJSON>(AdminProfileSaver.Admin.VoicesJSON);
            if (voices == null) voices = new VoicesJSON();

            if (voices.Voices.TryGetValue(voice.VoiceInfo.Name, out _))
                voices.Voices[voice.VoiceInfo.Name] = cost;
            else
                voices.Voices.Add(voice.VoiceInfo.Name, cost);

            AdminProfileSaver.Admin.VoicesJSON = JSONSerializeController.SerializeObject(voices);
            AdminProfileSaver.SaveChanges();
        }

        public static void DeleteVoice(string voiceName)
        {
            var voices = JSONSerializeController.DeserializeObject<VoicesJSON>(AdminProfileSaver.Admin.VoicesJSON);
            if (voices == null) voices = new VoicesJSON();

            voices.Voices.TryGetValue(voiceName, out int cost);
            if (cost == default) return;
            voices.Voices.Remove(voiceName);

            AdminProfileSaver.Admin.VoicesJSON = JSONSerializeController.SerializeObject(voices);
            AdminProfileSaver.SaveChanges();
        }
    }
}
