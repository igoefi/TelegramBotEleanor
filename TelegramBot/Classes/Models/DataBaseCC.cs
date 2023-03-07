using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Classes.Models
{
    public class DataBaseCC
    {
        public static BotEntities ConnectContext { get; set; } = new BotEntities();
    }
}
