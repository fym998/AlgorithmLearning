class RBT<TKey, TValue> : BST<TKey, TValue> where TKey : IComparable<TKey>
{
    enum NodeColor { Black, Red }
    class RBTNode : BTNode
    {
        public RBTNode(TKey key,TValue value, NodeColor color,BTNode? left=null,BTNode? right=null):base(key, value, left, right)
        {
            Color = color;
        }

        public NodeColor Color { get; set; }

        public new RBTNode Parent => (RBTNode)base.Parent;
        public new RBTNode? Left { get => (RBTNode)base.Left; set => base.Left = value; }
        public new RBTNode? Right { get => (RBTNode)base.Right; set => base.Right = value; }

        public override string ToString()
        {
            return $"({R},{Color})[{Key},{Value}]";
        }
    }

    public RBT()
    {
        RootParent = new RBTNode(default, default, NodeColor.Black);
    }

    enum Rotation { Left, Right }

    static RBTNode Rotate(RBTNode x, Rotation o)
    {
        RBTNode? c;
        if (o == Rotation.Left)
        {
            c = x.Right;
            x.Right = c.Left;
        }
        else
        {
            c = x.Left;
            x.Left = c.Right;
        }

        switch (x.R)
        {
            case R.Left: x.Parent.Left = c; break;
            case R.Right: x.Parent.Right = c; break;
        }

        if (o == Rotation.Left)
            c.Left = x;
        else
            c.Right = x;

        c.Color = x.Color;
        x.Color = NodeColor.Red;

        return c;
    }

    static void FlipColors(RBTNode x)
    {
        x.Color = NodeColor.Red;
        x.Left.Color = NodeColor.Black;
        x.Right.Color = NodeColor.Black;
    }

    static bool IsRed(RBTNode? x) => x != null && x.Color == NodeColor.Red;

    public override bool Put(TKey key, TValue value, PutBehavior behavior)
    {
        if (!base.Put(key, value, behavior)) return false;

        //从新增节点开始，不断向上移动，调整树的结构
        BTNode addedNode = FindNearest(RootParent.Left, key);
        RBTNode cur = new(key, value, NodeColor.Red);

        if (addedNode.R == R.Left) addedNode.Parent.Left = cur;
        else addedNode.Parent.Right = cur;

        do
        {
            //  cur  -->  R  --> cur
            // B   R    cur     R
            //         B      B
            if (RBT<TKey, TValue>.IsRed(cur.Right) && !RBT<TKey, TValue>.IsRed(cur.Left)) cur = RBT<TKey, TValue>.Rotate(cur,Rotation.Left);

            //   cur -->  R    -->  cur(R)
            //  R       R   cur    R     R
            // R
            if (RBT<TKey, TValue>.IsRed(cur.Left) && RBT<TKey, TValue>.IsRed(cur.Left.Left)) cur = RBT<TKey, TValue>.Rotate(cur,Rotation.Right);

            //   cur -->  cur(R)
            //  R   R    B      B
            if (RBT<TKey, TValue>.IsRed(cur.Left) && RBT<TKey, TValue>.IsRed(cur.Right)) RBT<TKey, TValue>.FlipColors(cur);
            cur = cur.Parent;
        } while (cur != RootParent);

        ((RBTNode)RootParent.Left).Color = NodeColor.Black;
        return true;
    }

    public override bool Remove(TKey key)
    {
        throw new NotImplementedException();
    }
}