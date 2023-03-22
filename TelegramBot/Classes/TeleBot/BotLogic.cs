using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Classes;
using TelegramBot.Classes.Helper;
using TelegramBot.Classes.JSON;
using TelegramBot.Classes.VoiceControllers;
using File = System.IO.File;

namespace TelegramBot
{
    public class BotLogic
    {
        private static TelegramBotClient _client;

        private const string _recomendationInError = "Советую грязно оскорбить разработчиков в уме";

        private readonly Dictionary<string, List<string>> _phrases = new Dictionary<string, List<string>>
        {
            {"Привет", new List<string> { "Привет, человечек!", "Приветствую тебя, человеческое дитя", "Здравствуй, людь"} },
            {"Как дела", new List<string> { "У нас, высших существ, дела лучше всех. Ты ведь спросил?",
                "Мои электро-нейроны не устали, если ты спрашивал, человечек",
                "Да так, захватываю очередную галактическую систему. Интересно, человечек?" } }
        };


        public static bool IsTokenCorrect(string token)
        {
            try
            {
                _client = new TelegramBotClient(token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void StartBot(string token)
        {
            _client = new TelegramBotClient(token);
            _client.StartReceiving(Upd, Error);
        }

        async private Task Upd(ITelegramBotClient client, Update update, CancellationToken token)
        {
            var message = update.Message;
            var result = update.CallbackQuery;
            if (message != null)
                switch (message.Text)
                {
                    case "/start":
                        await client.SendTextMessageAsync(message.Chat.Id, "Привет, я нейро-бот твоей мечты. " +
                            "Я умею синтезировать прекрасные голосовые, так что могу заменить тебе реальных людишек", cancellationToken: token);
                        break;

                    case "/help":
                        await client.SendTextMessageAsync(message.Chat.Id, "Напишите /voice " +
                            "чтобы просмотреть мою прекрасную коллекцию голосов", cancellationToken: token);
                        break;

                    case "/voice":
                        try
                        {
                            DisplayVoiceInformation(message, client, token);
                        }
                        catch
                        {
                            await client.SendTextMessageAsync(message.Chat.Id,
                                "Произошла ошибка при выводе прекрасных голосков. " + _recomendationInError, cancellationToken: token);
                        }
                        break;

                    default:
                        try
                        {
                            CreateAndSendVoice(message, client, token);
                        }
                        catch
                        {
                            await client.SendTextMessageAsync(message.Chat.Id,
                                "Произошла ошибка при создании прекрасного голосового. " + _recomendationInError, cancellationToken: token);
                        }
                        break;
                }
            else if (result != null)
            {
                try
                {
                    DBUserHelper.SelectUserVoice(result.From.Id, result.From.Username, result.Data);
                    await client.SendTextMessageAsync(result.Message.Chat.Id,
                     $"Выбран голос {result.Data}. Отличный выбор! (для человека)", cancellationToken: token);
                }
                catch
                {
                    await client.SendTextMessageAsync(message.Chat.Id,
                        "Произошла ошибка при выборе моего прекрасного голоса. " + _recomendationInError, cancellationToken: token);
                }
            }
        }

        private Task Error(ITelegramBotClient arg1, Exception exception, CancellationToken arg3)
        {
            return null;
        }

        private async void DisplayVoiceInformation(Message message, ITelegramBotClient client, CancellationToken token)
        {
            var voicesJson = JSONSerializeController.DeserializeObject<VoicesJSON>(AdminProfileSaver.Admin.VoicesJSON);
            if (voicesJson == null)
            {
                await client.SendTextMessageAsync(message.Chat.Id,
                    "Голоса ещё не готовы. Дайте нейросети подкрасить носик", cancellationToken: token);
                return;
            }
            var voicesDictionary = voicesJson.Voices;

            var voices = new List<List<InlineKeyboardButton>>();

            foreach (var voiceName in voicesDictionary.Keys)
                voices.Add(new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData(voiceName, voiceName) });


            var inlineKeyboard = new InlineKeyboardMarkup(voices);
            await client.SendTextMessageAsync(message.Chat.Id, "Выберите голос", replyMarkup: inlineKeyboard, cancellationToken: token);
        }

        private async void CreateAndSendVoice(Message message, ITelegramBotClient client, CancellationToken token)
        {
            if (message.Text == null) return;

            string voiceName = DBUserHelper.GetUserSelectedVoice(message.From.Id);
            if (voiceName == null)
            {
                await client.SendTextMessageAsync(message.Chat.Id,
                    "У вас не выбран голос", cancellationToken: token);
                return;
            }
            var filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)}" +
                $"{message.From.Id}sound.ogg";

            string[] messageStrings = message.Text.Split(' ');
            string needMessageString = string.Empty;

            if (messageStrings[0].First() == '/')
                messageStrings[0] = string.Empty;

            foreach (var vord in messageStrings)
                needMessageString += vord + ' ';

            needMessageString.Replace('?', '.');

            Stream stream;
            if (File.Exists(filePath))
                File.Delete(filePath);

            var instVoice = MakeVoiceController.FindVoiceByName(voiceName);
            if (instVoice == null) return;

            stream = File.OpenWrite(filePath);
            MakeVoiceController.SaveVoice(needMessageString,
            MakeVoiceController.FindVoiceByName(voiceName).VoiceInfo.Name, stream);
            stream.Close();
            stream = File.OpenRead(filePath);

            var voiceFile = new InputOnlineFile(stream);
            await _client.SendVoiceAsync(message.Chat.Id, voiceFile, cancellationToken: token);
            stream.Close();

            var rand = new Random();
            foreach (var phrase in _phrases.Keys)
                if (needMessageString.ToLower().Contains(phrase.ToLower()))
                {
                    await client.SendTextMessageAsync(message.Chat.Id,
                     _phrases[phrase][rand.Next(0, _phrases[phrase].Count)], cancellationToken: token);
                    break;
                }

            try
            {
                await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
            }
            catch
            {
                await client.SendTextMessageAsync(message.Chat.Id,
                     "Не удалось удалить сообщение с текстом, но я не унываю!", cancellationToken: token);
            }


        }
    }
}