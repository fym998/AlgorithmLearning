using System.Collections;
using System.Text;

abstract class Node
{
    public abstract Node GetParent();
    public abstract IEnumerable<Node> Children { get; }
}

abstract class Tree : IEnumerable<Node>
{
    protected Node RootParent { get; init; }

    protected virtual void Print(StringBuilder dest, Node node, string sep, int depth = 0)
    {
        if (node == RootParent)
        {
            dest.Append("root:");
            depth--;
        }
        else
            dest.AppendJoin(null, Enumerable.Repeat(sep, depth))
                .AppendLine($"{node.ToString()}");
        depth++;
        foreach (var child in node.Children)
        {
            Print(dest, child, sep, depth);
        }
    }

    public override string ToString()
    {
        StringBuilder ret = new();
        Print(ret, RootParent, "    |");
        return ret.Remove(ret.Length - 1, 1).ToString();
    }

    IEnumerator<Node> IEnumerable<Node>.GetEnumerator()
    {
        return new TreeNodeEnumerator(RootParent);
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Node>)this).GetEnumerator();

    public struct TreeNodeEnumerator : IEnumerator<Node>
    {
        public Node Current { get; private set; }
        object IEnumerator.Current => Current;

        private readonly Node RootParent;
        private readonly Queue<Node> next = new();

        public TreeNodeEnumerator(Node RootParent)
        {
            Current = this.RootParent = RootParent;
            Reset();
        }

        public void Reset()
        {
            Current = RootParent;
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