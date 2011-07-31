using Hellevator.Behavior.Animations;
using Hellevator.Behavior.Effects;

namespace Hellevator.Behavior.Interface
{
    public interface ILightStrip
    {
        int NumLights { get; }
        void SetColor(int light, Color color);
        void Update();
    }
}