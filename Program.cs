using SupermarketEF.EF;
using SupermarketEF.Models;

Create();
Read();
Update();
Delete();

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