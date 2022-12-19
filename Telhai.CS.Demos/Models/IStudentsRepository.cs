using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telhai.CS.Demos.Models
{
    public interface IStudentsRepository
    {
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void RemoveStudent(string id);
        string LoadAllStudents();
         void SaveAllStudents();
        Student[] Students { get;  }

    }
}
