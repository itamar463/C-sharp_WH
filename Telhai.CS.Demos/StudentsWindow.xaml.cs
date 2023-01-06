using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
    public partial class StudentsWindow : Window
    {
        IStudentsRepository repo; // coellction of students
        Dictionary<string, int> faculties = new Dictionary<string, int>(); //for combo box

        public StudentsWindow(IStudentsRepository repo)
        {
            InitializeComponent();
            this.repo = repo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //when window load add present "all" students in window
            faculties.Add("All", 1);
            FacultyCombo.Items.Add("All");
        }

        private void listBoxStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //present students in list box
            if (this.listBoxStudents.SelectedItem is Student s)
            {
                this.txtId.Text = s.Id;
                this.txtName.Text = s.Name;
                this.txtAge.Text = s.Age.ToString();
                this.txtFac.Text = s.Faculty;
                this.imgStudent.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + s.StudentImage));
            }
        }


        int iNoName = 1;
        private void BtnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            //add student to repo
            Student s = new Student { Name = "NoName_" + iNoName, Faculty = "Unknown" };
            this.repo.AddStudent(s);
            iNoName++;
            if (faculties.ContainsKey("Unknown"))
            {
                faculties["Unknown"]++;
            }
            else
            {
                faculties.Add("Unknown", 1);
                addFacToComboBox("Unknown");
            }
            FacultyComboUpdate();
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
            //set selected student by index in list box
            if (index >= 0 && index < this.listBoxStudents.Items.Count)
            {
                this.listBoxStudents.SelectedIndex = index;
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            // remove student from repo
            if (this.listBoxStudents.SelectedItem is Student s)
            {
                repo.RemoveStudent(s.Id);
                faculties[s.Faculty]--;
                if (faculties[s.Faculty] == 0)
                {
                    faculties.Remove(s.Faculty);
                    FacultyCombo.Items.Remove(s.Faculty);
                    FacultiesUpdate();
                }
                FacultyComboUpdate();
                SetSelectedByIndex(0);
                repo.SaveAllStudents();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) // "update" button
        {
            // update student info
            bool isChange = false;
            if (this.listBoxStudents.SelectedItem is Student s)
            {
                if (s.Name != txtName.Text || s.Faculty != txtFac.Text || s.Age.ToString() != txtAge.Text)
                {
                    isChange = true;
                    string originFac = s.Faculty;
                    s.Name = txtName.Text;
                    s.Faculty = txtFac.Text;
                    if (s.Faculty != originFac)
                    {
                        //if you update some facluty and what it was before was the last object from this faculty
                        faculties[originFac]--;
                        if (faculties[originFac] == 0)
                        {
                            faculties.Remove(originFac);
                            FacultyCombo.Items.Remove(originFac);
                            FacultiesUpdate();
                        }
                    }
                    //increase students amount in the updated faculty
                    if (faculties.ContainsKey(s.Faculty)) faculties[s.Faculty]++;
                    else
                    {
                        //new faculty
                        faculties.Add(s.Faculty, 1);
                        addFacToComboBox(s.Faculty);
                    }
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
                    FacultyComboUpdate();
                }
                
            }
            if (isChange) repo.SaveAllStudents();
            
        }

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            //load data from json
            this.Title = repo.LoadAllStudents();
            this.listBoxStudents.ItemsSource = repo.Students;
            foreach (Student student in repo.Students)
            {
                if (!faculties.ContainsKey(student.Faculty))
                {
                    faculties.Add(student.Faculty, 1);
                }
                else
                {
                    faculties[student.Faculty]++;
                }

            }
            FacultiesUpdate();
        }
        private void addFacToComboBox(string fac)
        {
            //add new faculty to combo box
            FacultyCombo.Items.Add(fac);
        }
        private void FacultiesUpdate()
        {
            //update combobox for loading or for removing whole faculty   
            FacultyCombo.Items.Clear();
            foreach (KeyValuePair<string, int> item in faculties)
            {
                FacultyCombo.Items.Add(item.Key);
            }
        }
        private void FacultyComboUpdate()
        {
            //update faculty combobox
            int i = this.FacultyCombo.SelectedIndex;
            if (i == -1)
            {
                listBoxStudents.ItemsSource = repo.Students;
                return;
            }
            string? fac = FacultyCombo.Items.GetItemAt(i).ToString();
            if (fac == "All")
            {
                listBoxStudents.ItemsSource = repo.Students;
                return;
            }
            List<Student> lst = new List<Student>();
            if (repo.Students.Length > 0)
            {
                foreach (Student student in repo.Students)
                {
                    if (student.Faculty == fac)
                    {
                        lst.Add(student);
                    }
                }
                listBoxStudents.ItemsSource = lst;
                if (lst.Count > 0)
                {
                    this.listBoxStudents.SelectedIndex = 0;
                }
            }
        }
        private void FacultyCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //showing chosen faculty acrroding to selected item
            int i = this.FacultyCombo.SelectedIndex;
            if (i == -1) return;
            string? fac = FacultyCombo.Items.GetItemAt(i).ToString();
            if (fac == "All")
            {
                listBoxStudents.ItemsSource = repo.Students;
                return;
            }
            List<Student> lst = new List<Student>();
            if (repo.Students.Length > 0)
            {
                foreach (Student student in repo.Students)
                {
                    if (student.Faculty == fac)
                    {
                        lst.Add(student);
                    }
                }
                listBoxStudents.ItemsSource = lst;
                if (lst.Count > 0)
                {
                    this.listBoxStudents.SelectedIndex = 0;
                }
            }
        }


        private void UpdatePhotoBTN_Click(object sender, RoutedEventArgs e)
        {
            //change image to student
            if (this.listBoxStudents.SelectedItem is Student s)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string imagePath = openFileDialog.FileName;
                    string currPath = "\\img\\img_" + s.Name + ".png";
                    string currDir = Directory.GetCurrentDirectory() + currPath;
                    File.Copy(imagePath, currDir, true);
                    s.StudentImage = currPath;
                    this.imgStudent.Source = new BitmapImage(new Uri(currDir));
                }
                repo.SaveAllStudents();
            }
        }
    }
}
