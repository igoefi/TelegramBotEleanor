using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Classes;
using TelegramBot.Classes.Models;
using TelegramBot.Classes.VoiceControllers;
using File = System.IO.File;

namespace TelegramBot
{
    public class BotLogic
    {
        private const string _token = "5670601739:AAF0zxvcajbSdq5rLnetM35YiwDRurj9EEA";
        private static readonly TelegramBotClient _client = new TelegramBotClient(_token);

        public BotLogic()
        {
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

                        DBUserHelper.SelectUserVoice(message.From.Id, MakeVoiceController.GetAllVoices()[0].VoiceInfo.Id);
                        return;

                    case "/help":
                        await client.SendTextMessageAsync(message.Chat.Id, "Напишите /voice чтобы выбрать голос", cancellationToken: token);
                        return;

                    case "/voice":
                        var voices = new List<InlineKeyboardButton>();
                        foreach (var voice in MakeVoiceController.GetAllVoices())
                            voices.Add(InlineKeyboardButton.WithCallbackData(voice.VoiceInfo.Name, voice.VoiceInfo.Id));

                        var inlineKeyboard = new InlineKeyboardMarkup(new[] { voices });
                        await client.SendTextMessageAsync(message.Chat.Id, "Выберите голос", replyMarkup: inlineKeyboard, cancellationToken: token);
                        return;

                    default:
                        var filePath = $"{message.From.Id}sound.ogg";
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

                        string voiceID = DBUserHelper.GetUserSelectedVoice(message.From.Id);
                        if (voiceID == null) return;

                        stream = File.OpenWrite(filePath);
                        MakeVoiceController.SaveVoice(message.Text,
                        MakeVoiceController.FindVoiceByID(voiceID).VoiceInfo.Name, stream);
                        stream.Close();
                        stream = File.OpenRead(filePath);

                        var voiceFile = new InputOnlineFile(stream);
                        await _client.SendVoiceAsync(message.Chat.Id, voiceFile, cancellationToken: token);
                        stream.Close();
                        return;
                }
            else if (result != null)
                DBUserHelper.SelectUserVoice(result.From.Id, result.Data);
        }

        private Task Error(ITelegramBotClient arg1, Exception exception, CancellationToken arg3)
        {
            return null;
        }
    }
}