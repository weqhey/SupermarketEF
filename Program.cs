﻿using Microsoft.EntityFrameworkCore;
using SupermarketEF.EF;
using SupermarketEF.Models;

GetProductsTypeBySales();

void GetProductsTypeBySales()
{
    SupermarketContext context = new SupermarketContext();
    var sum = context.ProductInReceipts.Sum(p => p.Amount);
    var query = context.Products.Join(
        context.ProductInReceipts,
        product => product.Id,
        receipt => receipt.ProductId,
        (product, receipt) => new { product.ProductType, Amount = receipt.Amount })
        .GroupBy(p => p.ProductType, 
            (key, amount) => new { ProductType = key, Amount = (amount.Sum(p => p.Amount) * 100)/sum})
        .OrderByDescending(p => p.Amount)
        .Select(p => new { p.ProductType, p.Amount });
    foreach(var it in query)
    {
        Console.WriteLine(it);
    }    
}

void Create()
{
    SupermarketContext context = new SupermarketContext();
    Worker worker = new Worker
    {
        Name = "name7",
        Birthday = DateTime.Now,
        Position = "position7",
        DateOfEmployment = DateTime.Now
    };
    context.Workers.Add(worker);
    context.SaveChanges();
    foreach (var it in context.Workers)
    {
        Console.WriteLine(it.Id + " " + it.Name);
    }
}

void Read()
{
    SupermarketContext context = new SupermarketContext();
    foreach (var it in context.Workers)
    {
        Console.WriteLine(it.Id + " " + it.Name);
    }
}

void Update()
{
    SupermarketContext context = new SupermarketContext();
    var newPrice = context.Products.Where(p => p.Price == 100).First();
    newPrice.Price = 1000;
    context.SaveChanges();
    foreach (var it in context.Products)
    {
        Console.WriteLine(it.Id + " " + it.Price);
    }
}
void Delete()
{
    SupermarketContext context = new SupermarketContext();
    var fired = context.Workers.Where(x => x.Id == 6).First();
    context.Workers.Remove(fired);
    context.SaveChanges();
    foreach (var it in context.Workers)
    {
        Console.WriteLine(it.Id + " " + it.Name);
    }
}

void Procedure()
{
    SupermarketContext context = new SupermarketContext();
    var products = context.Products.FromSql($"EXECUTE dbo.GetProducts").ToList();

    foreach (var item in products)
    {
        Console.WriteLine($"{item.Name} " + $"{item.Price}");
    }
}

void Function()
{
    SupermarketContext context = new SupermarketContext();
    var product = context.Products.FromSql($"SELECT * FROM dbo.GetProductById(1)").Single();
    Console.WriteLine($"{product.Name} " + $"{product.Price}");
}

void EagerLoading()
{
    SupermarketContext context = new SupermarketContext();
    var receipts = context.ProductInReceipts.Include(p => p.Product).ToList();
    foreach (var it in receipts)
    {
        Console.WriteLine(it.Product?.Name);
    }
}

void ExplicitLoading()
{
    SupermarketContext context = new SupermarketContext();
    var receipt = context.ProductInReceipts.First();
    if (receipt != null)
    {
        context.Products.Where(p => p.Id == receipt.ProductId).Load();
        Console.WriteLine(receipt?.Product.Name);
    }
}
void LazyLoading()
{
    SupermarketContext context = new SupermarketContext();
    var receipts = context.ProductInReceipts.ToList();
    foreach (var it in receipts)
    {
        Console.WriteLine(it.Product?.Name);
    }
}

void AsNoTracking()
{
    SupermarketContext context = new SupermarketContext();
    var product = context.Products.AsNoTracking().First();
    if (product != null)
    {
        product.Name = "New name";
        context.SaveChanges();
    }
    var products = context.Products.ToList();
    foreach (var it in products)
    {
        Console.WriteLine(it.Name);
    }
}

void Union()
{
    SupermarketContext context = new SupermarketContext();
    var query = context.Workers.Select(p => p.Id).Union(context.Receipts.Select(p => p.WorkerId));
    foreach (var it in query)
    {
        Console.WriteLine(it);
    }
}

void Except()
{
    SupermarketContext context = new SupermarketContext();
    var query = context.Workers.Select(p => p.Id).Except(context.Receipts.Select(p => p.WorkerId));
    foreach (var it in query)
    {
        Console.WriteLine(it);
    }
}

void Intersect()
{
    SupermarketContext context = new SupermarketContext();
    var query = context.Workers.Select(p => p.Id).Intersect(context.Receipts.Select(p => p.WorkerId));
    foreach (var it in query)
    {
        Console.WriteLine(it);
    }
}

void Distinct()
{
    SupermarketContext context = new SupermarketContext();
    var query = context.Products.Select(p => p.Price).Distinct();
    foreach (var it in query)
    {
        Console.WriteLine(it);
    }
}

void Join()
{
    SupermarketContext context = new SupermarketContext();
    var query = context.Products.Join(
        context.ProductInReceipts,
        product => product.Id,
        productInReceipt => productInReceipt.ProductId,
        (product, productInReceipt) => new
        {
            ProductInReceiptId = productInReceipt.ProductId,
            ProductId = product.Id
        }
    );
    foreach (var it in query)
    {
        Console.WriteLine(it);
    }
}

void GroupBy()
{
    SupermarketContext context = new SupermarketContext();
    var query = context.Receipts.Join(
        context.Workers,
        receipt => receipt.WorkerId,
        worker => worker.Id,
        (receipt, worker) => new
        {
            WorkerId = worker.Id,
            ReceiptId = receipt.Id
        })
        .GroupBy(p => p.WorkerId)
        .Select(m => new
        {
            m.Key,
            Count = m.Count()
        });
    foreach (var it in query)
    {
        Console.WriteLine(it);
    }
}

void Count()
{
    SupermarketContext context = new SupermarketContext();
    var query = context.Workers.Count();
    Console.WriteLine(query);
}

async Task AddAsync()
{
    SupermarketContext context = new SupermarketContext();
    for (int i = 0; i < 10; i++)
    {
        await context.Workers.AddAsync(new Worker
        {
            Name = i.ToString(),
            Position = i.ToString()
        });
        await context.SaveChangesAsync();
    }
}
async Task ReadAsync()
{
    SupermarketContext context = new SupermarketContext();
    var list = await context.Workers.ToListAsync();
    foreach (var it in list)
    {
        Console.WriteLine(it.Name);
    }
}
void LockExample()
{
    SupermarketContext context = new SupermarketContext();
    object locker = new object();
    int cnt = 0;
    for (int i = 0; i < 10; i++)
    {
        Thread newThread = new(() =>
        {
            for (int j = 0; j < 100; j++)
            {
                lock (locker)
                {
                    context.Workers.Add(new Worker
                    {
                        Name = cnt.ToString(),
                        Position = cnt.ToString()
                    });
                    cnt++;
                    Console.WriteLine(cnt);
                    context.SaveChanges();
                }
            }
        });
        newThread.Start();
        Thread.Sleep(100);
    }
}

void MonitorExample()
{
    SupermarketContext context = new SupermarketContext();
    object locker = new object();
    int cnt = 0;
    for (int i = 0; i < 10; i++)
    {
        Thread newThread = new(() =>
        {
            for(int j = 0; j < 100; j++)
            {
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(locker, ref lockTaken);
                    context.Workers.Add(new Worker
                    {
                        Name = cnt.ToString(),
                        Position = cnt.ToString()
                    });
                    cnt++;
                    Console.WriteLine(cnt);
                    context.SaveChanges();
                }
                finally
                {
                    if (lockTaken) Monitor.Exit(locker);
                }
            }
        });
        newThread.Start();
        Thread.Sleep(100);
    }
}

void MutexExample()
{
    SupermarketContext context = new SupermarketContext();
    Mutex mutexObj = new Mutex();
    int cnt = 0;
    for (int i = 0; i < 10; i++)
    {
        Thread newThread = new(() =>
        {
            for (int j = 0; j < 100; j++)
            {
                mutexObj.WaitOne();
                context.Workers.Add(new Worker
                {
                    Name = cnt.ToString(),
                    Position = cnt.ToString()
                });
                context.SaveChanges();
                cnt++;
                Console.WriteLine(cnt);
                mutexObj.ReleaseMutex();
            }
        });
        newThread.Start();
        Thread.Sleep(1000);
    }
}