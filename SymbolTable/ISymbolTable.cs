using System.Diagnostics.CodeAnalysis;

enum PutBehavior
{
    OverwriteExisting, DoNothingOnExisting
}

interface ISymbolTable<TKey, TValue> : IDictionary<TKey, TValue>
{
    // below are defined in IDictionary:
    // TValue this[TKey key] { get; set; }
    // bool ContainsKey(TKey key);
    // bool Remove(TKey key);

    /// <summary>
    ///     Puts an element with the provided <paramref name="key" /> and <paramref name="value" />
    ///     to the <see cref="ISymbolTable" />
    /// </summary>
    /// <param name="behavior">What to do when the key already exists.</param>
    /// <returns>
    ///     true when a new element is added to the <see cref="ISymbolTable" />;
    ///     false when the <see cref="ISymbolTable" /> already contains the specified <paramref name="key" />.
    /// </returns>
    bool Put(TKey key, TValue value, PutBehavior behavior);

    TValue? GetValue(TKey key);

    bool IDictionary<TKey, TValue>.TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        value = GetValue(key);
        return value != null;
    }

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
    {
        if (!Put(key, value, PutBehavior.DoNothingOnExisting))
            throw new ArgumentException("The key already exists.");
    }
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
        try { Add(item.Key, item.Value); }
        catch { throw; }
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        if (Contains(item))
        {
            Remove(item.Key);
            return true;
        }
        else return false;
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
        return ContainsKey(item.Key) && GetValue(item.Key).Equals(item.Value);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        foreach (var item in this)
        {
            array[arrayIndex++] = item;
        }
    }
}