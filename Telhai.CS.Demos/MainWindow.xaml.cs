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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telhai.CS.Demos.Models;
using Telhai.CS.Logging;

namespace Telhai.CS.Demos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {

            //--1.Initilize Calc Window class
            CalcWindow calcWin = new CalcWindow();
            //--2.Show Dialog  (blocking method till window close)
            calcWin.ShowDialog();
            //--3.read the public allExpressions field from calcWin
            List<string> userList = calcWin.allExpressions;
            //--4 copy CalcWindow inner list
            //to current listElem items
            foreach (var item in userList)
            {
                this.listElem.Items.Add(item);
            }
        }
                  
          
                

        private void btn2_Click(object sender, RoutedEventArgs e)
        {

            StudentsWindow s = new StudentsWindow(new StudentsDictionaryRepository());
            s.Show();
        }

        private void listElem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if (e.AddedItems.Count > 0 )
            {
                int indexSelected =  listElem.SelectedIndex;
                string val =  listElem.SelectedItem.ToString();
               // MessageBox.Show($"Index:{indexSelected+1}\nContent:{val}");
                // string valueSelected = ((string)e.AddedItems[0]);
                //MessageBox.Show(valueSelected);
                
                Logger.Log($"Index:{indexSelected + 1}\nContent:{val}");


            }
            
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            JsonDemos d = new JsonDemos();
            d.Show();
        }
    }
}
