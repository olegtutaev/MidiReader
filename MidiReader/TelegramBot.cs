﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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
          folder = "voice";

          await DownloadVoiceMessage(message.Voice.FileId);
          await botClient.SendTextMessageAsync(message.Chat.Id, "Загрузка в сэмплер...", replyMarkup: replyKeyboardMarkup);
          Console.WriteLine("Загрузка голоса в сэмплер...");

          Sampler.ConvertOggToWav();
          
          Sampler.ExportVoices();
          Console.WriteLine("Голос готов к использованию!");
          await botClient.SendTextMessageAsync(message.Chat.Id, "Голос готов к использованию!", replyMarkup: replyKeyboardMarkup);
        }



      }

    }

    private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
      
      throw new NotImplementedException();
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(new NotImplementedException());
      Console.ResetColor();
    }

    public static string SetFolder()
    {
      return folder;
    }

    private static async Task DownloadVoiceMessage(string fileId)
    {
      var voiceMessage = await client.GetFileAsync(fileId);

      using (var fileStream = new FileStream("C3.ogg", FileMode.OpenOrCreate))
      {
        await client.DownloadFileAsync(voiceMessage.FilePath, fileStream);
        Console.WriteLine("Скачалось");
      }
    }
  }
}