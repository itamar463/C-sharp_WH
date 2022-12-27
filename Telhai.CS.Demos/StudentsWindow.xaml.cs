using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        IStudentsRepository repo;
        List<String> faculties = new List<string> { "Unknown", "Computer Sceince", "Biotechnology", "Psychology", "All" };


        public StudentsWindow(IStudentsRepository repo)
        {
            InitializeComponent();
            this.repo = repo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < faculties.Count; i++)
            {
                txtFac.Items.Add(faculties[i]);
                FacultyCombo.Items.Add(faculties[i]);
            }
        }

        private void listBoxStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listBoxStudents.SelectedItem is Student s)
            {
                this.txtId.Text = s.Id;
                this.txtName.Text = s.Name;
                this.txtAge.Text = s.Age.ToString();
                this.txtFac.Text = s.Faculty;
                this.imgStudent.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + s.StudentImage));
                // this.imgStudent.Source = s.StudentImage.Source;
            }
        }


        int iNoName = 1;
        private void BtnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            Student s = new Student { Name = "NoName_" + iNoName, Faculty = faculties[0] };
            this.repo.AddStudent(s);
            iNoName++;

            this.listBoxStudents.ItemsSource = this.repo.Students;
            //SetSelectedByIndex(this.listBoxStudents.Items.Count-1);
            SetSelectedById(s.Id);
            repo.SaveAllStudents();
        }

        private void SetSelectedById(string id)
        {
            for (int i = 0; i < this.listBoxStudents.Items.Count; i++)
            {

                Student? s = listBoxStudents.Items[i] as Student;
                if (s != null)
                {
                    if (s.Id == id)
                        this.listBoxStudents.SelectedItem = s;
                }

            }
        }

        private void SetSelectedByIndex(int index)
        {
            if (index >= 0 && index < this.listBoxStudents.Items.Count)
            {
                this.listBoxStudents.SelectedIndex = index;
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBoxStudents.SelectedItem is Student s)
            {
                repo.RemoveStudent(s.Id);
            }

            this.listBoxStudents.ItemsSource = repo.Students;
            SetSelectedByIndex(0);
            repo.SaveAllStudents();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) // "update" button
        {
            bool isChange = false;
            if (this.listBoxStudents.SelectedItem is Student s)
            {
                if (s.Name != txtName.Text || s.Faculty != txtFac.Text || s.Age.ToString() != txtAge.Text)
                {
                    isChange = true;
                    s.Name = txtName.Text;
                    s.Faculty = txtFac.Text;
                    int convertedAge;
                    bool isOk = int.TryParse(txtAge.Text, out convertedAge);
                    if (isOk)
                    {
                        s.Age = convertedAge;
                    }
                    s.Id = this.txtId.Text;

                    this.repo.UpdateStudent(s);
                    this.listBoxStudents.ItemsSource = repo.Students;
                    this.SetSelectedById(s.Id);
                }
            }
            if (isChange) repo.SaveAllStudents();
        }

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {

            this.Title = repo.LoadAllStudents();
            this.listBoxStudents.ItemsSource = repo.Students;

        }
        private void FacultyCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? fac;
            int i = this.FacultyCombo.SelectedIndex;
            switch (i)
            {
                //--Calculte results
                case (0):
                    {
                        fac = faculties[0]; //unknown
                        break;
                    }
                case (1):
                    {
                        fac = faculties[1]; // cs
                        break;
                    }
                case (2):
                    {
                        fac = faculties[2]; //bio
                        break;
                    }
                case (3):
                    {
                        fac = faculties[3]; // psy
                        break;
                    }
                case (4):
                    {
                        fac = faculties[4];
                        break;
                    }
                default:
                    {
                        fac = "error";
                        break;
                    }
            }


            List<Student> lst = new List<Student>();
            listBoxStudents.ItemsSource = repo.Students;
            if (listBoxStudents.Items.Count > 0)
            {
                if (fac == "All")
                {
                    return;
                }
                foreach (Student student in listBoxStudents.Items)
                {
                    if (student.Faculty == fac)
                    {
                        lst.Add(student);
                    }
                }
                listBoxStudents.ItemsSource = lst;

            }

        }


        private void UpdatePhotoBTN_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBoxStudents.SelectedItem is Student s)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string imagePath = openFileDialog.FileName;
                    string currPath = "\\img\\img_" + s.Name + ".png";
                    string currDir = Directory.GetCurrentDirectory() + currPath;
                    File.Copy(imagePath, currDir, true);

                    // s.StudentImage.Source = new BitmapImage(new Uri(currDir));
                    s.StudentImage = currPath;
                    this.imgStudent.Source = new BitmapImage(new Uri(currDir));
                }
                repo.SaveAllStudents();
            }
        }
    }
}
