RBT<char, char> st = new();

foreach (var c in "ACEHLMPRSX")
{
    st[c] = c;
}

// for (int i = 0; i < 10; ++i)
// {
//     st[i] = i;
// }

Console.WriteLine(st.ToString());

//tree.Remove(1);
//Console.WriteLine(tree.ToString());

foreach (var node in st)
{
    Console.WriteLine(node.ToString());
}
//Console.WriteLine(string.Join('\n', (IDictionary<int,int>)tree));