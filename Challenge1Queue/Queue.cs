namespace Challenge1Queue
{
    /// <remarks>
    /// I made the Assumption that we don't want to use the C# queue https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.queue-1?view=net-7.0 
    /// </remarks>
    public class Queue<T>
    {
        private readonly List<T> _items = new List<T>();

        public void Enqueue(T item)
        {
            _items.Insert(0, item);
        }

        public T Dequeue()
        {
            var lastItem = _items.Last();
            _items.RemoveAt(_items.Count - 1);
            return lastItem;
        }

        public T Peek() => _items.Last();

        public int Count => _items.Count;

        public void Clear() 
        { 
            _items.Clear();
        }
    }
}