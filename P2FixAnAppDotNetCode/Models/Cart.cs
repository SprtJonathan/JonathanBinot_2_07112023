using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace P2FixAnAppDotNetCode.Models
{
    /// <summary>
    /// The Cart class
    /// </summary>
    public class Cart : ICart
    {
        /// <summary>
        /// List used to contain all the products added to the Cart
        /// </summary>
        private List<CartLine> cartLine = new List<CartLine>();

        /// <summary>
        /// Read-only property for dispaly only
        /// </summary>
        public IEnumerable<CartLine> Lines => GetCartLineList();

        /// <summary>
        /// Return the actual cartline list
        /// </summary>
        /// <returns></returns>
        private List<CartLine> GetCartLineList()
        {
            return cartLine;
        }

        /// <summary>
        /// Adds a product in the cart or increment its quantity in the cart if already added
        /// </summary>//
        public void AddItem(Product product, int quantity)
        {
            // Check if an article is already in the cart
            CartLine existingArticle = GetCartLineList().Find(productToFind => productToFind.Product.Id == product.Id);

            // If the article isn't already in the cart, we simply add it
            if (existingArticle == null)
            {
                GetCartLineList().Add(new CartLine()
                {
                    OrderLineId = Lines.Count(),
                    Product = product,
                    Quantity = quantity,
                });
            }
            // Else if the article is already in the cart, we simply increase its quantity
            else
            {
                existingArticle.Quantity += quantity;
            }
        }

        /// <summary>
        /// Removes a product form the cart
        /// </summary>
        public void RemoveLine(Product product) =>
            GetCartLineList().RemoveAll(l => l.Product.Id == product.Id);

        /// <summary>
        /// Get total value of a cart
        /// </summary>
        public double GetTotalValue()
        {
            // Calculate total cart value by making the sum of all products multiplied by their quantity
            return Lines.Sum(productValue => productValue.Product.Price * productValue.Quantity);
        }

        /// <summary>
        /// Get average value of a cart
        /// </summary>
        public double GetAverageValue()
        {
            // Declare the quantity and price variables
            int quantity = 0;
            double priceTotal = 0.0;

            // For each product in the cart, we increment the quantity and the total price
            foreach (CartLine line in Lines)
            {
                // Get the quantity of products
                quantity += line.Quantity;

                // Get the price of product multiplied by the quantity
                priceTotal += line.Product.Price * line.Quantity;
            }

            // Return the average price per product in the cart
            return priceTotal / quantity;
        }

        /// <summary>
        /// Looks after a given product in the cart and returns if it finds it
        /// </summary>
        public Product FindProductInCartLines(int productId)
        {
            // Find product matching with given Id
            return GetCartLineList().Find(productToFind => productToFind.Product.Id == productId).Product;
        }

        /// <summary>
        /// Get a specifid cartline by its index
        /// </summary>
        public CartLine GetCartLineByIndex(int index)
        {
            return Lines.ToArray()[index];
        }

        /// <summary>
        /// Clears a the cart of all added products
        /// </summary>
        public void Clear()
        {
            List<CartLine> cartLines = GetCartLineList();
            cartLines.Clear();
        }
    }

    public class CartLine
    {
        public int OrderLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
