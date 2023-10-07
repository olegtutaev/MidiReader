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
    private FileStream stream;

    public TelegramBot(string token)
    {
      client = new TelegramBotClient(token);
      folder = "piano";
    }

    public void Start()
    {
      client.StartReceiving(Update, Error);
    }


    public static void ConvertOggToWav()
    {
      var fileOgg = "C3.ogg";
      var fileWav = "C3.wav";

      using (FileStream fileIn = new FileStream($"{fileOgg}", FileMode.Open))
      using (MemoryStream pcmStream = new MemoryStream())
      {
        Concentus.Structs.OpusDecoder decoder = new Concentus.Structs.OpusDecoder(48000, 1);
        OpusOggReadStream oggIn = new OpusOggReadStream(decoder, fileIn);

        while (oggIn.HasNextPacket)
        {
          short[] packet = oggIn.DecodeNextPacket();
          if (packet != null)
          {
            for (int i = 0; i < packet.Length; i++)
            {
              var bytes = BitConverter.GetBytes(packet[i]);
              pcmStream.Write(bytes, 0, bytes.Length);
            }
          }
        }
        pcmStream.Position = 0;
        using var wavStream = new RawSourceWaveStream(pcmStream, new WaveFormat(44100, 1));
        var sampleProvider = wavStream.ToSampleProvider();
        WaveFileWriter.CreateWaveFile16($"{fileWav}", sampleProvider);
        
        
      }

      
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
          folder = "voice";

          await DownloadVoiceMessage(message.Voice.FileId);
          await botClient.SendTextMessageAsync(message.Chat.Id, "Загрузка в сэмплер...", replyMarkup: replyKeyboardMarkup);
          Console.WriteLine("Загрузка голоса в сэмплер...");

          ConvertOggToWav();
          Program.ExportVoices();
          Console.WriteLine("Голос готов к использованию!");
          await botClient.SendTextMessageAsync(message.Chat.Id, "Голос готов к использованию!", replyMarkup: replyKeyboardMarkup);
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

      using (var fileStream = new FileStream("C3.ogg", FileMode.OpenOrCreate))
      {
        await client.DownloadFileAsync(voiceMessage.FilePath, fileStream);
      }
    }
  }
}