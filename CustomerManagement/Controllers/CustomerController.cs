using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

public class CustomerController : Controller
{
    private readonly IMemoryCache _memoryCache;

    public CustomerController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public IActionResult Index()
    {
        var customers = _memoryCache.Get<List<Customer>>("Customers") ?? new List<Customer>();
        return View(customers);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Customer newCustomer)
    {
        if (!ModelState.IsValid)
        {
            return View(newCustomer);
        }

        var customers = _memoryCache.Get<List<Customer>>("Customers") ?? new List<Customer>();
        if (customers.Count == 0)
        {
            newCustomer.Id = 1; 
        }
        else
        {
            
            newCustomer.Id = customers.Last().Id + 1;
        }

        customers.Add(newCustomer);
        _memoryCache.Set("Customers", customers);

        return RedirectToAction("Index");
    }


    public IActionResult Edit(int id)
    {
        var customers = _memoryCache.Get<List<Customer>>("Customers") ?? new List<Customer>();
        var customer = customers.FirstOrDefault(c => c.Id == id);

        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    
    

    public IActionResult Delete(int id)
    {
        var customers = _memoryCache.Get<List<Customer>>("Customers") ?? new List<Customer>();
        var customer = customers.FirstOrDefault(c => c.Id == id);

        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var customers = _memoryCache.Get<List<Customer>>("Customers") ?? new List<Customer>();
        var customerToRemove = customers.FirstOrDefault(c => c.Id == id);

        if (customerToRemove == null)
        {
            return NotFound();
        }

        customers.Remove(customerToRemove);
        _memoryCache.Set("Customers", customers);

        return RedirectToAction("Index");
    }
    
    public IActionResult Details(int id)
    {
        var customers = _memoryCache.Get<List<Customer>>("Customers") ?? new List<Customer>();
        var customer = customers.FirstOrDefault(c => c.Id == id);

        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    [HttpPost]
    public IActionResult Edit(Customer editedCustomer)
    {
        if (!ModelState.IsValid)
        {
            return View(editedCustomer);
        }

        var customers = _memoryCache.Get<List<Customer>>("Customers") ?? new List<Customer>();
        var existingCustomer = customers.FirstOrDefault(c => c.Id == editedCustomer.Id);

        if (existingCustomer == null)
        {
            return NotFound();
        }

        existingCustomer.FirstName = editedCustomer.FirstName;
        existingCustomer.LastName = editedCustomer.LastName;
        existingCustomer.Email = editedCustomer.Email;
        existingCustomer.Identity = editedCustomer.Identity;
        existingCustomer.Phone = editedCustomer.Phone;
        existingCustomer.BirthDate = editedCustomer.BirthDate;

        _memoryCache.Set("Customers", customers);

        return RedirectToAction("Details", new { id = existingCustomer.Id });
    }

   
}