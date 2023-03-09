namespace Challenge1QueueTests
{
    public class Tests
    {
        [Test]
        public void EnqueueShouldAddItemToQueue()
        {
            var queue = new Challenge1Queue.Queue<string>();
            queue.Enqueue("test");
            Assert.That(queue.Count, Is.EqualTo(1));
        }

        [Test]
        public void DequeueShouldRemoveFirstItemThatWasAddedToQueue()
        {
            var queue = new Challenge1Queue.Queue<string>();
            var firstItem = "fistItem";
            var lastItem = "lastItem";
            queue.Enqueue(firstItem);
            queue.Enqueue(lastItem);
            
            var dequeuedItem = queue.Dequeue();

            Assert.That(dequeuedItem, Is.EqualTo(firstItem));
            Assert.That(queue.Count, Is.EqualTo(1));
        }

        [Test]
        public void PeekShouldShowFirstItemThatWasAddedToQueueAndNotRemoveIt()
        {
            var queue = new Challenge1Queue.Queue<string>();
            var firstItem = "fistItem";
            var lastItem = "lastItem";
            queue.Enqueue(firstItem);
            queue.Enqueue(lastItem);

            var peekedItem = queue.Peek();

            Assert.That(peekedItem, Is.EqualTo(firstItem));
            Assert.That(queue.Count, Is.EqualTo(2));
        }
    }
}