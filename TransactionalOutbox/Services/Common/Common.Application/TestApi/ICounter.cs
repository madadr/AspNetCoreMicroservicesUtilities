namespace Common.Application.TestApi
{
    public interface ICounter<T> where T : ICounterMarker
    {
        int Value { get; }
        void Increment();
        void Reset();
    }
}