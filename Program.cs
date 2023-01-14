using Microsoft.EntityFrameworkCore;
using SupermarketEF.EF;
using SupermarketEF.Models;
/*
Навести приклади використання union, except, intersect, join, distinct, group by, агрегатних функцій.  
*/

GroupBy();
Count();

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
    foreach(var it in query)
    {
        Console.WriteLine(it);
    }
}

void Except()
{
    SupermarketContext context = new SupermarketContext();
    var query = context.Workers.Select(p => p.Id).Except(context.Receipts.Select(p => p.WorkerId));
    foreach(var it in query)
    {
        Console.WriteLine(it);
    }
}

void Intersect()
{
    SupermarketContext context = new SupermarketContext();
    var query = context.Workers.Select(p => p.Id).Intersect(context.Receipts.Select(p => p.WorkerId));
    foreach(var it in query)
    {
        Console.WriteLine(it);
    }
}

void Distinct()
{

}

void Join()
{
    SupermarketContext context = new SupermarketContext();
    var query = context.Products.Join(
        context.ProductInReceipts,
        product => product.Id,
        productInReceipt => productInReceipt.ProductId,
        (product, productInReceipt) => new { 
            ProductInReceiptId = productInReceipt.ProductId,
            ProductId = product.Id
        }
    );
    foreach(var it in query)
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
        (receipt, worker) => new { 
            WorkerId = worker.Id,
            ReceiptId = receipt.Id
        }) 
        .GroupBy(p => p.WorkerId)
        .Select(m => new
        {
            m.Key,
            Count = m.Count()
        });
    foreach(var it in query)
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