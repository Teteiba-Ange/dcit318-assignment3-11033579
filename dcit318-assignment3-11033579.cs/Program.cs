using System;
using System.Collections.Generic;
using System.Linq;

// Finance management system
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

// Finance app
public class FinanceApp
{
    private readonly List<Transaction> _transactions = new();

    public void Run()
    {
        Console.WriteLine("Welcome to the Finance App");

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

// Healthcare Management System
class Repository<T>
{
    private readonly List<T> _items = new List<T>();

    public void Add(T item) => _items.Add(item);
    public List<T> GetAll() => new List<T>(_items);
    public T? GetById(Func<T, bool> predicate) => _items.FirstOrDefault(predicate);

    public bool Remove(Func<T, bool> predicate)
    {
        var item = GetById(predicate);
        if (item != null)
            return _items.Remove(item);
        return false;
    }
}

public class Patient
{
    public int Id { get; }
    public string Name { get; }
    public int Age { get; }
    public string Gender { get; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id;
        Name = name;
        Age = age;
        Gender = gender;
    }

    public override string ToString() => $"Id: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}";
}

public class Prescription
{
    public int Id { get; }
    public int PatientId { get; }
    public string MedicationName { get; }
    public DateTime DateIssued { get; }

    public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName;
        DateIssued = dateIssued;
    }

    public override string ToString() =>
        $"Id: {Id}, PatientId: {PatientId}, MedicationName: {MedicationName}, DateIssued: {DateIssued}";
}

public class HealthSystemApp
{
    private readonly Repository<Patient> _patientRepo = new Repository<Patient>();
    private readonly Repository<Prescription> _patientPrescription = new Repository<Prescription>();
    private readonly Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

    public void SeedData()
    {
        _patientRepo.Add(new Patient(1, "Angela", 20, "Female"));
        _patientRepo.Add(new Patient(2, "Ulysis", 22, "Male"));
        _patientRepo.Add(new Patient(3, "Andrew", 24, "Male"));

        _patientPrescription.Add(new Prescription(103, 1, "Decatylene", new DateTime(2024, 1, 2)));
        _patientPrescription.Add(new Prescription(107, 2, "Amoxiclav", new DateTime(2024, 7, 9)));
        _patientPrescription.Add(new Prescription(109, 3, "Tothema", new DateTime(2024, 11, 2)));
        _patientPrescription.Add(new Prescription(102, 4, "Wormplex400", new DateTime(2024, 12, 2)));
        _patientPrescription.Add(new Prescription(108, 5, "RooterMixture", new DateTime(2024, 1, 10)));
    }

    public void BuildPrescriptionMap()
    {
        _prescriptionMap.Clear();
        var prescriptions = _patientPrescription.GetAll();

        foreach (var prescription in prescriptions)
        {
            if (!_prescriptionMap.ContainsKey(prescription.PatientId))
            {
                _prescriptionMap[prescription.PatientId] = new List<Prescription>();
            }
            _prescriptionMap[prescription.PatientId].Add(prescription);
        }
    }

    public List<Prescription> GetPrescriptionsByPatientId(int patientId)
    {
        return _prescriptionMap.TryGetValue(patientId, out var prescriptions)
            ? prescriptions
            : new List<Prescription>();
    }

    public void PrintAllPatients()
    {
        Console.WriteLine("\n--- PATIENT LIST ---");
        foreach (var patient in _patientRepo.GetAll())
        {
            Console.WriteLine(patient);
        }
    }

    public void PrintPrescriptionsForPatient(int patientId)
    {
        var patient = _patientRepo.GetById(p => p.Id == patientId);
        if (patient == null)
        {
            Console.WriteLine($"\nPatient with ID {patientId} not found");
            return;
        }

        Console.WriteLine($"\nPRESCRIPTIONS FOR {patient.Name} (ID: {patient.Id}):");
        var prescriptions = GetPrescriptionsByPatientId(patientId);

        if (prescriptions.Count == 0)
        {
            Console.WriteLine("No prescriptions found");
            return;
        }

        foreach (var prescription in prescriptions)
        {
            Console.WriteLine($"- {prescription}");
        }
    }
}

// Application Entry Point
class Program
{
    static void Main()
    {
        // Run Finance App
        FinanceApp financeApp = new FinanceApp();
        financeApp.Run();

        // Run Health System App
        HealthSystemApp healthApp = new HealthSystemApp();
        healthApp.SeedData();
        healthApp.BuildPrescriptionMap();
        healthApp.PrintAllPatients();
        healthApp.PrintPrescriptionsForPatient(1);
    }
}

