using System.Collections;
using System.Diagnostics.CodeAnalysis;

abstract class BinaryTree<TKey, TValue> : Tree, ISymbolTable<TKey, TValue>
{
    protected enum R { Left, Right }
    protected class BTNode : Node
    {
        public BTNode(TKey key, TValue value, BTNode? left = null, BTNode? right = null)
        {
            (Key, Value, Left, Right) = (key, value, left, right);
        }

        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public R R { get; private set; }
        public BTNode Parent { get; private set; }

        public override Node GetParent() => Parent;

        BTNode? left, right;

        public BTNode? Left
        {
            get => left;
            set
            {
                left = value;
                if (value != null)
                {
                    value.R = R.Left;
                    value.Parent = this;
                }
            }
        }
        public BTNode? Right
        {
            get => right;
            set
            {
                right = value;
                if (value != null)
                {
                    value.R = R.Right;
                    value.Parent = this;
                }
            }
        }

        private static readonly BTNode[] emptyArray = new BTNode[0];
        public override BTNode[] Children => (Left, Right) switch
        {
            (not null, not null) => new BTNode[] { Left, Right },
            (not null, null    ) => new BTNode[] { Left },
            (null,     not null) => new BTNode[] { Right },
            (null,     null    ) => emptyArray
        };

        public override string ToString() => $"{R}[{Key},{Value}]";
    }

    protected BinaryTree() : base(new BTNode(default, default)) { }

    protected BinaryTree(BTNode rootParent) : base(rootParent) { }

    protected new BTNode RootParent
    {
        get => (BTNode)base.RootParent;
        init => base.RootParent = value;
    }

    public int Count { get; protected set; }
    public override bool TryGetCount(out int count)
    {
        count = Count;
        return true;
    }

    public bool IsReadOnly => false;

    ICollection<TKey> IDictionary<TKey, TValue>.Keys => throw new NotImplementedException();

    ICollection<TValue> IDictionary<TKey, TValue>.Values => throw new NotImplementedException();

    public TValue this[TKey key]
    {
        get
        {
            TValue? ret = GetValue(key);
            if (ret == null) throw new KeyNotFoundException();
            return ret;
        }
        set => Put(key, value, PutBehavior.OverwriteExisting);
    }

    public abstract TValue? GetValue(TKey key);

    public abstract bool Put(TKey key, TValue value, PutBehavior behavior);

    public abstract bool Remove(TKey key);

    public abstract bool ContainsKey(TKey key);

    public void Clear()
    {
        RootParent.Left = null;
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
        return ((Tree)this).Select(node =>
        {
            var btnode = (BTNode)node;
            return new KeyValuePair<TKey, TValue>(btnode.Key, btnode.Value);
        }).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
    // {
    //     public KeyValuePair<TKey, TValue> Current => curNode != null ? new(curNode.Key, curNode.Value) : default;
    //     object IEnumerator.Current => Current;

    //     readonly BTNode rootParent;
    //     BTNode? curNode;
    //     readonly Queue<BTNode> next = new();

    //     public Enumerator(BinaryTree<TKey, TValue> tree)
    //     {
    //         rootParent = tree.RootParent;
    //         Reset();
    //     }

    //     public void Reset()
    //     {
    //         curNode = rootParent;
    //         next.Clear();
    //         if (rootParent.Left != null) next.Enqueue(rootParent.Left);
    //     }

    //     public bool MoveNext()
    //     {
    //         if (next.Count == 0) return false;

    //         curNode = next.Dequeue();
    //         foreach (var c in curNode.Children)
    //         {
    //             next.Enqueue(c);
    //         }
    //         return true;
    //     }

    //     public void Dispose() => GC.SuppressFinalize(this);
    // }

    // =======================================================

    // bool IDictionary<TKey, TValue>.TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    // {
    //     value = GetValue(key);
    //     return value != null;
    // }

    // public void Add(TKey key, TValue value)
    // {
    //     if (!Put(key, value, PutBehavior.DoNothingOnExisting)) throw new ArgumentException("The key already exists.");
    // }
    // public void Add(KeyValuePair<TKey, TValue> item)
    // {
    //     try { Add(item.Key, item.Value); }
    //     catch { throw; }
    // }

    // bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    // {
    //     if (Contains(item))
    //     {
    //         Remove(item.Key);
    //         return true;
    //     }
    //     else return false;
    // }

    // public bool Contains(KeyValuePair<TKey, TValue> item)
    // {
    //     return ContainsKey(item.Key) && GetValue(item.Key).Equals(item.Value);
    // }

    // public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    // {
    //     foreach (var item in this)
    //     {
    //         array[arrayIndex++] = item;
    //     }
    // }
}