using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T> {

    T[] items;
    int count;

    public Heap(int size) {
        items = new T[size];
    }

    public void Add(T item) {
        item.HeapIndex = count;
        items[count] = item;
        SortUp(item);
        count++;
    }

    public T RemoveFirst() {
        T firstItem = items[0];
        count--;
        items[0] = items[count];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item) {
        SortUp(item);
        //SortDown(item);
    }

    public int Count {
        get {
            return count;
        }
    }

    public bool Contains(T item) {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item) {
        while(true) {
            int childLeft = item.HeapIndex * 2 + 1;
            int childRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childLeft < count)
            {
                swapIndex = childLeft;

                if (childRight < count)
                {
                    if (items[childLeft].CompareTo(items[childRight]) < 0)
                    {
                        swapIndex = childRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                    Swap(item, items[swapIndex]);
                else
                    return;

            }
            else
                return;
        }
    }

    void SortUp(T item) {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true) {
            T parentItem = items[parentIndex];
            if(item.CompareTo(parentItem) > 0) {
                Swap(item, parentItem);
            }
            else {
                break;
            }
        }
    }

    void Swap(T A, T B) {
        items[A.HeapIndex] = B;
        items[B.HeapIndex] = A;
        int indexA = A.HeapIndex;
        A.HeapIndex = B.HeapIndex;
        B.HeapIndex = indexA;
    }
}

public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex {
        get;
        set;
    }
}
