using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using TelegramBot.Classes.Helper;
using TelegramBot.Classes.JSON;
using TelegramBot.Classes.Models;

namespace TelegramBot.Classes
{
    public class DBUserHelper
    {
        private const int _numFirstCoins = 100;
        public static void SelectUserVoice(long userID, string userName, string voiceName)
        {
            var users = GetUsers();
            if (users == null || FindUser(userID, users) == null)
                CreateUser(userID, userName, voiceName, users);
            else
                SelectUserVoiceInDB(userID, voiceName, users);
        }

        private static UserEntity FindUser(long ID, List<UserEntity> users)
        {
            foreach (var user in users)
                if (user.ID == ID)
                    return user;
            return null;
        }

        private static void CreateUser(long userID, string userName, string voiceName, List<UserEntity> users)
        {
            if(users == null) users = new List<UserEntity>();
            users.Add(new UserEntity() { Coins = _numFirstCoins, ID = userID, SelectedVoice = voiceName, Name = userName });
            AdminProfileSaver.Admin.UsersJSON = JSONSerializeController.SerializeObject(new UsersJSON { Users = users });
            AdminProfileSaver.SaveChanges();
        }

        private static void SelectUserVoiceInDB(long userID, string voiceName, List<UserEntity> users)
        {
            foreach (var user in users)
                if (user.ID == userID)
                    user.SelectedVoice = voiceName;

            AdminProfileSaver.Admin.UsersJSON = JSONSerializeController.SerializeObject(new UsersJSON { Users = users });
            AdminProfileSaver.SaveChanges();
        }
        public static string GetUserSelectedVoice(long userID)
        {
            var users = GetUsers();
            if (users == null) return null;
            foreach (var user in users)
                if (user.ID == userID)
                    return user.SelectedVoice;
            return null;
        }

        private static List<UserEntity> GetUsers()
        {
            var users = JSONSerializeController.DeserializeObject<UsersJSON>(AdminProfileSaver.Admin.UsersJSON);
            if (users == null) return null;

            return users.Users;
        }
    }
}
