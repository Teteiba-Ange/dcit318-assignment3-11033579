using System;
using System.Collections.Generic;
using System.IO;
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

// Marker Interface for Inventory Items
public interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int Quantity { get; set; }
}

// Electronic Item Class
public class ElectronicItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public string Brand { get; }
    public int WarrantyMonths { get; }

    public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Brand = brand;
        WarrantyMonths = warrantyMonths;
    }
}

// Grocery Item Class
public class GroceryItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; }

    public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        ExpiryDate = expiryDate;
    }
}

// Custom Exceptions
public class DuplicateItemException : Exception
{
    public DuplicateItemException(string message) : base(message) { }
}

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message) : base(message) { }
}

public class InvalidQuantityException : Exception
{
    public InvalidQuantityException(string message) : base(message) { }
}

// Generic Inventory Repository
public class InventoryRepository<T> where T : IInventoryItem
{
    private readonly Dictionary<int, T> _items = new();

    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Id))
            throw new DuplicateItemException($"Item with ID {item.Id} already exists.");

        _items[item.Id] = item;
    }

    public T GetItemById(int id)
    {
        if (!_items.TryGetValue(id, out var item))
            throw new ItemNotFoundException($"Item with ID {id} not found.");

        return item;
    }

    public void RemoveItem(int id)
    {
        if (!_items.Remove(id))
            throw new ItemNotFoundException($"Item with ID {id} not found.");
    }

    public List<T> GetAllItems() => new(_items.Values);

    public void UpdateQuantity(int id, int newQuantity)
    {
        if (newQuantity < 0)
            throw new InvalidQuantityException("Quantity cannot be negative.");

        var item = GetItemById(id);
        item.Quantity = newQuantity;
    }
}

// Warehouse Manager Class
public class WarehouseManager
{
    private readonly InventoryRepository<ElectronicItem> _electronics = new();
    private readonly InventoryRepository<GroceryItem> _groceries = new();

    public void SeedData()
    {
        _electronics.AddItem(new ElectronicItem(1, "Ipad", 10, "BrandA", 24));
        _electronics.AddItem(new ElectronicItem(2, "Desktop", 15, "BrandB", 12));

        _groceries.AddItem(new GroceryItem(1, "Milo", 20, DateTime.Now.AddDays(7)));
        _groceries.AddItem(new GroceryItem(2, "T-roll", 30, DateTime.Now.AddDays(3)));
    }

    public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
    {
        var items = repo.GetAllItems();
        foreach (var item in items)
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}");
        }
    }

    public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
    {
        try
        {
            var item = repo.GetItemById(id);
            item.Quantity += quantity;
            Console.WriteLine($"Increased stock for {item.Name} by {quantity}. New quantity: {item.Quantity}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
    {
        try
        {
            repo.RemoveItem(id);
            Console.WriteLine($"Removed item with ID: {id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

// Custom Exception Classes for Student System
public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message) : base(message) { }
}

public class MissingFieldException : Exception
{
    public MissingFieldException(string message) : base(message) { }
}

// Student Class
public class Student
{
    public int Id { get; }
    public string FullName { get; }
    public int Score { get; }

    public Student(int id, string fullName, int score)
    {
        Id = id;
        FullName = fullName;
        Score = score;
    }

    public string GetGrade()
    {
        if (Score >= 80) return "A";
        if (Score >= 70) return "B";
        if (Score >= 60) return "C";
        if (Score >= 50) return "D";
        return "F";
    }
}

// StudentResultProcessor Class
public class StudentResultProcessor
{
    public List<Student> ReadStudentsFromFile(string inputFilePath)
    {
        var students = new List<Student>();

        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var fields = line.Split(',');

                if (fields.Length < 3)
                {
                    throw new MissingFieldException("One or more fields are missing in the input line.");
                }

                if (!int.TryParse(fields[2].Trim(), out int score))
                {
                    throw new InvalidScoreFormatException($"Invalid score format for student: {fields[1].Trim()}");
                }

                int id = int.Parse(fields[0].Trim());
                string fullName = fields[1].Trim();

                students.Add(new Student(id, fullName, score));
            }
        }

        return students;
    }

    public void WriteReportToFile(List<Student> students, string outputFilePath)
    {
        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            foreach (var student in students)
            {
                writer.WriteLine($"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}");
            }
        }
    }
}

// Main Application
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

        // Run Warehouse Manager
        WarehouseManager warehouseManager = new WarehouseManager();
        warehouseManager.SeedData();
        Console.WriteLine("--- Grocery Items ---");
        warehouseManager.PrintAllItems(warehouseManager._groceries);
        Console.WriteLine("--- Electronic Items ---");
        warehouseManager.PrintAllItems(warehouseManager._electronics);

        // Run Student Result Processor
        string inputFilePath = "students.txt"; // Input file path
        string outputFilePath = "report.txt";   // Output file path
        var studentProcessor = new StudentResultProcessor();

        try
        {
            // Read students from file
            List<Student> students = studentProcessor.ReadStudentsFromFile(inputFilePath);
            // Write report to file
            studentProcessor.WriteReportToFile(students, outputFilePath);
            Console.WriteLine("Report generated successfully.");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error: Input file not found. {ex.Message}");
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
//Q5

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Marker Interface for Inventory Entities
public interface IInventoryEntity
{
    int Id { get; }
}

// Immutable Inventory Record
public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

// Generic Inventory Logger
public class InventoryLogger<T> where T : IInventoryEntity
{
    private readonly List<T> _log = new();
    private readonly string _filePath;

    public InventoryLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Add(T item)
    {
        _log.Add(item);
    }

    public List<T> GetAll()
    {
        return new List<T>(_log);
    }

    public void SaveToFile()
    {
        try
        {
            var json = JsonSerializer.Serialize(_log);
            using (var writer = new StreamWriter(_filePath))
            {
                writer.Write(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data to file: {ex.Message}");
        }
    }

    public void LoadFromFile()
    {
        try
        {
            using (var reader = new StreamReader(_filePath))
            {
                var json = reader.ReadToEnd();
                var items = JsonSerializer.Deserialize<List<T>>(json);
                if (items != null)
                {
                    _log.Clear();
                    _log.AddRange(items);
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found. No data loaded.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data from file: {ex.Message}");
        }
    }
}

// Integration Layer - InventoryApp
public class InventoryApp
{
    private readonly InventoryLogger<InventoryItem> _logger;

    public InventoryApp(string filePath)
    {
        _logger = new InventoryLogger<InventoryItem>(filePath);
    }

    public void SeedSampleData()
    {
        _logger.Add(new InventoryItem(1, "Laptop", 10, DateTime.Now));
        _logger.Add(new InventoryItem(2, "Mouse", 50, DateTime.Now));
        _logger.Add(new InventoryItem(3, "Keyboard", 30, DateTime.Now));
        _logger.Add(new InventoryItem(4, "Monitor", 15, DateTime.Now));
        _logger.Add(new InventoryItem(5, "Headphones", 25, DateTime.Now));
    }

    public void SaveData()
    {
        _logger.SaveToFile();
    }

    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    public void PrintAllItems()
    {
        var items = _logger.GetAll();
        foreach (var item in items)
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Date Added: {item.DateAdded}");
        }
    }
}


class Program
{
    static void Main()
    {
        string filePath = "inventory.json"; 
        var inventoryApp = new InventoryApp(filePath);

        // Seed sample data
        inventoryApp.SeedSampleData();

        // Save data to file
        inventoryApp.SaveData();

        // Simulate clearing memory and starting a new session
        inventoryApp = new InventoryApp(filePath);

        
        inventoryApp.LoadData();

        
        Console.WriteLine("Inventory Items:");
        inventoryApp.PrintAllItems();
    }
}