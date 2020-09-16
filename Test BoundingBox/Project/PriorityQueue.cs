using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project
{
    // Min-Heap based Priority Queue
    //https://www.redblobgames.com/pathfinding/a-star/implementation.html 
    //https://www.dotnetlovers.com/article/231/priority-queue

    public class PriorityQueue<T, U> where T : IEquatable<T> where U : IComparable<U> //T is vector 2, U is the fcost
    {
        private List<Tuple<T, U>> element;
        private int count;

        public int GetCount()
        {
            return count;
        }

        public PriorityQueue()
        {
            element = new List<Tuple<T, U>>();
            count = -1;
        }

        public void Swap(int pos1, int pos2)
        {
            var temp = element[pos1];
            element[pos1] = element[pos2];
            element[pos2] = temp;
        }

        public int LeftChild(int pos)
        {
            return pos * 2 + 1;
        }

        public int RightChild(int pos)
        {
            return pos * 2 + 2;
        }

        public void MaxHeapify(int pos)
        {
            int left = LeftChild(pos);
            int right = RightChild(pos);

            int height = 1;

            if (left <= count && element[height].Item2.CompareTo(element[left].Item2) < 0)
                height = left;
            if (right <= count && element[height].Item2.CompareTo(element[right].Item2) < 0)
                height = right;

            if (height != pos)
            {
                Swap(height, pos);
                MaxHeapify(height);
            }
        }

        public void MinHeapify(int pos)
        {
            int left = LeftChild(pos);
            int right = RightChild(pos);
            int lowest = pos;

            if (left <= count && element[lowest].Item2.CompareTo(element[left].Item2) > 0)
                lowest = left;
            if (right <= count && element[lowest].Item2.CompareTo(element[right].Item2) > 0)
                lowest = right;

            if (lowest != pos)
            {
                Swap(lowest, pos);
                MinHeapify(lowest);
            }
        }

        public void BuildHeapMax(int pos)
        {
            while (pos >= 0 && element[(pos - 1) / 2].Item2.CompareTo(element[pos].Item2) < 0)
            {
                Swap(pos, (pos - 1) / 2);
                pos = (pos - 1) / 2;
            }
        }

        public void BuildHeapMin(int pos)
        {
            while (pos >= 0 && element[(pos - 1) / 2].Item2.CompareTo(element[pos].Item2) > 0)
            {
                Swap(pos, (pos - 1) / 2);
                pos = (pos - 1) / 2;
            }
        }

        public void Enqueue(T pos, U cost)
        {
            var tuple = new Tuple<T, U>(pos, cost);
            element.Add(tuple);
            count++;
            BuildHeapMin(count);
        }

        public T Dequeue()
        {
            if (count > -1)
            {
                var obj = element[0];
                element[0] = element[count];
                element.RemoveAt(count);
                count--;
                MinHeapify(0);
                return obj.Item1;
            }
            else
                throw new Exception("Queue is empty");
        }

        public bool Contain(T pos)
        {
            foreach (var tuple in element)
            {
                if (tuple.Item1.Equals(pos))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
