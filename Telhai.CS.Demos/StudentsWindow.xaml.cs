﻿using System;
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
using Telhai.CS.Demos.Models;

namespace Telhai.CS.Demos
{
    /// <summary>
    /// Interaction logic for StudentsWindow.xaml
    /// </summary>
    public partial class StudentsWindow : Window
    {
        StudentsRepository repo;

        public StudentsWindow()
        {
            InitializeComponent();
            repo = new StudentsRepository();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //-Initializer
            Student s1 = new Student { Name = "Moshe", Age = 15 };
            repo.AddStudent(s1);
            //-Initializer
            Student s2 = new Student { Name = "David", Age = 21 };
            repo.AddStudent(s2);

            //-Constractor
            Student s3 = new Student(name: "Yossi", age: 27);
            repo.AddStudent(s3);

            //-Get Current Data from student repo
            //-each List item is object of student
            //-each list item will display the toString of the bounded object (ToString of Student)
            this.listBoxStudents.ItemsSource = repo.Students;



        }

        private void listBoxStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listBoxStudents.SelectedItem is Student s)
            {
                this.txtId.Text = s.Id;
                this.txtName.Text = s.Name;
                this.txtAge.Text = s.Age.ToString();
            }
        }


        int iNoName = 1;
        private void BtnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            Student s = new Student { Name = "NoName_" + iNoName};
            this.repo.AddStudent(s);
            iNoName++;
         
            this.listBoxStudents.ItemsSource = this.repo.Students;
            SetSelectedByIndex(this.listBoxStudents.Items.Count-1);
        
        }

        private void SetSelectedById(string id)
        {

        }

        private void SetSelectedByIndex(int index)
        {
            if (index>=0 && index < this.listBoxStudents.Items.Count)
            {
                this.listBoxStudents.SelectedIndex = index;
            }
        }





    }
}
