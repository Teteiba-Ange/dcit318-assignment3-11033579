using System.Transactions;

public class Record { 

    public class Transaction()
    {
        int Id;
        DateTime Date;
        Decimal Amount;
        String Category;
    }
}

public interface ItransactionProcessor
{
    void Process(Transaction transaction);

}

publc class BankTransferProcessor : ItransactionProcessor
{
    void Process()
    {
       
        Console.WriteLine("This is your bank transfer");

        Console.WriteLine("Please Enter Amount");
        int user = Console.ReadLine();
        Amount = user;
        Console.WriteLine($"This is the Amount you entered {Amount}");
        Console.WriteLine("Please Enter Category");
        string user2 =Console.ReadLine();
        Category = user2;
        Console.WriteLine($"This is the category you entered {Category}");
    }
}


publc class MobileMoneyProcessor : ItransactionProcessor
{
    void Process()
    {
        Console.WriteLine("This is your Mobile Money transfer");
        Console.WriteLine("Please Enter Amount");
        int user3 = Console.ReadLine();
        Amount = user3;
        Console.WriteLine($"This is the Amount you entered {Amount}");
        Console.WriteLine("Please Enter Category");
        string user5 = Console.ReadLine();
        Category = user5;
        Console.WriteLine($"This is the category you entered{Category}");
    }
}

publc class CryptoWalletProcessor : ItransactionProcessor
{
    void Process()
    {
        Console.WriteLine("This is your Crypto wallet transfer");
    }
}
