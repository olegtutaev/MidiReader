using Sanford.Multimedia.Midi;

namespace MidiReader
{
  public class Program
  {
    private static string folder = "piano";
    private static int deviceNumber;

    private static byte[] CreateWavHeader(int dataSize)
    {
      int totalSize = 36 + dataSize; // Размер всего файла = размер заголовка (36 байт) + размер данных
      int audioFormat = 1; // PCM формат аудио данных
      int numChannels = 1; // Количество каналов (моно)
      int sampleRate = 16000; // Частота дискретизации
      int bitsPerSample = 16; // Разрядность (бит на сэмпл)

      byte[] header = new byte[44];

      // Chunk ID
      header[0] = (byte)'R';
      header[1] = (byte)'I';
      header[2] = (byte)'F';
      header[3] = (byte)'F';

      // Chunk Size
      header[4] = (byte)(totalSize & 0xFF);
      header[5] = (byte)((totalSize >> 8) & 0xFF);
      header[6] = (byte)((totalSize >> 16) & 0xFF);
      header[7] = (byte)((totalSize >> 24) & 0xFF);

      // Format
      header[8] = (byte)'W';
      header[9] = (byte)'A';
      header[10] = (byte)'V';
      header[11] = (byte)'E';

      // Subchunk 1 ID
      header[12] = (byte)'f';
      header[13] = (byte)'m';
      header[14] = (byte)'t';
      header[15] = (byte)' ';

      // Subchunk 1 Size
      header[16] = 16;
      header[17] = 0;
      header[18] = 0;
      header[19] = 0;

      // Audio Format
      header[20] = (byte)(audioFormat & 0xFF);
      header[21] = (byte)((audioFormat >> 8) & 0xFF);

      // Num Channels
      header[22] = (byte)(numChannels & 0xFF);
      header[23] = (byte)((numChannels >> 8) & 0xFF);

      // Sample Rate
      header[24] = (byte)(sampleRate & 0xFF);
      header[25] = (byte)((sampleRate >> 8) & 0xFF);
      header[26] = (byte)((sampleRate >> 16) & 0xFF);
      header[27] = (byte)((sampleRate >> 24) & 0xFF);

      // Byte Rate
      int byteRate = sampleRate * numChannels * bitsPerSample / 8;
      header[28] = (byte)(byteRate & 0xFF);
      header[29] = (byte)((byteRate >> 8) & 0xFF);
      header[30] = (byte)((byteRate >> 16) & 0xFF);
      header[31] = (byte)((byteRate >> 24) & 0xFF);

      // Block Align
      int blockAlign = numChannels * bitsPerSample / 8;
      header[32] = (byte)(blockAlign & 0xFF);
      header[33] = (byte)((blockAlign >> 8) & 0xFF);

      // Bits Per Sample
      header[34] = (byte)(bitsPerSample & 0xFF);
      header[35] = (byte)((bitsPerSample >> 8) & 0xFF);

      // Subchunk 2 ID
      header[36] = (byte)'d';
      header[37] = (byte)'a';
      header[38] = (byte)'t';
      header[39] = (byte)'a';

      // Subchunk 2 Size
      header[40] = (byte)(dataSize & 0xFF);
      header[41] = (byte)((dataSize >> 8) & 0xFF);
      header[42] = (byte)((dataSize >> 16) & 0xFF);
      header[43] = (byte)((dataSize >> 24) & 0xFF);

      return header;
    }


    public static void Main()
    {
      var bot = new TelegramBot(BotToken.Token);

      bot.Start();

      MidiDeviceLister deviceLister = new MidiDeviceLister();
      deviceLister.ListDevices();

      // чекнуть нажатия
      //MidiKeyboardListener keyboardListener = new MidiKeyboardListener();
      //keyboardListener.StartListening();

      Dictionary<int, string> noteMappings = new Dictionary<int, string>()
        {
          { 36, "C1.wav" }, // первая До на 4-октавной
          { 37, "C#1.wav"}, 
          { 38, "D1.wav"},
          { 39, "D#1.wav" },
          { 40, "E1.wav"},
          { 41, "F1.wav"},
          { 42, "F#1.wav"},
          { 43, "G1.wav"},
          { 44, "G#1.wav"},
          { 45, "A1.wav"},
          { 46, "A#1.wav"},
          { 47, "B1.wav"},
          { 48, "C2.wav" }, // вторая До на 4-октавной
          { 49, "C#2.wav"},
          { 50, "D2.wav"},
          { 51, "D#2.wav" },
          { 52, "E2.wav"},
          { 53, "F2.wav"},
          { 54, "F#2.wav"},
          { 55, "G2.wav"},
          { 56, "G#2.wav"},
          { 57, "A2.wav"},
          { 58, "A#2.wav"},
          { 59, "B2.wav"},
          { 60, "C3.wav" }, // третья До на 4-октавной
          { 61, "C#3.wav"},
          { 62, "D3.wav"},
          { 63, "D#3.wav" },
          { 64, "E3.wav"},
          { 65, "F3.wav"},
          { 66, "F#3.wav"},
          { 67, "G3.wav"},
          { 68, "G#3.wav"},
          { 69, "A3.wav"},
          { 70, "A#3.wav"},
          { 71, "B3.wav"},
          { 72, "C4.wav"}, // четвёртая До на 4-октавной
          { 73, "C#4.wav"},
          { 74, "D4.wav"},
          { 75, "D#4.wav" },
          { 76, "E4.wav"},
          { 77, "F4.wav"},
          { 78, "F#4.wav"},
          { 79, "G4.wav"},
          { 80, "G#4.wav"},
          { 81, "A4.wav"},
          { 82, "A#4.wav"},
          { 83, "B4.wav"},
          { 84, "C5.wav"} // пятая До на 4-октавной.

        };

      Console.WriteLine();
      Console.WriteLine("Введите номер MIDI-устройства");
      bool success = false;
      while (success == false)
      {
        string userInput = Console.ReadLine();
        success = int.TryParse(userInput, out deviceNumber) && (deviceNumber <= (InputDevice.DeviceCount - 1)) && (deviceNumber >= 0);

        if (success)
        {
          string deviceName = InputDevice.GetDeviceCapabilities(deviceNumber).name;
          Console.WriteLine($"Выборано устройство {deviceNumber}: {deviceName}");
        }
        else
        {
          Console.WriteLine("Некорректный ввод. Введите номер одного из предложенных устройств.");
        }
      }
      MidiPlayer midiPlayer = new MidiPlayer(noteMappings, folder, deviceNumber);
      midiPlayer.Start();

      Console.ReadLine();
    }

    public static void SetPiano(out string folder)
    {
      folder = "piano";
    }
  }
}

