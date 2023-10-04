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
//using OpusDotNet;
using Concentus;
using Concentus.Oggfile;
using Concentus.Structs;
//using OpusDotNet;

namespace MidiReader
{
  public class TelegramBot
  {
    private static TelegramBotClient client;
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
          string filePath  = "C1.ogg";
          folder = "voice";

          await DownloadVoiceMessage(update.Message.Voice.FileId);
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

    private static async Task DownloadVoiceMessage(string fileId)
    {
      var voiceMessage = await client.GetFileAsync(fileId);

      using (var fileStream = new FileStream("C1.ogg", FileMode.Create))
      {
        await client.DownloadFileAsync(voiceMessage.FilePath, fileStream);
      }
    }
  }
}