using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Telhai.CS.Demos.Models
{
    internal class StudentsDictionaryRepository : IStudentsRepository
    {
        private Dictionary<string,Student> _students;
        public StudentsDictionaryRepository()
        {
            _students = new Dictionary<string, Student>();
        }

        public Student[] Students
        {
            get { return _students.Values.ToArray(); }

        }

        public void AddStudent(Student student)
        {
            this._students.Add(student.Id,student);
        }

        public void UpdateStudent(Student student)
        {
            //int indexFound = this._students.FindIndex(s => s.Id == student.Id);
            if (this._students.Count >= 0)
            {
                if (_students.Keys.Contains(student.Id))
                {
                    this._students[student.Id] = student;
                }
            }

        }

        public string LoadAllStudents()
        {
            string jsonPath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                jsonPath = openFileDialog.FileName;
            }

            if (jsonPath != string.Empty)
            {
                //1) Load Student from Text As Object
                //From User Selected File
                //
                string studentsText = File.ReadAllText(jsonPath);
                var studentsDict =
                JsonSerializer.Deserialize<Dictionary<string,Student>>(studentsText);
                //2)Add Objects to Repo Manager

               foreach(KeyValuePair<string,Student> item in studentsDict)
                {
                    AddStudent(item.Value);
                }
                string filename = Path.GetFileName(jsonPath);
                string[] filenameArray = filename.Split(".");
                return filenameArray[0];
            }
            return "Students";
        }

        public void SaveAllStudents()
        {

            // List<Student> students = Students.ToList(); ;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonStudentsString = JsonSerializer.Serialize<Dictionary<string,Student>>(_students, options);

            if (!Directory.Exists("AppData"))
            {
                Directory.CreateDirectory("AppData");
            }

            File.WriteAllText("AppData/students.json", jsonStudentsString);

        }

        public void RemoveStudent(string id)
        {
            
            if (_students.Count >= 0)
            {
                if(_students.ContainsKey(id))
                this._students.Remove(id);
            }
        }

    }
}
