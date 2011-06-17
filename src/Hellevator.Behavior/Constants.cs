using System;
using Microsoft.SPOT;

namespace Hellevator.Behavior
{
    public enum Destinations
    {
        Unknown  = 0,
        Heaven,
        Purgatory,
        Hell,
        BlackRockCity
    }

    public static class Floors
    {
        public const float BlackRockCity = 7;
    }
}
