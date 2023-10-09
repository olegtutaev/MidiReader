using Concentus.Oggfile;
using SoundTouch.Net.NAudioSupport;
using SoundTouch;
using NAudio.Wave;
using Concentus.Structs;

namespace MidiReader
{
  public class Sampler
  {
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

        soundTouchWaveProvider.Rate = (float)Math.Pow(2, semitones / 12.0);

        WaveFileWriter.CreateWaveFile16(outputFile, soundTouchWaveProvider.ToSampleProvider());
      }
    }
  }
}
