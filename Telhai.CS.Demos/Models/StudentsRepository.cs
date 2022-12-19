using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Telhai.CS.Demos.Models
{
    public class StudentsRepository : IStudentsRepository
    {
        private List<Student> _students;
        public StudentsRepository()
        {
            _students = new List<Student>();
        }

        public Student[] Students
        {
            get { return _students.ToArray();  }
           
        }

        public void AddStudent(Student student)
        {
            this._students.Add(student);
        }

        public void UpdateStudent(Student student)
        {
            int indexFound =  this._students.FindIndex(s => s.Id == student.Id);
            if (indexFound >= 0)
            {
                this._students[indexFound] = student;
           
            }

        }

        public string LoadAllStudents() {
            string jsonPath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    jsonPath =  openFileDialog.FileName;
                }

            if (jsonPath != string.Empty)
            {
                //1) Load Student from Text As Object
                //From User Selected File
                //
                string studentsText = File.ReadAllText(jsonPath);
                var studentsList =
                JsonSerializer.Deserialize<Student[]>(studentsText);
                //2)Add Objects to Repo Manager

                foreach (Student item in studentsList)
                {
                    AddStudent(item);
                }
                string filename = Path.GetFileName(jsonPath);
                string[] filenameArray = filename.Split(".");
                return filenameArray[0];
            }
            return "Students";
        }

        public void SaveAllStudents() {

           // List<Student> students = Students.ToList(); ;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonStudentsString = JsonSerializer.Serialize<List<Student>>(_students, options);

            if (!Directory.Exists("AppData"))
            {
                Directory.CreateDirectory("AppData");
            }

            File.WriteAllText("AppData/students.json", jsonStudentsString);
        
        }

        public void RemoveStudent(string id)
        {
            int indexFound = this._students.FindIndex(s => s.Id == id);
            if (indexFound >= 0)
            {
                this._students.RemoveAt(indexFound);
            }
        }

    }
}
