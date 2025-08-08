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
        Amount = user
        Console.WriteLine("Please Enter Category");
    }
}


publc class MobileMoneyProcessor : ItransactionProcessor
{
    void Process()
    {
        Console.WriteLine("This is your Mobile Money transfer");
    }
}

publc class CryptoWalletProcessor : ItransactionProcessor
{
    void Process()
    {
        Console.WriteLine("This is your Crypto wallet transfer");
    }
}
