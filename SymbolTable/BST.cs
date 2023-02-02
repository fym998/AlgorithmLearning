class BST<TKey, TValue> : BinaryTree<TKey, TValue> where TKey : IComparable<TKey>
{
    protected static BTNode FindNearest(BTNode x, TKey key)
    {
        while (true)
        {
            switch (x.Key.CompareTo(key))
            {
                case 0:
                    return x;

                case < 0:
                    if (x.Right == null) return x;
                    else x = x.Right;
                    break;

                case > 0:
                    if (x.Left == null) return x;
                    else x = x.Left;
                    break;
            }
        }
    }

    public override TValue? GetValue(TKey key)
    {
        if (RootParent.Left == null)
            return default;

        var retBTNode = FindNearest(RootParent.Left, key);

        if (retBTNode.Key.Equals(key))
            return retBTNode.Value;
        else
            return default;
    }

    public override bool Put(TKey key, TValue value, PutBehavior behavior)
    {
        if (RootParent.Left == null)
        {
            RootParent.Left = new BTNode(key, value);
            Count++;
            return true;
        }

        BTNode found = FindNearest(RootParent.Left, key);

        if (found.Key.Equals(key))
        {
            if (behavior == PutBehavior.OverwriteExisting) found.Value = value;
            return false;
        }

        BTNode newNode = new(key,value);

        if (found.Key.CompareTo(key) < 0) found.Right = newNode;
        else found.Left = newNode;

        Count++;
        return true;
    }

    public override bool Remove(TKey key)
    {
        if (RootParent.Left == null) return false;

        var target = FindNearest(RootParent.Left, key);

        if (!target.Key.Equals(key)) return false;

        BTNode parent = target.Parent;
        BTNode? replacement;

        if (target.Right == null)
        {
            replacement = target.Left;
        }
        else
        {
            replacement = DeleteMin(target.Right);
            replacement.Left = target.Left;
            if (replacement != target.Right)
                replacement.Right = target.Right;
        }

        switch (target.R)
        {
            case R.Left: parent.Left = replacement; break;
            case R.Right: parent.Right = replacement; break;
        }

        Count--;

        return true;
    }

    static BTNode Min(BTNode x)
    {
        while (x.Left != null) x = x.Left;
        return x;
    }

    static BTNode DeleteMin(BTNode x)
    {
        BTNode target = Min(x);
        BTNode? right = target.Right;
        if (target != x) target.Parent.Left = right;
        return target;
    }

    public override bool ContainsKey(TKey key)
    {
        return RootParent.Left != null && FindNearest(RootParent.Left, key).Key.Equals(key);
    }
}