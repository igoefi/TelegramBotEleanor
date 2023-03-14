using System;
using System.Collections.Generic;
using System.IO;
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
                        await client.SendTextMessageAsync(message.Chat.Id, "Приветствуем в tts боте", cancellationToken: token);

                        DBUserHelper.SelectUserVoice(message.From.Id, message.From.Username, MakeVoiceController.GetAllVoices()[0].VoiceInfo.Id);
                        return;

                    case "/help":
                        await client.SendTextMessageAsync(message.Chat.Id, "Напишите /voice чтобы выбрать голос", cancellationToken: token);
                        return;

                    case "/voice":
                        var voicesJson = JSONSerializeController.DeserializeObject<VoicesJSON>(AdminProfileSaver.Admin.VoicesJSON);
                        if (voicesJson == null)
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, 
                                "Голоса ещё не готовы. Дайте нейросети подкрасить носик", cancellationToken: token);
                            return;
                        }
                        var voicesDictionary = voicesJson.Voices;

                        var voices = new List<InlineKeyboardButton>();
                        foreach (var IDVoice in voicesDictionary.Keys)
                            voices.Add(InlineKeyboardButton.WithCallbackData(IDVoice, IDVoice));

                        var inlineKeyboard = new InlineKeyboardMarkup(new[] { voices });
                        await client.SendTextMessageAsync(message.Chat.Id, "Выберите голос", replyMarkup: inlineKeyboard, cancellationToken: token);
                        return;

                    default:
                        string voiceName = DBUserHelper.GetUserSelectedVoice(message.From.Id);

                        if (voiceName == null)
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, 
                                "У вас не выбран голос", cancellationToken: token);
                            return;
                        }
                        var filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)}" +
                            $"{message.From.Id}sound.ogg";

                        Stream stream;
                        if (File.Exists(filePath))
                        {
                            try
                            {
                                File.Delete(filePath);
                            }
                            catch
                            {

                            }

                        }


                        stream = File.OpenWrite(filePath);
                        MakeVoiceController.SaveVoice(message.Text,
                        MakeVoiceController.FindVoiceByID(voiceName).VoiceInfo.Name, stream);
                        stream.Close();
                        stream = File.OpenRead(filePath);

                        var voiceFile = new InputOnlineFile(stream);
                        await _client.SendVoiceAsync(message.Chat.Id, voiceFile, cancellationToken: token);
                        stream.Close();
                        return;
                }
            else if (result != null)
                DBUserHelper.SelectUserVoice(result.From.Id, result.From.Username, result.Data);
        }

        private Task Error(ITelegramBotClient arg1, Exception exception, CancellationToken arg3)
        {
            return null;
        }
    }
}