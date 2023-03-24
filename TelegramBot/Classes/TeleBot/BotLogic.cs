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

        public void StartBot(string token) =>
            Start(token);

        private static void Start(string token)
        {
            _client = new TelegramBotClient(token);
            _client.StartReceiving(Upd, Error);
        }

        async static private Task Upd(ITelegramBotClient client, Update update, CancellationToken token)
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
                            DisplayVoiceInformation(message);
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
                            string voiceName = DBUserHelper.GetUserSelectedVoice(message.Chat.Id);
                            if (voiceName == null)
                            {
                                await client.SendTextMessageAsync(message.Chat.Id,
                                    "У вас не выбран голос", cancellationToken: token);
                                return;
                            }

                            CreateAndSendVoice(message.Text, message.Chat.Id, voiceName);

                            SendRandomAnswer(message.Text, message.Chat.Id);

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

        private static Task Error(ITelegramBotClient arg1, Exception exception, CancellationToken arg3)
        {
            MessageBox.Show("Произошла ошибка при работе бота. Подробнее:" + exception.Message.ToString());           

            return null;
        }

        public async static void SendMessage(string message)
        {
            if (_token == null || _client == null)
            {
                if (string.IsNullOrWhiteSpace(AdminProfileSaver.Admin.Token))
                {
                    MessageBox.Show("Сначала укажите токен");
                    return;
                }
                Start(AdminProfileSaver.Admin.Token);
            }

            var users = DBUserHelper.GetUsers();
            if (users == null)
            {
                MessageBox.Show("Некому отправлять сообщение");
                return;
            }

            var subscribedUsers = new Dictionary<long, string>();
            foreach (var user in users)
                if (user.IsSubscribed)
                    subscribedUsers.Add(user.ID, user.SelectedVoice);

            foreach (var ID in subscribedUsers.Keys)
            {
                await _client.SendTextMessageAsync(ID, message, cancellationToken: _token);

                CreateAndSendVoice(message, ID, subscribedUsers[ID]);
            }

        }

        private static void Subscribe(long chatID) =>
            DBUserHelper.SelectUserSubInDB(chatID);

        private async static void DisplayVoiceInformation(Message message)
        {
            var voicesJson = JSONSerializeController.DeserializeObject<VoicesJSON>(AdminProfileSaver.Admin.VoicesJSON);
            if (voicesJson == null)
            {
                await _client.SendTextMessageAsync(message.Chat.Id,
                    "Голоса ещё не готовы. Дайте нейросети подкрасить носик", cancellationToken: _token);
                return;
            }
            var voicesDictionary = voicesJson.Voices;

            var voices = new List<List<InlineKeyboardButton>>();

            foreach (var voiceName in voicesDictionary.Keys)
                voices.Add(new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData(voiceName, voiceName) });


            var inlineKeyboard = new InlineKeyboardMarkup(voices);
            await _client.SendTextMessageAsync(message.Chat.Id, "Выберите голос", replyMarkup: inlineKeyboard, cancellationToken: _token);
        }

        private async static void CreateAndSendVoice(string message, long chatID, string voiceName)
        {
            if (message == null) return;

            var filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)}" +
                $"{chatID}sound.ogg";

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
            await _client.SendVoiceAsync(chatID, voiceFile, cancellationToken: _token);
            stream.Close();
        }

        private async static void SendRandomAnswer(string message, long chatID)
        {
            var rand = new Random();

            var answers = DataBaseCC.ConnectContext.QuestionAnswer.ToList();
            var phrases = new Dictionary<string, List<string>>();

            foreach (var QA in answers)
                phrases.Add(QA.Question, JSONSerializeController.DeserializeObject<List<string>>(QA.AnswersJSON));

            foreach (var phrase in phrases.Keys)
                if (message.ToLower().Contains(phrase.ToLower()))
                {
                    await _client.SendTextMessageAsync(chatID,
                     phrases[phrase][rand.Next(0, phrases[phrase].Count)], cancellationToken: _token);
                    break;
                }
        }
    }
}