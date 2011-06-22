using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hellevator.Simulator.ViewModels
{
    public class LimitedQueue<T> : Queue<T>
    {
        public int Limit { get; private set; }

        public LimitedQueue(int limit)
        {
            Limit = limit;
        }

        public new void Enqueue(T item)
        {
            if(Count >= Limit)
            {
                Dequeue();
            }
            base.Enqueue(item);
        }
    }
}
