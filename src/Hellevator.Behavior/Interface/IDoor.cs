using System;

namespace Hellevator.Behavior.Interface
{
    public interface IDoor
    {
        // TODO: IsClosed - don't rotate the damned box till the door is shut.
        void Open();
        void Close();
    }
}
