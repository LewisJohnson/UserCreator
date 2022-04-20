using System.Threading;

namespace UserCreator
{

    // We need a separate class as the generics mess up the atomic ID 
    public class IdGenerator
    {
        private static int nextId;

        public static int GetNextId()
        {
            nextId = Interlocked.CompareExchange(ref nextId, 0, 0);
            return Interlocked.Increment(ref nextId);
        }
    }
}