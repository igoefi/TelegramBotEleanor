using Newtonsoft.Json;

namespace TelegramBot.Classes.Helper
{
    public class JSONSerializeController
    {
        static public string SerializeObject(object obj)
        {
            if(obj == null) return null;
            return JsonConvert.SerializeObject(obj);
        }

        static public T DeserializeObject<T>(string JSON)
        {
            if (string.IsNullOrWhiteSpace(JSON)) return default;
            return JsonConvert.DeserializeObject<T>(JSON);
        }

    }
}
