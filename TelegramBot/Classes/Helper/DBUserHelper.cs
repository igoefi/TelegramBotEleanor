using System.Collections.Generic;
using TelegramBot.Classes.Helper;
using TelegramBot.Classes.JSON;

namespace TelegramBot.Classes
{
    public class DBUserHelper
    {
        public static void SelectUserVoice(long userID, string userName, string voiceName)
        {
            var users = GetUsers();
            if (users == null || FindUser(userID, users) == null)
                CreateUser(userID, voiceName, users);
            else
                SelectUserVoiceInDB(userID, voiceName, users);
        }

        public static void SelectUserSubInDB(long userID)
        {
            var users = GetUsers();
            if (users == null)
                users = new List<UserEntity>
                {
                    new UserEntity() { IsSubscribed = true, ID = userID}
                };
            else
                foreach (var user in users)
                    if (user.ID == userID)
                        user.IsSubscribed = !user.IsSubscribed;

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

        private static UserEntity FindUser(long ID, List<UserEntity> users)
        {
            foreach (var user in users)
                if (user.ID == ID)
                    return user;
            return null;
        }

        private static void CreateUser(long userID, string voiceName, List<UserEntity> users)
        {
            if (users == null) users = new List<UserEntity>();
            users.Add(new UserEntity() { ID = userID, SelectedVoice = voiceName});
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

        public static List<UserEntity> GetUsers()
        {
            var users = JSONSerializeController.DeserializeObject<UsersJSON>(AdminProfileSaver.Admin.UsersJSON);
            if (users == null) return null;

            return users.Users;
        }
    }
}
