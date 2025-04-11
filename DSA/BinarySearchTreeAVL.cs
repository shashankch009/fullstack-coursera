public class Node 
{
    public int Value {get;set;}

    public Node Left {get;set;}
    public Node Right {get;set;}
    public int Height { get; set; } // Add height property for AVL balancing

    public Node(int value)
    {
        Value = value;
        Left = Right = null;
        Height = 1; // Initialize height to 1
    }
}

public class BinaryTree
{
    public Node Root {get;}
    public BinaryTree()
    {
        Root = null;
    }

    public void Insert(int value)
    {
        Root = InsertRecursive(Root, value); // Update root after balancing
    }

    public void PrintInOrder()
    {
        PrintInOrder(Root);
    }

    private int GetHeight(Node node)
    {
        return node == null ? 0 : node.Height;
    }

    private int GetBalanceFactor(Node node)
    {
        return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
    }

    private Node RotateRight(Node y)
    {
        Node x = y.Left;
        Node T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
        x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

        return x;
    }

    private Node RotateLeft(Node x)
    {
        Node y = x.Right;
        Node T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
        y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

        return y;
    }

    private Node Balance(Node node)
    {
        int balanceFactor = GetBalanceFactor(node);

        // Left heavy
        if (balanceFactor > 1)
        {
            if (GetBalanceFactor(node.Left) < 0)
            {
                node.Left = RotateLeft(node.Left);
            }
            return RotateRight(node);
        }

        // Right heavy
        if (balanceFactor < -1)
        {
            if (GetBalanceFactor(node.Right) > 0)
            {
                node.Right = RotateRight(node.Right);
            }
            return RotateLeft(node);
        }

        return node;
    }

    private Node InsertRecursive(Node current, int value) 
    {
        if (current == null)
        {
            return new Node(value);
        }

        if (value < current.Value)
        {
            current.Left = InsertRecursive(current.Left, value);
        }
        else
        {
            current.Right = InsertRecursive(current.Right, value);
        }

        current.Height = Math.Max(GetHeight(current.Left), GetHeight(current.Right)) + 1;

        return Balance(current); // Balance the tree after insertion
    }

    private void PrintInOrder(Node node)
    {
        if (node == null) return;
        PrintInOrder(node.Left);
        Console.Write(node.Value + " ");
        PrintInOrder(node.Right);
    }

}
