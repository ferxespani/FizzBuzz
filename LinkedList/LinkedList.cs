using LinkedList;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace LikedList
{
    public class LinkedList<T> : IList<T>
    {
        private Node<T> head;
        public Node<T> Head => head;
        public int Count { get; private set; }
        public bool IsReadOnly => false;

        public LinkedList(params T[] values)
        {
            if (values is null)
                throw new ArgumentNullException(nameof(values), "Values is null");

            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i]);
            }
        }

        public LinkedList(IEnumerable<T> values)
        {
            if (values is null)
                throw new ArgumentNullException(nameof(values), "Values is null");

            var array = values.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                Add(array[i]);
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index > Count - 1)
                    CheckCollectionOnIndexRange(index);

                Node<T> node = head;
                int counter = 0;

                while (counter != Count)
                {
                    if (counter == index)
                        return node.Data;

                    node = node.Next;
                    counter++;
                }

                return default;
            }
            set
            {
                if (index < 0 || index > Count - 1)
                    CheckCollectionOnIndexRange(index);

                Node<T> node = head;
                int counter = 0;

                while (counter != Count)
                {
                    if (counter == index)
                        node.Data = value;

                    node = node.Next;
                    counter++;
                }
            }
        }

        public void Add(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item), "Argument is null");

            Node<T> node = new Node<T>(item);
            Node<T> current;
            if (head is null)
            {
                head = node;
            }
            else
            {
                current = head;

                while (current.Next != null)
                {
                    current = current.Next;
                }

                current.Next = node;
            }

            Count++;
        }


        public void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                RemoveAt(0);
            }

            head = null;
            Count = 0;
        }

        public bool Contains(T item)
        {
            Node<T> node = head;
            int counter = 0;

            while (node != null)
            {
                if (this[counter].Equals(item))
                    return true;

                node = node.Next;
                counter++;
            }

            return false;
        }

        public bool Remove(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item), "Argument is null");

            Node<T> current = head;
            Node<T> previous = head;
            int counter = 0;

            while (current != null)
            {
                if (this[counter].Equals(item))
                {
                    if (current.Next != null)
                    {
                        current.Data = current.Next.Data;
                        current.Next = current.Next.Next;
                    }
                    else
                    {
                        previous.Next = null;
                    }
                    Count--;

                    return true;
                }
                previous = current;
                current = current.Next;
                counter++;
            }

            return false;
        }

        public int IndexOf(T item)
        {
            Node<T> node = head;
            int counter = 0;

            while (node != null)
            {
                if (this[counter].Equals(item))
                    return counter;

                node = node.Next;
                counter++;
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item), "Argument is null");

            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index was out of range");
            }

            Node<T> current = head;
            Node<T> node;

            for (int i = 0; i <= Count; i++)
            {
                if (i == index)
                {
                    node = new Node<T>(current.Data)
                    {
                        Next = current.Next
                    };

                    current.Data = item;
                    current.Next = node;

                    Count++;

                    return;
                }

                if (current.Next is null)
                {
                    current.Next = new Node<T>(item);
                }
                current = current.Next;
            }
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index > Count - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index was out of range");
            }

            Node<T> node = head;
            int counter = 0;

            while (node != null)
            {
                if (counter == index)
                {
                    if (node.Next is null)
                    {
                        node = null;
                    }
                    else
                    {
                        node.Data = node.Next.Data;
                        node.Next = node.Next.Next;
                    }

                    Count--;
                }

                if (node is null)
                    return;

                node = node.Next;
                counter++;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array), "Argument is null");

            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("Array cannot contain list");

            var Node = head;

            for (int i = arrayIndex; i < arrayIndex + Count; i++)
            {
                array[i] = head.Data;

                Node = Node.Next;
            }
        }


        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnum<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void CheckCollectionOnIndexRange(int index)
        {
            IndexOutOfRangeException indexOutOfRangeException = new IndexOutOfRangeException("Index was out of range");

            if (index < 0 || index > Count - 1)
                throw indexOutOfRangeException;
        }

    }

    public class ListEnum<T> : IEnumerator<T>
    {
        public LinkedList<T> List { get; set; }

        int position = -1;
        bool disposed = false;
        private readonly SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        public ListEnum(LinkedList<T> list)
        {
            List = list;
        }



        public T Current
        {
            get
            {
                try
                {
                    return List[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current => Current;

        T IEnumerator<T>.Current => Current;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                _safeHandle?.Dispose();
            }

            disposed = true;
        }

        ~ListEnum()
        {
            Dispose(false);
        }

        public bool MoveNext()
        {
            position++;
            return (position < List.Count);
        }

        public void Reset()
        {
            position = -1;
        }
    }

}

