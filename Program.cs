using System;
using System.Collections.Generic;
using System.Linq;
using BuyerTable;
using ShoppingTable;

namespace LINQTask
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            var buyers = new List<Buyer>()
            {
                new Buyer(1, "Max"),
                new Buyer(2, "Tom"),
                new Buyer(3, "Bob"),
                new Buyer(5, "Kate"),
                new Buyer(6, "Jane")
            };

            var shoppings = new List<Shopping>()
            {
                new Shopping(1, 100.98M, 6),
                new Shopping(2, 5M, 1),
                new Shopping(3, 1000.1M, 2),
                new Shopping(4, 400M, 6),
                new Shopping(5, 800M, 3),
                new Shopping(6, 40.50M, 3),
                new Shopping(7, 100M, 6),
                new Shopping(8, 10.10M, 3),
                new Shopping(9, 995.1M, 1)
            };

            var result = buyers.GroupJoin(shoppings,
                                buyer => buyer.Id,
                                shopping => shopping.BuyersId,
                                (buyer, shopping) => (buyer.Name, shopping.Sum(summa => summa.Summa)))
                                .OrderByDescending(s => s.Item2)
                                .Take(1);

            foreach (var emp in result)
                Console.WriteLine($"{emp.Name} - {emp.Item2}");

            
            Console.WriteLine("Новое");

            var sqlLike = from b in buyers
                          join s in shoppings on b.Id equals s.BuyersId
                          select new
                          {
                              Name = b.Name,
                              Summa = s.Summa,
                          } into temp
                          group temp by temp.Name into g
                          select new
                          {
                              Name = g.Key,
                              Summa = g.Sum(s => s.Summa)
                          } into temp
                          orderby temp.Summa descending
                          select new { Name = temp.Name, Summa = temp.Summa };

            foreach (var emp in sqlLike.Take(1))
                Console.WriteLine($"{emp.Name} - {emp.Summa}");

            Console.WriteLine("Получение всех лучих покупателей:");

            var groupedDataSet = from s in shoppings
                           group s by s.BuyersId into g
                           select new
                           {
                               BuyersId = g.Key,
                               Summa = g.Sum(summa => summa.Summa)
                           };

            var maxValue = groupedDataSet.Max(summa => summa.Summa);

            var sqlLike2 = from b in buyers
                          join s in (
                          from ss in shoppings
                          group ss by ss.BuyersId into g
                          select new
                          {
                              BuyersId = g.Key,
                              Summa = g.Sum(summa => summa.Summa)
                          }
                          ) on b.Id equals s.BuyersId into temp
                          from t in temp
                          where t.Summa == temp.Max(summa => summa.Summa)
                          select new
                          {
                              Name = b.Name,
                              Summa = t.Summa,
                          } into temp
                          where temp.Summa == maxValue
                          select temp;                  


            foreach (var emp in sqlLike2)
                Console.WriteLine($"{emp.Name} - {emp.Summa}");
        }
    }
}
