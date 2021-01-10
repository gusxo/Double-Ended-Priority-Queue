using System;

namespace PriorityQueue
{
    public class PriorityQueue<T>
    {
        private class Node
        {
            public T Value { get; set; }
            public Node Parent { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public int LeftCount { get; set; }
            public int RightCount { get; set; }
            public Node() : this(default) { }
            public Node(T value)
            {
                Value = value;
                Parent = null;
                Left = null;
                Right = null;
                LeftCount = 0;
                RightCount = 0;
            }
        }

        private readonly Compare<T> compare;
        private Node root;

        private void AddParentCount(Node start,int value)
        {
            Node node = start;
            while(node.Parent != null)
            {
                if(node.Parent.Left == node)
                {
                    node.Parent.LeftCount += value;
                }
                else
                {
                    node.Parent.RightCount += value;
                }
                node = node.Parent;
            }
        }
        private void SwapValue(Node a, Node b)
        {
            T value = a.Value;
            a.Value = b.Value;
            b.Value = value;
        }
        private void UpHeap(Node start)
        {
            Node node = start;
            while(node.Parent != null)
            {
                if (compare(node.Parent.Value,node.Value))
                {
                    SwapValue(node, node.Parent);
                    node = node.Parent;
                }
                else break;
            }
        }
        private void DeleteNode(Node target)
        {
            Node node = target;
            while(node.Left != null || node.Right != null)
            {
                node = node.Right ?? node.Left;
            }
            SwapValue(target, node);
            AddParentCount(node,-1);
            if(node == root)
            {
                root = null;
            }
            else if(node.Parent.Left == node)
            {
                node.Parent.Left = null;
            }
            else
            {
                node.Parent.Right = null;
            }
            DownHeap(target);
        }
        private void DownHeap(Node start)
        {
            Node node = start;
            while(node.Left != null || node.Right != null)
            {
                Node select = node.Right ?? node.Left;
                if(node.Left != null && node.Right != null)
                {
                    select = compare(node.Left.Value, node.Right.Value) ? node.Right : node.Left;
                }

                if (compare(node.Value, select.Value))
                {
                    SwapValue(node, select);
                    node = select;
                }
                else break;
            }
        }

        /// <summary>
        /// Create a empty PriorityQueue. Prioirty is determined by compareMethod.
        /// </summary>
        public PriorityQueue(Compare<T> compareMethod)
        {
            compare = compareMethod ?? throw new ArgumentNullException(nameof(compareMethod));
            root = null;
        }
        /// <summary>
        /// Add a element to PrioirtyQueue.
        /// </summary>
        public void Enqueue(T element)
        {
            Node instant = new Node(element);
            if(root == null)
            {
                root = instant;
            }
            else if(Count < int.MaxValue)
            {
                Node node = root;
                while (true)
                {
                    if(node.Left == null)
                    {
                        node.Left = instant;
                        instant.Parent = node;
                        AddParentCount(instant,1);
                        UpHeap(instant);
                        break;
                    }
                    else if(node.Right == null)
                    {
                        node.Right = instant;
                        instant.Parent = node;
                        AddParentCount(instant, 1);
                        UpHeap(instant);
                        break;
                    }
                    else
                    {
                        node = node.LeftCount > node.RightCount ? node.Right : node.Left;
                    }
                }
            }
            else
            {
                throw new OverflowException("Doing Enqueue() will overflow the Count property.");
            }
        }
        /// <summary>
        /// The element with the highest priority is removed.
        /// </summary>
        /// <returns>Returns the element with the highest priority.</returns>
        public T Dequeue()
        {
            _ = root ?? throw new Exception("PriorityQueue is Empty.");
            T returnValue = root.Value;
            DeleteNode(root);
            return returnValue;
        }
        /// <summary>
        /// Returns the number of elements in the PrioirtyQueue.
        /// </summary>
        public int Count { get => root != null ? root.LeftCount + root.RightCount + 1 : 0; }
        /// <summary>
        /// Returns the element with the highest priority.
        /// </summary>
        public T Front()
        {
            return root != null ? root.Value : throw new Exception("PriorityQueue is Empty.");
        }
    }

    /// <summary>
    /// element compare method, if it returns <see langword="true"/> the prioirty of 'b' is higher.
    /// </summary>
    public delegate bool Compare<T>(T a, T b);
}
