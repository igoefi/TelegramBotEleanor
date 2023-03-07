using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using TelegramBot.Classes.Models;

namespace TelegramBot.Classes
{
    public class DBUserHelper
    {
        public static void SelectUserVoice(long userID, string voiceID)
        {
            int.TryParse(userID.ToString(), out int needID);
            if (FindUser(needID) == null)
            {
                CreateUser(needID, voiceID);
            }
            else
            {
                SelectUserVoiceInDB(needID, voiceID);
            }
        }

        private static UserSelectedVoice FindUser(int ID)
        {
            return DataBaseCC.ConnectContext.UserSelectedVoice.FirstOrDefault(c => c.UserID == ID);
        }

        private static void CreateUser(int userID, string voiceID)
        {
            try
            {
                DataBaseCC.ConnectContext.UserSelectedVoice.Add(new UserSelectedVoice()
                { UserID = userID, VoiceId = voiceID });
                DataBaseCC.ConnectContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private static void SelectUserVoiceInDB(long userID, string voiceID)
        {
            IEnumerable<UserSelectedVoice> users = DataBaseCC.ConnectContext.UserSelectedVoice.Where(c => c.UserID == userID).
                AsEnumerable().Select(
                c =>
                {
                    c.VoiceId = voiceID;
                    return c;
                });

            foreach (var user in users)
                DataBaseCC.ConnectContext.Entry(user).State = EntityState.Modified;

            DataBaseCC.ConnectContext.SaveChangesAsync();
        }
        public static string GetUserSelectedVoice(long userID)
        {
            int.TryParse(userID.ToString(), out int needID);
            var user = DataBaseCC.ConnectContext.UserSelectedVoice.FirstOrDefault(c => c.UserID == needID);
            if (user == null) return null;

            return user.VoiceId;
        }
    }
}
