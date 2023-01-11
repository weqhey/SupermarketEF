using Microsoft.EntityFrameworkCore;
using SupermarketEF.EF;
using SupermarketEF.Models;
/*
Навести приклади використання union, except, intersect, join, distinct, group by, агрегатних функцій.  
Навести приклади різних стратегій завантаження зв'язаних даних (Eager, Explicit, Lazy). 
Навести приклад завантаження даних що не відслідковуються, їх зміни та збереження. 
Навести приклади виклику збережених процедур та функцій за допомогою Entity Framework.
*/

Function();

void Create()
{
    SupermarketContext context = new SupermarketContext();
    Worker worker = new Worker{ 
        Name = "name7", Birthday = DateTime.Now, Position = "position7", DateOfEmployment = DateTime.Now };
    context.Workers.Add(worker);
    context.SaveChanges();
    foreach(var it in context.Workers)
    {
        Console.WriteLine(it.Id + " " + it.Name);
    }
} 

void Read()
{
    SupermarketContext context = new SupermarketContext();
    foreach(var it in context.Workers)
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
    foreach(var it in context.Products)
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
    foreach(var it in context.Workers)
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