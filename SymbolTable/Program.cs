RBT<int, int> tree = new();
for (int i = 0; i < 10; ++i)
{
    tree.Put(i, i, PutBehavior.DoNothingOnExisting);
}

Console.WriteLine(tree.ToString());

//tree.Remove(1);
//Console.WriteLine(tree.ToString());

foreach (var node in (IEnumerable<Node>)tree)
{
    Console.WriteLine(node);
}
//Console.WriteLine(string.Join('\n', (IDictionary<int,int>)tree));