using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TelegramBot.Classes.Models;

namespace TelegramBot.Classes
{
    public static class AdminProfileSaver
    {
        public static Admins Admin { get; set; }

        public static void SaveChanges()
        {
            IEnumerable<Admins> admins = DataBaseCC.ConnectContext.Admins.Where(c => c.ID == Admin.ID).
                AsEnumerable().Select(
            c =>
                {
                    c.Login = Admin.Login;
                    c.Password = Admin.Password;
                    c.Token = Admin.Token;
                    c.UsersJSON = Admin.UsersJSON;
                    c.VoicesJSON = Admin.VoicesJSON;
                    return c;
                });

            foreach (var admin in admins)
                DataBaseCC.ConnectContext.Entry(admin).State = EntityState.Modified;

            DataBaseCC.ConnectContext.SaveChangesAsync();
        }
    }
}
