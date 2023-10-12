﻿namespace MidiReader
{
  public static class SemitonesMap
  {
    public static Dictionary<int, string> Semitones { get; } = new Dictionary<int, string>()
    {
      { -24, "C1.wav" }, // первая До на 4-октавной клавиатуре.
      { -23, "C#1.wav"},
      { -22, "D1.wav"},
      { -21, "D#1.wav" },
      { -20, "E1.wav"},
      { -19, "F1.wav"},
      { -18, "F#1.wav"},
      { -17, "G1.wav"},
      { -16, "G#1.wav"},
      { -15, "A1.wav"},
      { -14, "A#1.wav"},
      { -13, "B1.wav"},
      { -12, "C2.wav" }, // вторая До на 4-октавной клавиатуре.
      { -11, "C#2.wav"},
      { -10, "D2.wav"},
      { -9, "D#2.wav" },
      { -8, "E2.wav"},
      { -7, "F2.wav"},
      { -6, "F#2.wav"},
      { -5, "G2.wav"},
      { -4, "G#2.wav"},
      { -3, "A2.wav"},
      { -2, "A#2.wav"},
      { -1, "B2.wav"},
      { 0, "C3.wav" }, // третья До на 4-октавной клавиатуре.
      { 1, "C#3.wav"},
      { 2, "D3.wav"},
      { 3, "D#3.wav" },
      { 4, "E3.wav"},
      { 5, "F3.wav"},
      { 6, "F#3.wav"},
      { 7, "G3.wav"},
      { 8, "G#3.wav"},
      { 9, "A3.wav"},
      { 10, "A#3.wav"},
      { 11, "B3.wav"},
      { 12, "C4.wav"}, // четвёртая До на 4-октавной клавиатуре.
      { 13, "C#4.wav"},
      { 14, "D4.wav"},
      { 15, "D#4.wav" },
      { 16, "E4.wav"},
      { 17, "F4.wav"},
      { 18, "F#4.wav"},
      { 19, "G4.wav"},
      { 20, "G#4.wav"},
      { 21, "A4.wav"},
      { 22, "A#4.wav"},
      { 23, "B4.wav"},
      { 24, "C5.wav"} // пятая До на 4-октавной клавиатуре.
    };
  }
}
