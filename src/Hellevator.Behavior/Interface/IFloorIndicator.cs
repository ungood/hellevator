using System;

namespace Hellevator.Behavior.Interface
{
    public interface IFloorIndicator
    {
        double Floor { get; set; }
        void Flicker();
        void TurnOff();

        void SetLight(int i, double intensity);
    }

    public static class Floors
    {
        public const int Total = 24;

        public const int BlackRockCity = 7;
    }

    public static class FloorIndicatorHelper
    {
        public static void UpdateLights(this IFloorIndicator indicator)
        {
            var floor = (int)Math.Floor(indicator.Floor);
            var ceil = (int)Math.Ceiling(indicator.Floor);
            var intensity = indicator.Floor - floor;

            for(int i = 1; i <= Floors.Total; i++)
            {
                if(i == floor && i == ceil)
                    indicator.SetLight(i, 1.0);
                else if(i == floor)
                    indicator.SetLight(i, intensity);
                else if(i == ceil)
                    indicator.SetLight(i, 1.0 - intensity);
                else
                    indicator.SetLight(i, 0);
            }
        }
    }
}
