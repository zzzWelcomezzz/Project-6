using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomPriorityQueue
{
    private RoomTuple[] roomHeap;
    public int queueSize { get; private set; }
    public List<Room> path;

    public RoomPriorityQueue(int queueSize)
    {
        if (queueSize <= 0)
        {
            queueSize = 1;
        }

        this.roomHeap = new RoomTuple[queueSize + 1]; //since roomHeap[0] isn't used, we need an extra element to store 's' items
        this.queueSize = 0;
    }

    public void Add(RoomTuple roomTuple)
    {
        if (queueSize >= roomHeap.Length - 1)
        {
            //TODO: change?
            Debug.LogError("PQ Full: queueSize = " + queueSize);
            return;
        }

        int index = queueSize + 1;
        roomHeap[index] = roomTuple;

        while (index > 1)
        {
            int parentIndex = index / 2;
            if (roomHeap[parentIndex].room.weight < roomTuple.room.weight)
            {
                roomHeap[index] = roomHeap[parentIndex];
                roomHeap[parentIndex] = roomTuple;

                index = parentIndex;
            }
            else { break; }
        }

        this.queueSize++;
    }

    public RoomTuple Remove()
    {
        if (IsEmpty())
        {
            return null;
        }

        RoomTuple root = roomHeap[1];

        RoomTuple last = roomHeap[queueSize];
        roomHeap[1] = last;
        roomHeap[queueSize] = null;

        int index = 1;
        while (index * 2 < queueSize)
        {
            int leftChild = index * 2;
            int rightChild = (index * 2) + 1;

            float leftChildValue = roomHeap[leftChild].room.weight;
            float rightChildValue = -1.0f;

            if (rightChild < queueSize)
            {
                rightChildValue = roomHeap[rightChild].room.weight;
            }

            int maxIndex;
            if (leftChildValue >= rightChildValue)
            {
                maxIndex = leftChild;
            }
            else
            {
                maxIndex = rightChild;
            }

            RoomTuple maxChild = roomHeap[maxIndex];

            if (last.room.weight < maxChild.room.weight)
            {
                roomHeap[index] = maxChild;
                roomHeap[maxIndex] = last;
                index = maxIndex;
            }
            else { break; }
        }

        this.queueSize--;
        return root;
    }

    public bool IsEmpty()
    {
        return roomHeap[1] == null;
    }

    public RoomTuple GetFirst()
    {
        return roomHeap[1];
    }
}
