using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> heap;

    public PriorityQueue()
    {
        heap = new List<T>();
    }

    public int Count => heap.Count;

    public void Enqueue(T item)
    {
        heap.Add(item);
        HeapifyUp(heap.Count - 1);
    }

    public T Dequeue()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("The priority queue is empty.");

        T root = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        HeapifyDown(0);

        return root;
    }

    public T Peek()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("The priority queue is empty.");

        return heap[0];
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            if (heap[index].CompareTo(heap[parentIndex]) >= 0)
                break;

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        int lastIndex = heap.Count - 1;

        while (true)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int smallestIndex = index;

            if (leftChildIndex <= lastIndex && heap[leftChildIndex].CompareTo(heap[smallestIndex]) < 0)
                smallestIndex = leftChildIndex;

            if (rightChildIndex <= lastIndex && heap[rightChildIndex].CompareTo(heap[smallestIndex]) < 0)
                smallestIndex = rightChildIndex;

            if (smallestIndex == index)
                break;

            Swap(index, smallestIndex);
            index = smallestIndex;
        }
    }

    private void Swap(int i, int j)
    {
        T temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
}
