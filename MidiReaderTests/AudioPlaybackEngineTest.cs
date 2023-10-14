namespace MidiReaderTests
{
  public class Tests
  {
    [Test]
    public void PlaySound_MixerIsNotEmpty()
    {
      // Arrange
      string fileName = "C3.wav";
      AudioPlaybackEngine audioPlaybackEngine = new AudioPlaybackEngine(44100, 2);
      Type engineType = typeof(AudioPlaybackEngine);
      FieldInfo mixerField = engineType.GetField("mixer", BindingFlags.Instance | BindingFlags.NonPublic);
      MixingSampleProvider mixer = (MixingSampleProvider)mixerField.GetValue(audioPlaybackEngine);

      // Act
      audioPlaybackEngine.PlaySound(fileName);

      // Assert
      Assert.IsTrue(mixer.MixerInputs.Any());
    }
  }
}