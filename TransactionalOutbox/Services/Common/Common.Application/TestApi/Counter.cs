using System.Threading;

namespace Common.Application.TestApi
{
    public class Counter<T> : ICounter<T> where T : ICounterMarker
    {
        private int _value;
        public int Value => _value;

        public Counter()
        {
            Reset();
        }

        public void Increment()
        {
            Interlocked.Increment(ref _value);
        }

        public void Reset()
        {
            _value = 0;
        }
    }
}