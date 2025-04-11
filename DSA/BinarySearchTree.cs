
public class Node 
{
    public int Value {get;set;}

    public Node Left {get;set;}
    public Node Right {get;set;}
    public Node(int value)
    {
        Value = value;
        Left = Right = null;
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
        if (Root == null)
        {
            Root = new Node(value);
        }
        else
        {
            InsertRecursive(Root, value);
        }
    }

    public void PrintInOrder()
    {
        PrintInOrder(Root);
    }

    private void InsertRecursive(Node current, int value) 
    {
        if (value < current.Value)
        {
            if (current.Left == null)
            {
                current.Left = new Node(value);
            }
            else
            {
                InsertRecursive(current.Left, value);
            }
        }
        else
        {
            if (current.Right == null)
            {
                current.Right = new Node(value);
            }
            else
            {
                InsertRecursive(current.Right, value);
            }
        }
    }

    private void PrintInOrder(Node node)
    {
        if (node == null) return;
        PrintInOrder(node.Left);
        Console.Write(node.Value + " ");
        PrintInOrder(node.Right);
    }

}
