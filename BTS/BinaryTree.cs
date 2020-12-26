using System;
using System.Collections;
using System.Collections.Generic;

namespace BTS
{
    public class BinaryTree<T> : IEnumerable<T>
    {
        private Node<T> root;
        private readonly IComparer<T> comparer;

        public int Count { get; private set; }

        public Node<T> Root
        {
            get
            {
                if (root != null)
                    return root;
                else
                    return default;
            }
        }

        public BinaryTree()
        {
            if (typeof(T).GetInterface(typeof(IComparable<T>).Name) == typeof(IComparable<T>))
            {
                comparer = Comparer<T>.Default;
            }
            else
                throw new ArgumentException($"{typeof(T)} does not implement IComparable");
        }

        public void Add(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item), "Item is null");

            if (root is null)
            {
                root = new Node<T>(item);
                Count++;

                return;
            }

            AddToSide(root, item);
            Count++;
        }

        private void AddToSide(Node<T> current, T item)
        {
            if (comparer.Compare(item, current.Data) == -1)
            {
                if (current.Left is null)
                {
                    current.Left = new Node<T>(item);
                }
                else
                {
                    AddToSide(current.Left, item);
                }
            }
            else
            {
                if (current.Right is null)
                {
                    current.Right = new Node<T>(item);
                }
                else
                {
                    AddToSide(current.Right, item);
                }
            }
        }

        public bool Remove(T item)
        {
            Node<T> current;

            current = SearchNode(item, out Node<T> parent);

            if (current is null)
                return false;


            if (parent is null)
            {
                RemoveRoot(current);

                Count--;
                return true;
            }

            if (current.Right is null && current.Left is null)
            {
                Comparison(current, parent, 0);
            }
            else if (current.Left is null && !(current.Right is null))
            {
                Comparison(current, parent, 1);
            }
            else if (!(current.Left is null) && current.Right is null)
            {
                Comparison(current, parent, 2);
            }
            else
            {
                if (comparer.Compare(current.Data, parent.Data) == -1)
                {
                    parent.Left = SearchMinOfSubTree(current, out Node<T> left, out Node<T> right);
                    parent.Left.Left = left;
                    parent.Left.Right = right;
                }
                else
                {
                    parent.Right = SearchMinOfSubTree(current, out Node<T> left, out Node<T> right);
                    parent.Left.Left = left;
                    parent.Left.Right = right;
                }
            }
            Count--;

            return true;
        }

        private void Comparison(Node<T> current, Node<T> parent, int choice)
        {
            if (choice == 0)
            {
                if (comparer.Compare(current.Data, parent.Data) == -1)
                    parent.Left = null;
                else
                    parent.Right = null;
            }
            else if (choice == 1)
            {
                if (comparer.Compare(current.Data, parent.Data) == -1)
                {
                    parent.Left = current.Right;
                }
                else
                {
                    parent.Right = current.Right;
                }
            }
            else
            {
                if (comparer.Compare(current.Data, parent.Data) == -1)
                {
                    parent.Left = current.Left;
                }
                else
                {
                    parent.Right = current.Left;
                }
            }
        }

        private void RemoveRoot(Node<T> current)
        {
            if (Count == 1)
            {
                root = null;
            }
            else if (current.Left != null && current.Right != null)
            {
                root = SearchMinOfSubTree(current, out Node<T> left, out Node<T> right);
                root.Left = left;
                root.Right = right;
            }
            else if (current.Right is null)
            {
                root = current.Left;
            }
            else
            {
                root = current.Right;
            }
        }
        private Node<T> SearchMinOfSubTree(Node<T> current, out Node<T> left, out Node<T> right)
        {
            if (current.Right is null)
            {
                right = null;
                if (current.Left is null)
                {
                    left = null;
                }
                else
                {
                    left = current.Left;
                }

                return current;
            }
            else
            {
                if (current.Left is null)
                {
                    left = null;
                }
                else
                {
                    left = current.Left;
                }

                current = current.Right;
            }

            while (current.Left != null)
            {
                current = current.Left;
            }

            right = current.Right;

            return current;
        }

        public T TreeMax()
        {
            if (Count == 0)
                throw new InvalidOperationException("Tree is empty");

            Node<T> current = root;

            while (current.Right != null)
            {
                current = current.Right;
            }

            return current.Data;
        }

        public T TreeMin()
        {
            if (Count == 0)
                throw new InvalidOperationException("Tree is empty");

            Node<T> current = root;

            while (current.Left != null)
            {
                current = current.Left;
            }

            return current.Data;
        }

        public bool Contains(T data)
        {
            return SearchNode(data, out Node<T> parent) != null;
        }

        private Node<T> SearchNode(T value, out Node<T> parent)
        {
            var current = root;
            parent = null;

            while (current != null)
            {
                int result = comparer.Compare(value, current.Data);

                if (result < 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (result > 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else
                {
                    break;
                }
            }

            return current;
        }

        public List<T> Traverse()
        {
            return InOrderTraverse(root);
        }

        private List<T> InOrderTraverse(Node<T> item)
        {
            var list = new List<T>();
            if (item != null)
            {
                if (item.Left != null)
                {
                    list.AddRange(InOrderTraverse(item.Left));
                }

                list.Add(item.Data);

                if (item.Right != null)
                {
                    list.AddRange(InOrderTraverse(item.Right));
                }
            }

            return list;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new BinaryTreeEnum<T>(InOrderTraverse(root));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class BinaryTreeEnum<T> : IEnumerator<T>
    {
        public List<T> List { get; set; }
        int position = -1;
        bool disposed = false;

        public BinaryTreeEnum(List<T> list)
        {
            List = list;
        }
        public T Current => List[position];

        object IEnumerator.Current => Current;

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
                
            }

            disposed = true;
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
