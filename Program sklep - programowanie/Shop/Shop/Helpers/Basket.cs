using Shop.Models;
using System.Data;

namespace Shop.Helpers
{
    public static class Basket
    {
        public static List<Product> products;
        public static double productsCost;
        public static int productsAmount;
        public static void AddProduct(Product product)
        {
            if(products == null)
            {
                products = new List<Product>();
            }

            products.Add(product);
            productsCost += product.Price * product.Amount;
            productsAmount += product.Amount;
        }
        public static void RemoveProduct(int index)
        {
            var product = products[index];
            productsCost -= product.Price * product.Amount;
            productsAmount -= product.Amount;
            products.RemoveAt(index);
            
        }

        public static void Clear()
        {
            products.Clear();
            productsCost = 0;
            productsAmount = 0;
        }
    }
}
