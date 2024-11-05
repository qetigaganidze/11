using System;
using System.Collections.Generic;
using System.Linq;

// IEntity Interface
public interface IEntity
{
    int Id { get; set; }
}

// IRepository<T> Interface
public interface IRepository<T> where T : class, IEntity
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

// BaseRepository<T> Class
public class BaseRepository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly List<T> _entities;

    public BaseRepository()
    {
        _entities = new List<T>();
    }

    public void Add(T entity)
    {
        _entities.Add(entity);
    }

    public void Delete(int id)
    {
        var entity = GetById(id);
        if (entity != null)
        {
            _entities.Remove(entity);
        }
    }

    public IEnumerable<T> GetAll()
    {
        return _entities;
    }

    public T GetById(int id)
    {
        return _entities.FirstOrDefault(e => e.Id == id);
    }

    public void Update(T entity)
    {
        var existingEntity = GetById(entity.Id);
        if (existingEntity != null)
        {
            var index = _entities.IndexOf(existingEntity);
            _entities[index] = entity;
        }
    }
}

// Customer Class
public class Customer : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

// CustomerRepository Class
public class CustomerRepository : BaseRepository<Customer>
{
    public Customer GetByEmail(string email)
    {
        return _entities.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
}

// Program Class
class Program
{
    static void Main(string[] args)
    {
        // Create an instance of the CustomerRepository
        var customerRepo = new CustomerRepository();

        // Create and add new customers
        var customer1 = new Customer { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };
        var customer2 = new Customer { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" };

        customerRepo.Add(customer1);
        customerRepo.Add(customer2);

        // Read all customers
        Console.WriteLine("All Customers:");
        foreach (var customer in customerRepo.GetAll())
        {
            Console.WriteLine($"ID: {customer.Id}, Name: {customer.Name}, Email: {customer.Email}");
        }

        // Update a customer
        customer1.Name = "Johnathan Doe";
        customerRepo.Update(customer1);

        // Read updated customer
        var updatedCustomer = customerRepo.GetById(1);
        Console.WriteLine("\nUpdated Customer:");
        Console.WriteLine($"ID: {updatedCustomer.Id}, Name: {updatedCustomer.Name}, Email: {updatedCustomer.Email}");

        // Search for a customer by email
        var searchEmail = "jane.smith@example.com";
        var customerByEmail = customerRepo.GetByEmail(searchEmail);
        Console.WriteLine($"\nCustomer with email {searchEmail}:");
        Console.WriteLine($"ID: {customerByEmail.Id}, Name: {customerByEmail.Name}, Email: {customerByEmail.Email}");

        // Delete a customer
        customerRepo.Delete(1);

        // Read all customers after deletion
        Console.WriteLine("\nAll Customers After Deletion:");
        foreach (var customer in customerRepo.GetAll())
        {
            Console.WriteLine($"ID: {customer.Id}, Name: {customer.Name}, Email: {customer.Email}");
        }
    }
}

