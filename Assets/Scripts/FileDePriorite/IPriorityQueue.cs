using System;
using System.Collections.Generic;

public interface IPriorityQueue<TItem, TPriority> : IEnumerable<TItem>
{
    void Enqueue(TItem node, TPriority priority);
    TItem Dequeue();
    void Clear();
    bool Contains(TItem node);
    void Remove(TItem node);
    void UpdatePriority(TItem node, TPriority priority);
    TItem First { get; }
    int Count { get; }
}
