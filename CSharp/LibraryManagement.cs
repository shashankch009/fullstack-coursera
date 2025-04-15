using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Library library = new Library(); // Create an instance of the Library class

        while (true)
        {
            Console.WriteLine("Would you like to add, remove, list, search, checkout, or checkin books? (add/remove/list/search/checkout/checkin/exit)");
            string userCommand = (Console.ReadLine() ?? string.Empty).ToLower(); // Handle potential null value

            if (userCommand == "add")
            {
                library.AddBook();
            }
            else if (userCommand == "remove")
            {
                library.RemoveBook();
            }
            else if (userCommand == "list")
            {
                library.DisplayBooks();
            }
            else if (userCommand == "search")
            {
                library.SearchBook();
            }
            else if (userCommand == "checkout")
            {
                library.CheckoutBook();
            }
            else if (userCommand == "checkin")
            {
                library.CheckinBook();
            }
            else if (userCommand == "exit")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid action. Please type 'add', 'remove', 'list', 'search', 'checkout', 'checkin', or 'exit'.");
            }
        }
    }
}

class Library
{
    private Dictionary<string, bool> books = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
    private const int MaxCheckouts = 3; // Maximum number of books a user can check out
    private int currentCheckouts = 0; // Track the number of books currently checked out by the user

    public void AddBook()
    {
        Console.WriteLine("Enter the title of the book to add:");
        string newBook = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(newBook))
        {
            Console.WriteLine("Book title cannot be empty.");
        }
        else if (books.ContainsKey(newBook))
        {
            Console.WriteLine("This book is already in the library.");
        }
        else
        {
            books[newBook] = false; // Book is added as not checked out
            Console.WriteLine($"'{newBook}' has been added to the library.");
        }
    }

    public void RemoveBook()
    {
        Console.WriteLine("Enter the title of the book to remove:");
        string removeBook = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(removeBook))
        {
            Console.WriteLine("Book title cannot be empty.");
        }
        else if (books.Remove(removeBook))
        {
            Console.WriteLine($"'{removeBook}' has been removed from the library.");
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }

    public void DisplayBooks()
    {
        Console.WriteLine("Available books:");
        if (books.Count == 0)
        {
            Console.WriteLine("No books in the library.");
        }
        else
        {
            int index = 1;
            foreach (var book in books)
            {
                string status = book.Value ? "Checked Out" : "Available";
                Console.WriteLine($"{index}. {book.Key} - {status}");
                index++;
            }
        }
    }

    public void SearchBook()
    {
        Console.WriteLine("Enter the title of the book to search:");
        string searchTitle = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(searchTitle))
        {
            Console.WriteLine("Book title cannot be empty.");
        }
        else if (books.TryGetValue(searchTitle, out bool isCheckedOut))
        {
            string status = isCheckedOut ? "Checked Out" : "Available";
            Console.WriteLine($"The book '{searchTitle}' is in the library and is currently {status}.");
        }
        else
        {
            Console.WriteLine($"The book '{searchTitle}' is not available in the library.");
        }
    }

    public void CheckoutBook()
    {
        if (currentCheckouts >= MaxCheckouts)
        {
            Console.WriteLine($"You have reached the maximum limit of {MaxCheckouts} checkouts. Please check in a book before checking out another.");
            return;
        }

        Console.WriteLine("Enter the title of the book to checkout:");
        string checkoutTitle = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(checkoutTitle))
        {
            Console.WriteLine("Book title cannot be empty.");
        }
        else if (books.TryGetValue(checkoutTitle, out bool isCheckedOut))
        {
            if (isCheckedOut)
            {
                Console.WriteLine($"The book '{checkoutTitle}' is already checked out.");
            }
            else
            {
                books[checkoutTitle] = true; // Mark the book as checked out
                currentCheckouts++; // Increment the count
                Console.WriteLine($"You have successfully checked out '{checkoutTitle}'.");
            }
        }
        else
        {
            Console.WriteLine($"The book '{checkoutTitle}' is not available in the library.");
        }
    }

    public void CheckinBook()
    {
        Console.WriteLine("Enter the title of the book to check in:");
        string checkinTitle = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(checkinTitle))
        {
            Console.WriteLine("Book title cannot be empty.");
        }
        else if (books.TryGetValue(checkinTitle, out bool isCheckedOut))
        {
            if (!isCheckedOut)
            {
                Console.WriteLine($"The book '{checkinTitle}' is already available in the library.");
            }
            else
            {
                books[checkinTitle] = false; // Mark the book as available
                currentCheckouts--; // Decrement the count
                Console.WriteLine($"You have successfully checked in '{checkinTitle}'.");
            }
        }
        else
        {
            Console.WriteLine($"The book '{checkinTitle}' is not available in the library.");
        }
    }
}
