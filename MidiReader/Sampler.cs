using Concentus.Oggfile;
using SoundTouch.Net.NAudioSupport;
using SoundTouch;
using NAudio.Wave;
using Concentus.Structs;

namespace MidiReader
{
  /// <summary>
  /// Класс Sampler предоставляет методы для конвертации Ogg-файла в WAV-файл и изменения тональности звуковых файлов.
  /// </summary>
  internal sealed class Sampler
  {
    #region Константы
    private const int decoderSampleRate = 48000;  // OpusDecoder не поддерживает частоту дискретизации 44100 кГц.
    private const int wavSampleRate = 44100;
    private const int channelCount = 2;
    private const double semitonesOctave = 12.0;
    private const string folder = "Voice";
    #endregion

    #region Поля
    private static string fileOgg = "C3.ogg";
    private static string fileWav = "C3.wav";
    #endregion

    #region Методы
    /// <summary>
    /// Метод ConvertOggToWav выполняет конвертацию Ogg-файла в WAV-файл.
    /// </summary>
    public static void ConvertOggToWav()
    {
      using (var fileIn = new FileStream($"{fileOgg}", FileMode.Open))
      using (var pcmStream = new MemoryStream())
      {
        var decoder = new OpusDecoder(decoderSampleRate, channelCount);
        var oggIn = new OpusOggReadStream(decoder, fileIn);

        while (oggIn.HasNextPacket)
        {
          var packet = oggIn.DecodeNextPacket();

          if (packet != null)
          {
            for (var i = 0; i < packet.Length; i++)
            {
              var bytes = BitConverter.GetBytes(packet[i]);
              pcmStream.Write(bytes, 0, bytes.Length);
            }
          }
        }
        pcmStream.Position = 0;
        using var wavStream = new RawSourceWaveStream(pcmStream, new WaveFormat(wavSampleRate, channelCount));
        var sampleProvider = wavStream.ToSampleProvider();
        WaveFileWriter.CreateWaveFile16($"{fileWav}", sampleProvider);
      }
    }

    /// <summary>
    /// Метод ExportPitchedVoices экспортирует звуковые файлы с измененной тональностью.
    /// </summary>
    public static void ExportPitchedVoices()
    {
      var semitones = SemitonesMap.Semitones.Keys;

      for (var i = semitones.FirstOrDefault(); i <= semitones.Last(); i++)
      {
        var name = SemitonesMap.Semitones.GetValueOrDefault(i);
        var outputFilePath = @$"{folder}\{name}";
        ChangePitch(fileWav, outputFilePath, i);
      }
    }

    /// <summary>
    /// Метод ChangePitch изменяет тональность звукового файла.
    /// </summary>
    /// <param name="inputFile">Входной WAV-файл.</param>
    /// <param name="outputFile">Выходной WAV-файл.</param>
    /// <param name="semitones">Количество полутонов для изменения тональности.</param>
    public static void ChangePitch(string inputFile, string outputFile, int semitones)
    {
      using (var reader = new AudioFileReader(inputFile))
      {
        var processor = new SoundTouchProcessor();
        var soundTouchWaveProvider = new SoundTouchWaveProvider(reader, processor);
        soundTouchWaveProvider.Pitch = (float)Math.Pow(2, semitones / semitonesOctave);
        WaveFileWriter.CreateWaveFile16(outputFile, soundTouchWaveProvider.ToSampleProvider());
      }
    }
    #endregion
  }
}
