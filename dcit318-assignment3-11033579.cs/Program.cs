using System;
using System.Collections.Generic;

//Finance management system  Question 1

public record Transaction( 
    Guid Id,
    decimal Amount,
    DateTime Date,
    string Category
);



// Account System

public class Account
{
    public string AccountNumber { get; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
        Console.WriteLine($"Transaction of {transaction.Amount:C} applied. New balance: {Balance:C}");
    }
}

public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance)
    {
    }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine($"Insufficient funds for transaction: {transaction.Category} ({transaction.Amount:C})");
        }
        else
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction for '{transaction.Category}' of {transaction.Amount:C} processed.");
            Console.WriteLine($"Updated Balance: {Balance:C}");
        }
    }
}



// Processor Interface and Classes

public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[MobileMoney] Processing: {transaction.Category} - {transaction.Amount:C}");
    }
}

public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[BankTransfer] Processing: {transaction.Category} - {transaction.Amount:C}");
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[CryptoWallet] Processing: {transaction.Category} - {transaction.Amount:C}");
    }
}

//Finance app

public class FinanceApp
{
    private readonly List<Transaction> _transactions = new();

    public void Run()
    {
        Console.WriteLine("You are welcome to the Finance App");

        SavingsAccount account = new SavingsAccount("AC123456", 1000m);

        Transaction t1 = new Transaction(Guid.NewGuid(), 120m, DateTime.Now, "Groceries");
        Transaction t2 = new Transaction(Guid.NewGuid(), 300m, DateTime.Now, "Utilities");
        Transaction t3 = new Transaction(Guid.NewGuid(), 700m, DateTime.Now, "Entertainment");

        ITransactionProcessor mobileProcessor = new MobileMoneyProcessor();
        ITransactionProcessor bankProcessor = new BankTransferProcessor();
        ITransactionProcessor cryptoProcessor = new CryptoWalletProcessor();

        mobileProcessor.Process(t1);
        account.ApplyTransaction(t1);
        _transactions.Add(t1);

        bankProcessor.Process(t2);
        account.ApplyTransaction(t2);
        _transactions.Add(t2);

        cryptoProcessor.Process(t3);
        account.ApplyTransaction(t3);
        _transactions.Add(t3);

        Console.WriteLine();
        Console.WriteLine("Transaction Summary:");
        foreach (var tx in _transactions)
        {
            Console.WriteLine($"- {tx.Category}: {tx.Amount:C} on {tx.Date}");
        }

        Console.WriteLine($"\nFinal Balance: {account.Balance:C}");
    }
}





public class Program
{
    public static void Main()
    {
        FinanceApp app = new FinanceApp();
        app.Run();
    }
}
// Question 2 Healthcare Management system
//Entity Management
class Repository<T> {
    private readonly List<T> _items = new List<T>();
    public void Add(T item) => _items.Add(item)
    public List<T> GetAll => new List<T>(_items)
    T? GetById(Func<T, bool> predicate) => _items.FirstOrDefault(predicate);

    bool Remove(Func<T, bool> predicate) {
        var item = GetById(predicate);
        if (item != null)
            return _items.Remove(item);
        return false;
    }
}

public class Patient
{
    private int Id { get; }
    private string Name   { get; };
    private int Age { get; };
    private string Gender { get; };


    public Patient(int id,string name,int age, string gender)
    {
        this.Id = id;
        this.Name = name;
        this.Age = age;
        this.Gender = gender;
    }
    public override string toString()=>$"Id :{Id}, Name :{Name}  Age :{Age} Gender :{Gender}"
}

public class Prescription
{
    private int Id { get; }
    private int PatientId { get; };
    private string MedicationName { get; };
    private DateTime DateIssued { get; };


    public Prescription(int id, int patientid, string medicationName,DateTime dateIssued)
    {
        this.Id = id;
        this.PatietId = patientid;
        this.MedicationName = medicationName;
        this.DateIssued = dateIssued;
    }
    public override string toString()=>
        $"id :{Id} PateintId :{PatientId} MedicationName :{MedicationName} DateIssued :{DateIssued}"
}

public class HealthManagementSystem
{

}

