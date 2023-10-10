using Concentus.Oggfile;
using SoundTouch.Net.NAudioSupport;
using SoundTouch;
using NAudio.Wave;
using Concentus.Structs;

namespace MidiReader
{
  public class Sampler
  {
    /// <summary>
    /// Данный метод ConvertOggToWav() служит для преобразования аудиофайла формата Ogg в формат WAV.

    ///Сначала указываются исходные и конечные файлы: fileOgg - имя исходного файла Ogg, fileWav - имя файла WAV, в который будет сохранен результат.

    ///Затем создается поток fileIn для чтения исходного файла Ogg и поток pcmStream в памяти для записи PCM данных.

    ///Далее создается экземпляр декодера OpusDecoder с параметрами частоты дискретизации (48000) и количества каналов(2).

    ///Создается экземпляр OpusOggReadStream, используя декодер и поток fileIn, для чтения и декодирования пакетов из файла Ogg.

    ///Затем происходит цикл, в котором происходит декодирование пакетов и запись PCM данных в поток pcmStream.

    ///Декодированные данные представлены в виде массива short[]. Каждое значение массива преобразуется в байты с помощью BitConverter.GetBytes() и записывается в поток pcmStream.

    ///После окончания чтения и декодирования пакетов, устанавливаем позицию потока pcmStream в начало.

    ///Создается экземпляр RawSourceWaveStream с использованием потока pcmStream и формата WAV (частота дискретизации 44100, 2 канала).

    ///Далее, с помощью WaveFileWriter.CreateWaveFile16(), создается файл WAV с именем fileWav, используя данные из sampleProvider.

    ///Наконец, освобождаем ресурсы, вызывая Dispose() для потока wavStream.

    ///Этот метод позволяет преобразовывать аудиофайлы формата Ogg в формат WAV с помощью декодирования и записи PCM данных.
    /// </summary>
    public static void ConvertOggToWav()
    {
      var fileOgg = "C3.ogg";
      var fileWav = "C3.wav";

      using (FileStream fileIn = new FileStream($"{fileOgg}", FileMode.Open))
      using (MemoryStream pcmStream = new MemoryStream())
      {
        OpusDecoder decoder = new OpusDecoder(48000, 2);
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
        using var wavStream = new RawSourceWaveStream(pcmStream, new WaveFormat(44100, 2));
        var sampleProvider = wavStream.ToSampleProvider();
        WaveFileWriter.CreateWaveFile16($"{fileWav}", sampleProvider);
        wavStream.Dispose();
      }
    }

    public static void ExportVoices()
    {
      for (int i = -24; i <= 24; i++)
      {
        string name = SemitonesMap.Names.GetValueOrDefault(i);
        string outputFilePath = @$"voice\{name}";
        ChangePitch("C3.wav", outputFilePath, i);
      }
    }

    public static void ChangePitch(string inputFile, string outputFile, int semitones)
    {
      using (var reader = new AudioFileReader(inputFile))
      {
        var processor = new SoundTouchProcessor();
        var soundTouchWaveProvider = new SoundTouchWaveProvider(reader, processor);

        soundTouchWaveProvider.Pitch = (float)Math.Pow(2, semitones / 12.0);

        WaveFileWriter.CreateWaveFile16(outputFile, soundTouchWaveProvider.ToSampleProvider());
      }
    }
  }
}
