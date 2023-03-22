namespace TelegramBot.Classes.Models
{
    public class DataBaseCC
    {
        public static BotEntities ConnectContext { get; set; } = new BotEntities();
    }
}
