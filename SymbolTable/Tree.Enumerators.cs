using System.Collections;

abstract partial class Tree
{
    public struct PreOrderEnumerator : IEnumerator<Node>
    {
        public Node Current { get; private set; }
        object IEnumerator.Current => Current;

        private readonly Stack<IEnumerator<Node>> s;
        private readonly Node rootParent;

        public PreOrderEnumerator(Tree tree)
        {
            if (tree.TryGetMaxHeight(out int h))
                s = new Stack<IEnumerator<Node>>(h + 1);
            else
                s = new Stack<IEnumerator<Node>>(2);
            rootParent = tree.RootParent;
            Reset();
        }

        public void Reset()
        {
            s.Clear();
            s.Push(rootParent.Children.GetEnumerator());
        }

        public bool MoveNext()
        {
            var cur = s.Peek();
            if (cur.MoveNext())
            {
                Current = cur.Current;
                s.Push(Current.Children.GetEnumerator());
                return true;
            }
            while (!cur.MoveNext() && s.TryPop(out cur)) ;
            if (s.Count > 0)
            {
                Current = cur.Current;
                return true;
            }
            return false;
        }

        public void Dispose() { }
    }

    public struct BreadthFirstEnumerator : IEnumerator<Node>
    {
        public Node Current { get; private set; }
        object IEnumerator.Current => Current;

        private readonly Node RootParent;
        private readonly Queue<Node> next;

        public BreadthFirstEnumerator(Tree tree)
        {
            if (tree.TryGetCount(out int count))
                next = new((count + 1) / 2);
            else
                next = new(2);
            RootParent = tree.RootParent;
            Reset();
        }

        public void Reset()
        {
            next.Clear();
            foreach (var node in RootParent.Children)
            {
                next.Enqueue(node);
            }
        }

        public bool MoveNext()
        {
            if (next.Count == 0) return false;
            Current = next.Dequeue();
            foreach (var cld in Current.Children)
            {
                next.Enqueue(cld);
            }
            return true;
        }

        public void Dispose() { }
    }
}