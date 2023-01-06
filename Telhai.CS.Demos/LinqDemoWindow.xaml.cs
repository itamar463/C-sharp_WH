using LINQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Telhai.CS.Demos
{
    /// <summary>
    /// Interaction logic for LinqDemoWindowxaml.xaml
    /// </summary>
    public partial class LinqDemoWindow : Window
    {
        public LinqDemoWindow()
        {
            InitializeComponent();
        }

        public bool MyFunc(int n)
        {
            return n < 5;
        }
        private void demo1_Click(object sender, RoutedEventArgs e)
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0, 10 };
            //Query syntax
            var data = from num in numbers
                       where num < 5
                       select (num);
            // lambda func syntax
            var dataFunc1 = numbers.Where(n => n < 5).Select(n => n*2);
            var dataFunc2 = numbers.Where(MyFunc).Select(n => n * 3);
            foreach (var item in dataFunc2)
            {
                ResultTextBox.Text += "[" + item.ToString() + "] ";
            }
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            //var shortDigits = digits.Where((digit, index) => digit.Length < index).Select(index,digit,digit.Length);
           // var shortDigits = digits.Select((digit, index) => $"{index}.{digit} ({digit.Length})");
            var shortDigits = digits.Where((digit, index) => digit.Length < index).Select((digit, index) => $"{index}.{digit} ({digit.Length})");
            foreach (var item in shortDigits)
            {
                ResultTextBox.Text += "\n" + item.ToString(); 
            }

        }

        private void demo2_Click(object sender, RoutedEventArgs e)
        {
            //--Working On Objects
            //--Project Result with anonymous multiuValues
            List<Product> productsSource = Products.ProductList;
            var productsQuery = from p in productsSource
                                where p.UnitsInStock > 0 && p.UnitPrice > 3
                                select new { ProductCat = p.Category + "_" + p.ProductName, KUKU = p.ProductID };
            foreach (var itemQuery in productsQuery)
            {
                ResultTextBox.Text += itemQuery.ToString();
            }

        }
    }
}
