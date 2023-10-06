using NAudio.Wave;

//Класс CachedSound используется в классе AudioPlaybackEngine для предварительной загрузки и кэширования звуковых данных.
//Класс CachedSound использует библиотеку NAudio для работы с звуковыми данными.
//При создании объекта CachedSound, звуковые данные из указанного потока звукового файла загружаются в память и сохраняются в массиве типа float для последующего использования.

namespace MidiReader
{
  class CachedSound
  {
    /// <summary>
    /// Класс CachedSound представляет звуковые данные, которые были предварительно загружены в память для последующего использования.
    /// </summary>
    public float[] AudioData { get; private set; }
    public WaveFormat WaveFormat { get; private set; }

    public CachedSound(Stream sound)
    {
      using (var audioFileReader = new WaveFileReader(sound))
      {
        WaveFormat = audioFileReader.WaveFormat;
        var sp = audioFileReader.ToSampleProvider();
        var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
        var sourceSamples = (int)(audioFileReader.Length / (audioFileReader.WaveFormat.BitsPerSample / 8));
        var sampleData = new float[sourceSamples];
        int samplesread;

        while ((samplesread = sp.Read(sampleData, 0, sourceSamples)) > 0)
        {
          wholeFile.AddRange(sampleData.Take(samplesread));
        }
        AudioData = wholeFile.ToArray();
      }
    }
  }
}
///Класс `CachedSound` используется для предварительной загрузки и кэширования звуковых данных из звукового файла. Он использует библиотеку NAudio для работы с аудио данными.

///Свойства класса `CachedSound`:

///- `AudioData`: массив типа `float`, который содержит звуковые данные из файла. Эти данные были предварительно загружены в память и могут быть использованы для воспроизведения
///звука.
///- `WaveFormat`: объект типа `WaveFormat`, который представляет формат звуковых данных, включая частоту дискретизации, битность и количество каналов.

///Конструктор класса `CachedSound` принимает поток `sound`, который содержит звуковой файл. Внутри конструктора происходит следующее:

///1.Создается объект `WaveFileReader` из потока `sound`, чтобы прочитать звуковые данные из файла.
///2. Значение свойства `WaveFormat` устанавливается равным формату звуковых данных из файла.
///3. Создается объект `SampleProvider` (`sp`), который преобразует звуковые данные из файла в формат `SampleProvider`, чтобы можно было читать данные по частям.
///4. Создается список `wholeFile`, который будет содержать все звуковые данные из файла.
///5. Вычисляется количество сэмплов (`sourceSamples`) в звуковых данных на основе длины файла и битности.
///6. Создается массив `sampleData` для временного хранения прочитанных сэмплов.
///7. В цикле считываются сэмплы из `sp` в `sampleData` и добавляются в список `wholeFile`.
///8. После окончания цикла, список `wholeFile` преобразуется в массив `AudioData`, содержащий все звуковые данные из файла.

///Теперь, после создания объекта `CachedSound`, вы можете использовать его свойство `AudioData` для воспроизведения звука или выполнения других операций с звуковыми данными.