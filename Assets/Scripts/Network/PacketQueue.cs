using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class PacketQueue
    {
        Queue<ArraySegment<byte>> buffQueue = new Queue<ArraySegment<byte>>();
        object _lock = new object();

        public void Push(ArraySegment<byte> buff)
        {
            lock (_lock)
            {
                buffQueue.Enqueue(buff);
            }
        }

        public ArraySegment<byte>? Pop()
        {
            lock (_lock)
            {
                if (buffQueue.Count == 0)
                    return null;

                return buffQueue.Dequeue();
            }
        }

        public List<ArraySegment<byte>> PopAll()
        {
            List<ArraySegment<byte>> list = new List<ArraySegment<byte>>();

            lock (_lock)
            {
                while (buffQueue.Count > 0)
                    list.Add(buffQueue.Dequeue());
            }

            return list;
        }
    }
}
