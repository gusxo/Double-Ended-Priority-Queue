using System;

namespace PriorityQueue
{
    public class DEPriorityQueue<T>
    {
        private class Node
        {
            public T Value { get; set; }
            public Node Another { get; set; }
            public Node Parent { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public int LeftCount { get; set; }
            public int RightCount { get; set; }
            public Node() : this(default) { }
            public Node(T value)
            {
                Value = value;
                Another = null;
                Parent = null;
                Left = null;
                Right = null;
                LeftCount = 0;
                RightCount = 0;
            }
        }

        private readonly Compare<T> maxCompare;
        private readonly Compare<T> minCompare;
        private Node maxRoot;
        private Node minRoot;

        private void AddParentCount(Node start, int value)
        {
            Node node = start;
            while (node.Parent != null)
            {
                if (node.Parent.Left == node)
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
            b.Another.Another = a;
            a.Another.Another = b;
            T value = a.Value;
            Node another = a.Another;
            a.Value = b.Value;
            a.Another = b.Another;
            b.Value = value;
            b.Another = another;
        }
        private void UpHeap(Node start, Compare<T> compare)
        {
            Node node = start;
            while (node.Parent != null)
            {
                if (compare(node.Parent.Value, node.Value))
                {
                    SwapValue(node, node.Parent);
                    node = node.Parent;
                }
                else break;
            }
        }
        private void DeleteNode(Node target, Priority priority)
        {
            Node node = target;
            while (node.Left != null || node.Right != null)
            {
                node = node.Right ?? node.Left;
            }
            SwapValue(target, node);
            AddParentCount(node, -1);
            if (priority == Priority.Highest && maxRoot == node)
            {
                maxRoot = null;
            }
            else if (priority == Priority.Lowest && minRoot == node)
            {
                minRoot = null;
            }
            else if (node.Parent.Left == node)
            {
                node.Parent.Left = null;
            }
            else
            {
                node.Parent.Right = null;
            }
            DownHeap(target, priority == Priority.Highest? maxCompare:minCompare);
        }
        private void DownHeap(Node start, Compare<T> compare)
        {
            Node node = start;
            while (node.Left != null || node.Right != null)
            {
                Node select = node.Right ?? node.Left;
                if (node.Left != null && node.Right != null)
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
        private void Insert(Node root, Node instant, Priority priority)
        {
            Node node = root;
            while (true)
            {
                if (node.Left == null)
                {
                    node.Left = instant;
                    instant.Parent = node;
                    AddParentCount(instant, 1);
                    UpHeap(instant, priority == Priority.Highest ? maxCompare : minCompare);
                    break;
                }
                else if (node.Right == null)
                {
                    node.Right = instant;
                    instant.Parent = node;
                    AddParentCount(instant, 1);
                    UpHeap(instant, priority == Priority.Highest ? maxCompare : minCompare);
                    break;
                }
                else
                {
                    node = node.LeftCount > node.RightCount ? node.Right : node.Left;
                }
            }
        }

        /// <summary>
        /// Create a empty Double-Ended-PriorityQueue. Prioirty is determined by compareMethod.
        /// </summary>
        public DEPriorityQueue(Compare<T> compareMethod)
        {
            maxCompare = compareMethod ?? throw new ArgumentNullException(nameof(compareMethod));
            minCompare = (a, b) => { return !maxCompare(a, b); };
            maxRoot = minRoot = null;
        }
        /// <summary>
        /// Add a element to Double-Ended-PrioirtyQueue.
        /// </summary>
        public void Enqueue(T element)
        {
            Node maxNode = new Node(element);
            Node minNode = new Node(element);
            maxNode.Another = minNode;
            minNode.Another = maxNode;
            if (maxRoot == null)
            {
                maxRoot = maxNode;
                minRoot = minNode;
            }
            else if (Count < int.MaxValue)
            {
                Insert(maxRoot, maxNode, Priority.Highest);
                Insert(minRoot, minNode, Priority.Lowest);
            }
            else
            {
                throw new OverflowException("Doing Enqueue() will overflow the Count property.");
            }
        }
        /// <summary>
        /// The element with the highest/lowest priority is removed.
        /// </summary>
        /// <returns>Returns the element with the highest/lowest priority.</returns>
        public T Dequeue(Priority select)
        {
            _ = maxRoot ?? throw new Exception("Double-Ended-PriorityQueue is Empty.");
            Node node = select == Priority.Highest ? maxRoot : minRoot;
            T returnValue = node.Value;
            Node another = node.Another;
            DeleteNode(node,select);
            DeleteNode(another, select == Priority.Highest ? Priority.Lowest : Priority.Highest);
            return returnValue;
        }
        /// <summary>
        /// Returns the number of elements in the Double-Ended-PrioirtyQueue.
        /// </summary>
        public int Count { get => maxRoot != null ? maxRoot.LeftCount + maxRoot.RightCount + 1 : 0; }
        /// <summary>
        /// Returns the element with the highest/lowest priority.
        /// </summary>
        public T Front(Priority select)
        {
            _ = maxRoot ?? throw new Exception("Double-Ended-PriorityQueue is Empty.");
            return select == Priority.Highest ? maxRoot.Value : minRoot.Value;
        }
    }

    /// <summary>
    /// In <c>Dequeue()</c> or <c>Front()</c>, decide whether to select highest priority elements or lowest priority elements.
    /// </summary>
    public enum Priority { Highest, Lowest }
}
