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
    public Node RootParent { get; init; }

    protected Tree(Node rootParent) => RootParent = rootParent;

    public virtual bool TryGetCount(out int count)
    {
        count = 0;
        return false;
    }
    public virtual bool TryGetMaxHeight(out int maxHeight)
    {
        maxHeight = 0;
        return false;
    }

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

    public IEnumerator<Node> GetEnumerator() => new PreOrderEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Node>)this).GetEnumerator();
}