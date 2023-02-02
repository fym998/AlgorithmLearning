RBT<int, int> tree = new();
for (int i = 0; i < 1024; ++i)
    tree.Put(i, i, PutBehavior.DoNothingOnExisting);

Console.WriteLine(tree.ToString());

//tree.Remove(1);
//Console.WriteLine(tree.ToString());

//Console.WriteLine(string.Join('\n', (IDictionary<int,int>)tree));
Console.WriteLine(string.Join('\n', (IEnumerable<Node>)tree));