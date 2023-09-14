using D10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static D10.ListGenerators;
using System.Text.RegularExpressions;

namespace operatorOverloading
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> lst = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<string> name = new List<string>() { "Ahmed Ali", "Muhammed Alaa", "Ola Samy", "Alaa Ali" };
            var seq01 = Enumerable.Range(0, 100);
            var seq02 = Enumerable.Range(50, 100);
            //******************frist  way**********************
            //IEnumerable<int>result=Enumerable.Where(lst,i=>i%2==0);
            //************** second way **********************
            //var result = Enumerable.Where(lst, i => i % 2 == 0); // static fun
            //********************** third way most used************************* 
            // second and third called fluent syntax becouse we can use more fun in same query
            //var result = lst.Where(i => i % 2 == 0); //extinsion methood
            //************** fourth way **********************
            // query exepression or query syntax    like sql
            var rr = from i in lst
                     where i % 2 == 0
                     select i;

            //foreach (var item in result)
            //{
            //    Console.WriteLine(item);
            //}
            // used in some only subset of  40 func
            //Console.WriteLine("**********************************************");
            //Console.WriteLine(ProductList[0]);
            //Console.WriteLine("**********************************************");
            #region Where  - Filteration
            var res = ProductList.Where(p => p.UnitsInStock == 0);
            res = from i in res
                  where i.UnitsInStock == 0
                  select i;

            var result01 = ProductList.Where(p => (p.UnitsInStock == 0) && (p.Category == "Meat/Poultry"));
            result01 = from p in ProductList
                       where (p.UnitsInStock == 0) && (p.Category == "Meat/Poultry")
                       select p;
            #endregion

            #region indexedWhere
            result01 = ProductList.Where((p, i) => p.UnitsInStock == 0 && i <= 100);
            // valid only in fluent synatx and can't use in query expression 
            #endregion

            #region select - Transformation
            //var result = ProductList.Select(p => p.ProductName);
            var result = ProductList.Where(p => p.UnitsInStock == 0)
                                    .Select(p => new { ID = p.ProductID, p.ProductName });
            result = from p in ProductList
                     where p.UnitsInStock == 0
                     select new { ID = p.ProductID, p.ProductName };

            var discountedLst = ProductList.Select(p => new Product()
            {
                ProductName = p.ProductName,
                ProductID = p.ProductID,
                Category = p.Category,
                UnitsInStock = p.UnitsInStock,
                UnitPrice = 0.9M * p.UnitPrice
            });

            discountedLst = from p in discountedLst
                            select new Product()
                            {
                                ProductName = p.ProductName,
                                ProductID = p.ProductID,
                                Category = p.Category,
                                UnitsInStock = p.UnitsInStock,
                                UnitPrice = 0.9M * p.UnitPrice
                            };
            // first way to get select before where 
            /* 
             
            var r = ProductList.Select(p => new { Name = p.ProductName, Price = p.UnitPrice })
                               .Where(p => p.Price > 20); 
            */

            // second way to get select before where ( valid for fleunt synatx and query expression)
            /*
             
            var r1 = from p in ProductList
                     select new { Name = p.ProductName, Price = p.UnitPrice };
            var r2 = from p in r1
                     where p.Price > 20
                     select p;
            */
            // third way to get select before where (using INTO)
            var r1 = from p in ProductList
                     select new { Name = p.ProductName, Price = p.UnitPrice }
                     into taxedPrd
                     where taxedPrd.Price < 20
                     select taxedPrd;
            #endregion

            #region orderBy/Descending - thenBy/Decending  (Ordering)
            //fluent 
            var e = ProductList.OrderBy(p => p.UnitsInStock)
                .Select(p => new { p.UnitPrice, p.UnitsInStock });

            e = ProductList.OrderByDescending(p => p.UnitsInStock)
                .Select(p => new { p.UnitPrice, p.UnitsInStock });

            e = ProductList.OrderBy(p => p.UnitsInStock)
                .ThenBy(p => p.UnitPrice)
                .Select(p => new { p.UnitPrice, p.UnitsInStock });

            e = ProductList.OrderByDescending(p => p.UnitsInStock)
               .ThenByDescending(p => p.UnitPrice)
               .Select(p => new { p.UnitPrice, p.UnitsInStock });
            // query Expression

            e = from p in ProductList
                orderby p.UnitsInStock
                select new { p.UnitPrice, p.UnitsInStock };

            e = from p in ProductList
                orderby p.UnitsInStock descending
                select new { p.UnitPrice, p.UnitsInStock };

            e = from p in ProductList
                orderby p.UnitsInStock descending, p.UnitPrice
                select new { p.UnitPrice, p.UnitsInStock }; //= thenBy

            e = from p in ProductList
                orderby p.UnitsInStock, p.UnitPrice descending   // = thenByDescending
                select new { p.UnitPrice, p.UnitsInStock };

            #endregion

            #region Element Operator -- immidiate excecution
            //var d = ProductList.First();
            //                            // both can't handle exception
            // d = ProductList.Last();
            // d = ProductList.FirstOrDefault(); //
            //                                    // both can handle exceptions
            // var d = ProductList.LastOrDefault(p=>p.UnitPrice>20); // 
            //Console.WriteLine(d?.ProductName??"NF");

            // valid only of fluent synatx 
            // but we can write it by Hybrid syntax (Query Expression).element operator();
            var m = (from p in ProductList
                     where p.UnitPrice > 20         // this way called Hybrid syntax
                     select p).FirstOrDefault();
            /* ElementAt / ElementAtOrDefault
             
            m = ProductList.ElementAt(1);
            m = ProductList.ElementAtOrDefault(1);
            Console.WriteLine(m?.ProductName ?? "NF");

            */


            /*m = ProductList.Single();*/ /// throw excption
            //m = ProductList.Single(p=>p.ProductID==7);    // throw excption if two element have same num 


            //m = ProductList.SingleOrDefault(p => p.ProductID == 0); // throw excption if two element have same num 
            // return null if no element match
            // Console.WriteLine(m?.ProductName ?? "NF");

            #endregion

            #region Aggregate -- immdiate 

            //var k = ProductList.Count();
            //k = ProductList.Count(p => p.UnitsInStock == 0);

            /*
            var k = ProductList.Max(); // nased on icomparable
            var k = ProductList.Max(p => p.UnitPrice);
            */

            //var k = ProductList.Min(p => p.ProductName.Length);
            //var l = (from p in ProductList
            //         where p.ProductName.Length == ProductList.Min(p => p.ProductName.Length)
            //         select p).FirstOrDefault();
            //var s = ProductList.Sum(p => p.UnitPrice);
            //var f = ProductList.Average(p => p.UnitPrice);

            //Console.WriteLine(k);
            //Console.WriteLine(s);
            //Console.WriteLine(f);
            //Console.WriteLine(l?.ProductName ?? "NF");
            #endregion

            #region Generateors Operator 
            //// Generate output sequance we call them with only one way by Enumrable class 
            //var r = Enumerable.Range(0, 10);
            //var rEmpty =Enumerable.Empty<Product>();
            //var r2 = Enumerable.Repeat(3, 10);
            //foreach (var item in r2)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #region SelectMany
            //output sequance count > input sequance count 
            // transformation each  element in input sequance to subsequance (IEnumrable)
            // Fluent syntax
            var M = name.SelectMany(FN => FN.Split(" "))
                        .OrderByDescending(SN => SN.Length);
            //foreach (var item in M)
            //{
            //    Console.WriteLine(item);
            //}

            //Query syntax
            // use multable from to product selectmany
            var M1 = from FN in name
                     from SN in FN.Split(" ")
                     orderby SN.Length
                     select SN;
            //foreach (var item in M1)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #region casting Operator
            // ToList()
            List<Product> prd = ProductList.Where(p => p.UnitPrice > 20).ToList();
            //ToArray()
            Product[] prd1 = ProductList.Where(p => p.UnitsInStock == 0).ToArray();
            //HashSet()
            //ToDictionary()
            //ToLookup()
            #endregion

            #region Set Operator
            /* 
            
            var S = seq01.Union(seq02);
             S = seq01.Concat(seq02);
             S = seq01.Distinct();
             S = seq01.Except(seq02);
             S = seq01.Intersect(seq02);

            foreach (var item in S) 
            {
                Console.WriteLine($"{item}  ");
            }
            Console.WriteLine(" ");
            */
            #endregion

            #region Quantifer Operator
            //ProductList.Any(); // RETURN TRUE IF ONE ELEMENT AT LEAST EXIST IN INPUT SEQUANCE 
            //ProductList.Any(P => P.UnitPrice > 20); // RETURN TRUE IF ONE ELEMENT AT LEAST EXIST IN INPUT SEQUANCE MATCH WITH PRIDICATE
            //ProductList.All(P => P.UnitsInStock == 0); //RETURN TRUE IF ALL ELEMENT EXIST IN INPUT SEQUANCE MATCH WITH PRIDICATE   
            //seq01.SequenceEqual(seq02); // compare every element in seq equal the second seq or not ;
            #endregion

            #region zip OPerator
            //var rsult = name.Zip(lst, (FN, i) => new {i , name=FN});
            //foreach (var item in rsult)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #region Grouping 
            var G = from p in ProductList
                    where p.UnitsInStock > 0
                    group p by p.Category
                    into prdgroup
                    where prdgroup.Count() >= 10
                    orderby prdgroup.Count() descending
                    select new {Category = prdgroup.Key , productcount= prdgroup.Count() };

            // fluent 
            G = ProductList.GroupBy(p => p.Category)
                           .Where(prd => prd.Count() >= 10)
                           .OrderByDescending(prdgroup => prdgroup.Count())
                           .Select(prdgroup => new { Category = prdgroup.Key, productcount = prdgroup.Count() });

            //foreach (var item in G)     
            //{
            //    Console.WriteLine(item) ;
            //}


            #endregion

            #region Let/Into

            // INTO
            var L = from n in name
                    select Regex.Replace(n, "[aeiouAEIOU]", string.Empty)
                    into Novol
                    where Novol.Length >= 3
                    orderby Novol, Novol.Length descending
                    select Novol;
            //LET
            var I = from N in name
                    let Novel = Regex.Replace(N, "[aeiouAEIOU]", string.Empty)
                    where Novel.Length >= 3
                    orderby Novel, Novel.Length descending
                    select Novel;
            foreach (var item in I)
            {
                Console.WriteLine(item);
            }
            #endregion

        }
    }
}
