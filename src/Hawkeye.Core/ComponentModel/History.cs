using System;
using System.Collections.Generic;

namespace Hawkeye.ComponentModel
{
    /// <summary>
    /// Represents a navigation history list.
    /// </summary>
    internal class History<T>
    {
        private List<T> list = new List<T>();
        private int index = -1;

        public T Current 
        {
            get 
            {
                if (index == -1) return default(T);
                if (index >= list.Count) throw new InvalidOperationException();
                return list[index]; 
            }
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool HasPrevious
        {
            get { return index > 0; }
        }

        public bool HasNext
        {
            get { return index < list.Count - 1; }
        }

        public void Reset()
        {
            list.Clear();
            index = -1;
        }

        public void Push(T item)
        {
            list.Add(item);
            index = list.Count - 1;
        }

        public void MoveToPrevious()
        {
            if (!HasPrevious) throw new InvalidOperationException();
            index--;
        }

        public void MoveToNext()
        {
            if (!HasNext) throw new InvalidOperationException();
            index++;
        }
    }
}
