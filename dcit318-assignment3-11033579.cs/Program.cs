using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter Amount:");
        string inputAmount = Console.ReadLine();
        decimal amount = decimal.Parse(inputAmount);

        Console.WriteLine("Enter Category:");
        string category = Console.ReadLine();

        var transaction = new Transaction(
            Id: 1,
            Date: DateTime.Now,
            Amount: amount,
            Category: category
        );

        // Choose processor
        ITransactionProcessor processor;

        Console.WriteLine("Select processor: 1. Bank  2. Mobile  3. Crypto");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                processor = new BankTransferProcessor();
                break;
            case "2":
                processor = new MobileMoneyProcessor();
                break;
            case "3":
                processor = new CryptoWalletProcessor();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                return;
        }

        processor.Process(transaction);
    }
}


public record Transaction
(
    int Id,
    DateTime Date,
    decimal Amount,
    string Category
);

// Interface
public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

// Bank Transfer Processor
public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine("This is your bank transfer");
        Console.WriteLine($"Amount: {transaction.Amount}");
        Console.WriteLine($"Category: {transaction.Category}");
        
    }
}

// Mobile Money Processor
public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine("This is your Mobile Money transfer");
        Console.WriteLine($"Amount: {transaction.Amount}");
        Console.WriteLine($"Category: {transaction.Category}");
        Console.WriteLine($"Date: {transaction.Date}");
    }
}

// Crypto Wallet Processor
public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine("This is your Crypto wallet transfer");
        Console.WriteLine($"Amount: {transaction.Amount}");
        Console.WriteLine($"Category: {transaction.Category}");
        
    }
}

// Main program
