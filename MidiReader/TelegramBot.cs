using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MidiReader
{
  /// <summary>
  /// Класс TelegramBot предоставляет функциональность для работы с Telegram-ботом.
  /// </summary>
  internal sealed class TelegramBot
  {
    #region Константы
    private const string bass = "Bass";
    private const string piano = "Piano";
    private const string brass = "Brass";
    private const string windbox = "Windbox";
    private const string voice = "Voice";
    private const string downloadingFile = "C3.ogg";
    #endregion

    #region Поля
    private static TelegramBotClient client;
    private static string folder;
    #endregion

    #region Методы
    /// <summary>
    /// Метод Start запускает процесс приема сообщений от пользователей.
    /// </summary>
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
            new KeyboardButton(bass),
            new KeyboardButton(piano)
          },
          new[]
          {
            new KeyboardButton(brass),
            new KeyboardButton(windbox)
          }
        });

        if (message.Text == bass)
        {
          folder = bass;
          await botClient.SendTextMessageAsync(message.Chat.Id, $"{bass} активирован", replyMarkup: replyKeyboardMarkup);
          Console.WriteLine($"{bass} активирован");

          return;
        }

        if (message.Text == piano)
        {
          folder = piano;
          await botClient.SendTextMessageAsync(message.Chat.Id, $"{piano} активирован", replyMarkup: replyKeyboardMarkup);
          Console.WriteLine($"{piano} активирован");

          return;
        }

        if (message.Text == brass)
        {
          folder = brass;
          await botClient.SendTextMessageAsync(message.Chat.Id, $"{brass} активирован", replyMarkup: replyKeyboardMarkup);
          Console.WriteLine($"{brass} активирован");

          return;
        }

        if (message.Text == windbox)
        {
          folder = windbox;
          await botClient.SendTextMessageAsync(message.Chat.Id, $"{windbox} активирован", replyMarkup: replyKeyboardMarkup);
          Console.WriteLine($"{windbox} активирован");

          return;
        }

        if (message.Voice != null)
        {
          await botClient.SendTextMessageAsync(message.Chat.Id, "Загрузка в сэмплер...", replyMarkup: replyKeyboardMarkup);
          Console.WriteLine("Загрузка голосового сообщения в сэмплер...");
          folder = null;
          await DownloadVoiceMessage(message.Voice.FileId);
          Thread.Sleep(1000); // Без задержки последний проигранный файл может не успеть освободится от чтения и возникнет ошибка при перезаписи.
          Sampler.ConvertOggToWav();         
          Sampler.ExportPitchedVoices();
          Console.WriteLine("Запись готова к использованию!");
          await botClient.SendTextMessageAsync(message.Chat.Id, "Запись готова к использованию!", replyMarkup: replyKeyboardMarkup);
          folder = voice;

          return;
        }
      }
    }

    private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {    
      throw new NotImplementedException();
    }

    /// <summary>
    /// Метод SetFolder возвращает текущую выбранную папку.
    /// </summary>
    /// <returns>Выбранная папка.</returns>
    public static string SetFolder()
    {
      return folder;
    }

    private static async Task DownloadVoiceMessage(string fileId)
    {
      var voiceMessage = await client.GetFileAsync(fileId);

      using (var fileStream = new FileStream(downloadingFile, FileMode.OpenOrCreate))
      {
        await client.DownloadFileAsync(voiceMessage.FilePath, fileStream);
      }
    }
    #endregion

    #region Конструктор
    /// <summary>
    /// Конструктор TelegramBot инициализирует экземпляр класса TelegramBotClient с указанным токеном.
    /// </summary>
    /// <param name="token">Токен Telegram-бота.</param>
    public TelegramBot(string token)
    {
      client = new TelegramBotClient(token);
      folder = piano;
    }
    #endregion
  }
}