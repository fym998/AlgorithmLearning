using System.Collections;
using System.Text;

abstract class Node
{
    public abstract Node GetParent();
    public abstract IEnumerable<Node> Children { get; }
}

abstract partial class Tree : IEnumerable<Node>
{
    /// <summary>
    /// 这是“根节点的父节点”，不保存数据，只起辅助作用
    /// </summary>
    protected Node RootParent { get; init; }

    public virtual int TryGetCount() => 0;
    public virtual int TryGetMaxHeight() => 0;

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
        return new PreOrderEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Node>)this).GetEnumerator();
}