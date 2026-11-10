
using System;
using System.Collections.Generic;
using System.Linq;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Department { get; set; } = "";
    public string JobTitle { get; set; } = "";
    public decimal MonthlySalary { get; set; }
    public double TaxRate { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }
    public string Email { get; set; } = "";
}

class Program
{
    static List<Employee> employees = new List<Employee>();
    static int nextId = 1001;

    static void Main(string[] args)
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== EMPLOYEE MANAGEMENT SYSTEM ===");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View Employees");
            Console.WriteLine("3. Search Employees");
            Console.WriteLine("4. Update Employee");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Exit");
            Console.Write("\nSelect an option: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1": AddNewEmployee(); break;
                case "2": DisplayAllEmployees(); break;
                case "3": SearchAndFilterEmployees(); break;
                case "4": UpdateEmployeeInformation(); break;
                case "5": DeleteEmployeeRecord(); break;
                case "6": running = false; break;
                default: Console.WriteLine("Invalid choice."); break;
            }

            if (running)
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }

    // CREATE
    static void AddNewEmployee()
    {
        Console.WriteLine("--- Add New Employee ---");

        Employee employee = new Employee();

        employee.Id = nextId++;
        employee.FirstName = GetText("First Name");
        employee.LastName = GetText("Last Name");
        employee.Department = GetText("Department");
        employee.JobTitle = GetText("Job Title");
        employee.MonthlySalary = GetDecimal("Monthly Salary");
        employee.TaxRate = GetTaxRate();
        employee.HireDate = GetDate();
        employee.IsActive = GetStatus();
        employee.Email = GetText("Email");

        employees.Add(employee);

        Console.WriteLine("\nEmployee added successfully!");
    }

    // READ
    static void DisplayAllEmployees()
    {
        Console.WriteLine("--- Employee Records ---");

        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return;
        }

        DisplayEmployeeTable(employees.OrderBy(e => e.Id));
    }

    // SEARCH / FILTER
    static void SearchAndFilterEmployees()
    {
        Console.WriteLine("--- Search Employees ---");
        Console.WriteLine("1. Search by Name");
        Console.WriteLine("2. Filter by Department");
        Console.WriteLine("3. Filter by Status");
        Console.Write("Select an option: ");

        string? choice = Console.ReadLine();
        List<Employee> results;

        switch (choice)
        {
            case "1":
                string name = GetText("Enter Name");

                results = employees.Where(e =>
                    e.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                    e.LastName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                    (e.FirstName + " " + e.LastName)
                        .Contains(name, StringComparison.OrdinalIgnoreCase)
                ).ToList();
                break;

            case "2":
                string department = GetText("Enter Department");
                results = employees.Where(e =>
                    e.Department.Equals(department, StringComparison.OrdinalIgnoreCase)
                ).ToList();
                break;

            case "3":
                bool status = GetStatus();
                results = employees.Where(e => e.IsActive == status).ToList();
                break;

            default:
                Console.WriteLine("Invalid choice.");
                return;
        }

        DisplayEmployeeTable(results);
    }

    // UPDATE
    static void UpdateEmployeeInformation()
    {
        DisplayAllEmployees();

        int id = GetInteger("\nEnter Employee ID to update");

        Employee? employee = employees.FirstOrDefault(e => e.Id == id);

        if (employee == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        Console.WriteLine("\nEnter new employee information:");

        employee.FirstName = GetText("First Name");
        employee.LastName = GetText("Last Name");
        employee.Department = GetText("Department");
        employee.JobTitle = GetText("Job Title");
        employee.MonthlySalary = GetDecimal("Monthly Salary");
        employee.TaxRate = GetTaxRate();
        employee.HireDate = GetDate();
        employee.IsActive = GetStatus();
        employee.Email = GetText("Email");

        Console.WriteLine("\nEmployee updated successfully!");
    }

    // DELETE
    static void DeleteEmployeeRecord()
    {
        DisplayAllEmployees();

        int id = GetInteger("\nEnter Employee ID to delete");

        Employee? employee = employees.FirstOrDefault(e => e.Id == id);

        if (employee == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        employees.Remove(employee);
        Console.WriteLine("\nEmployee deleted successfully!");
    }

    // DISPLAY TABLE
    static void DisplayEmployeeTable(IEnumerable<Employee> records)
    {
        List<Employee> list = records.ToList();

        if (list.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return;
        }

        Console.WriteLine(new string('-', 155));

        Console.WriteLine(
            $"{"ID",-6} {"FIRST NAME",-15} {"LAST NAME",-15} " +
            $"{"DEPARTMENT",-15} {"JOB TITLE",-20} {"SALARY",12} " +
            $"{"TAX",8} {"HIRE DATE",-12} {"ACTIVE",-8} {"EMAIL",-25}");

        Console.WriteLine(new string('-', 155));

        foreach (Employee e in list)
        {
            Console.WriteLine(
                $"{e.Id,-6} {e.FirstName,-15} {e.LastName,-15} " +
                $"{e.Department,-15} {e.JobTitle,-20} " +
                $"{e.MonthlySalary,12:N2} {e.TaxRate,8:P0} " +
                $"{e.HireDate,-12:yyyy-MM-dd} {e.IsActive,-8} {e.Email,-25}");
        }

        Console.WriteLine(new string('-', 155));
        Console.WriteLine($"Total Employees: {list.Count}");
    }

    // UTILITY FUNCTIONS
    static string GetText(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (input != "")
                return input;

            Console.WriteLine("Input cannot be empty.");
        }
    }

    static int GetInteger(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");

            if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
                return value;

            Console.WriteLine("Invalid number.");
        }
    }

    static decimal GetDecimal(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");

            if (decimal.TryParse(Console.ReadLine(), out decimal value) && value >= 0)
                return value;

            Console.WriteLine("Invalid amount.");
        }
    }

    static double GetTaxRate()
    {
        while (true)
        {
            Console.Write("Tax Rate (0.12 = 12%): ");

            if (double.TryParse(Console.ReadLine(), out double rate) &&
                !double.IsNaN(rate) && rate >= 0 && rate <= 1)
                return rate;

            Console.WriteLine("Invalid tax rate.");
        }
    }

    static DateTime GetDate()
    {
        while (true)
        {
            Console.Write("Hire Date (YYYY-MM-DD): ");

            if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                return date;

            Console.WriteLine("Invalid date.");
        }
    }

    static bool GetStatus()
    {
        while (true)
        {
            Console.Write("Active (Y/N): ");
            string? input = Console.ReadLine()?.ToUpper();

            if (input == "Y") return true;
            if (input == "N") return false;

            Console.WriteLine("Enter Y or N.");
        }
    }
}
