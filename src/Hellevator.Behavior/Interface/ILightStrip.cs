using Hellevator.Behavior.Animations;

namespace Hellevator.Behavior.Interface
{
    public interface ILightStrip
    {
        int NumLights { get; }
        void SetColor(int light, Color color);
        void Update();
    }
}