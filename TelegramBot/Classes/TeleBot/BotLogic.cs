using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Classes;
using TelegramBot.Classes.Helper;
using TelegramBot.Classes.JSON;
using TelegramBot.Classes.Models;
using TelegramBot.Classes.VoiceControllers;
using File = System.IO.File;

namespace TelegramBot
{
    public class BotLogic
    {
        private static TelegramBotClient _client;
        private static CancellationToken _token;

        private const string _recomendationInError = "Советую грязно оскорбить разработчиков в уме";

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
            //Ужаснейший костыль, но время жестоко
            _token = token;

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
                            "чтобы просмотреть мою прекрасную коллекцию голосов. Или же /sub, чтобы подписаться на уведомления МГОК-а", cancellationToken: token);
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

                    case "/sub":
                        try
                        {
                            Subscribe(message.Chat.Id);

                            await client.SendTextMessageAsync(message.Chat.Id,
                                "Ты подписал этот чат на уведомления МГОК-а, человечек. Мудрое решение", cancellationToken: token);
                        }
                        catch
                        {
                            await client.SendTextMessageAsync(message.Chat.Id,
                                "Произошла ошибка при подписке. " + _recomendationInError, cancellationToken: token);
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
                    DBUserHelper.SelectUserVoice(result.Message.Chat.Id, result.Message.Chat.FirstName, result.Data);
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

        private void Subscribe(long chatID) =>
            DBUserHelper.SelectUserSubInDB(chatID);

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

        private async void CreateAndSendVoice(Message message,  ITelegramBotClient client, CancellationToken token)
        {
            if (message == null) return;

            string voiceName = DBUserHelper.GetUserSelectedVoice(message.Chat.Id);
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

            var answers = DataBaseCC.ConnectContext.QuestionAnswer.ToList();
            var phrases = new Dictionary<string, List<string>>();

            foreach (var QA in answers)
                phrases.Add(QA.Question, JSONSerializeController.DeserializeObject<List<string>>(QA.AnswersJSON));

            foreach (var phrase in phrases.Keys)
                if (needMessageString.ToLower().Contains(phrase.ToLower()))
                {
                    await client.SendTextMessageAsync(message.Chat.Id,
                     phrases[phrase][rand.Next(0, phrases[phrase].Count)], cancellationToken: token);
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

        public async static void SendMessage(string message)
        {
            if(_token == null || _client == null)
            {
                MessageBox.Show("Сначала запустите бота");
                return;
            }

            var users = DBUserHelper.GetUsers();
            if(users == null)
            {
                MessageBox.Show("Некому отправлять бота");
                return;
            }

            var subscribedUsers = new Dictionary<long, string>();
            foreach (var user in users)
                if (user.IsSubscribed)
                    subscribedUsers.Add(user.ID, user.SelectedVoice);

            foreach(var ID in subscribedUsers.Keys)
            {
                await _client.SendTextMessageAsync(ID, message, cancellationToken: _token);

                string voiceName = DBUserHelper.GetUserSelectedVoice(ID);
                if (voiceName == null) return;

                var filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)}" +
                    $"{ID}sound.ogg";

                string[] messageStrings = message.Split(' ');
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
                await _client.SendVoiceAsync(ID, voiceFile, cancellationToken: _token);
                stream.Close();
            }

        }
    }
}