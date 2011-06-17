namespace Hellevator.Behavior.Interface
{
    public delegate void PressedEventHandler();

    public interface IButton
    {
        event PressedEventHandler Pressed;
    }
}