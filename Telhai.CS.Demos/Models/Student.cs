using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Telhai.CS.Demos.Models
{
    public class Student
    {
        public string Name { get; set; }
        public string Id { get; set; }
        //public int Age { get ; set; }
        private int age =-1;
        public BitmapImage StudentImage { get; set; }
        public string Faculty { get; set; }
        public int Age
        {
            get { return age; }
            set {
                if (value > 18)
                {
                    age = value;
                }

            }
        }
       
        public override string ToString()
        {
            return  this.Name ;
        }


        public Student():this("",-1)
        {

        }

        public Student(string name,int age)
        {
           
            this.Name = name;
            this.Age = age;
            this.Id = Guid.NewGuid().ToString();
            this.StudentImage = new BitmapImage(new Uri("\\img\\KoalaAI.png", UriKind.Relative));
        }



    }
}
