using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using MidiReader;
using NAudio.Midi;
using System.Media;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net.Http;
//using NAudio.Vorbis;

namespace MidiReader
{
  public class TelegramBot
  {
    private TelegramBotClient client;
    private static string folder;

    public TelegramBot(string token)
    {
      client = new TelegramBotClient(token);
      folder = "piano";
    }

    public void Start()
    {
      client.StartReceiving(Update, Error);
    }

    async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {

      var message = update.Message;
      //var audioMessage = messageEventArgs.Message;

      if (message != null)
      {
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
          new[]
          {
            new KeyboardButton("Bass"),
            new KeyboardButton("Piano")
          },
            new[]
          {
            new KeyboardButton("Brass"),
            new KeyboardButton("Windbox")
          }
        });


        if (message.Text == "Bass")
        {
          folder = "bass";
          await botClient.SendTextMessageAsync(message.Chat.Id, "Bass activated", replyMarkup: replyKeyboardMarkup);

          return;
        }

        if (message.Text == "Piano")
        {
          folder = "piano";
          await botClient.SendTextMessageAsync(message.Chat.Id, "Piano acticated", replyMarkup: replyKeyboardMarkup);

          return;
        }

        if (message.Text == "Brass")
        {
          folder = "brass";
          await botClient.SendTextMessageAsync(message.Chat.Id, "Brass activated", replyMarkup: replyKeyboardMarkup);

          return;
        }

        if (message.Text == "Windbox")
        {
          folder = "windbox";
          await botClient.SendTextMessageAsync(message.Chat.Id, "Windbox activated", replyMarkup: replyKeyboardMarkup);

          return;
        }

        if (message.Voice != null)
        {
          string oggFilePath = "C3.ogg";
          string wavFilePath = "C1.wav";
          var botToken = BotToken.Token;
          folder = "voice";

          var file = await botClient.GetFileAsync(message.Voice.FileId);
          //file.FilePath = "C1.ogg";

          using (var httpClient = new HttpClient())
          {
            var fileUrl = $"https://api.telegram.org/file/bot{BotToken.Token}/{"C1.ogg"}";
            var response = await httpClient.GetAsync(fileUrl);
            using (var fileOutput = System.IO.File.Create(oggFilePath))
            {
              await response.Content.CopyToAsync(fileOutput);
            }
          }

          //string tempWavFilePath = "temp.wav";
          //Program.ConvertOggToWav(oggFilePath, tempWavFilePath);

          // Перемещаем временный WAV файл в итоговую позицию
          //System.IO.File.Move(tempWavFilePath, wavFilePath);

          Console.WriteLine("Voice message saved");
          await botClient.SendTextMessageAsync(message.Chat.Id, "Голосовуха сохранена", replyMarkup: replyKeyboardMarkup);
        }



      }

    }

    private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
      throw new NotImplementedException();
    }

    public static string SetFolder()
    {

      return folder;


    }
  }
}