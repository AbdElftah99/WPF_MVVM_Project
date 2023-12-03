using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Project.Model
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class TeacherSearch
    {
        private Teacher[] teachers;

        public TeacherSearch(Teacher[] teachers)
        {
            this.teachers = teachers;
        }

        // Indexer for searching teachers by ID
        public Teacher this[int id]
        {
            get
            {
                foreach (Teacher teacher in teachers)
                {
                    if (teacher.Id == id)
                        return teacher;
                }
                return null;
            }
        }

        // Indexer for searching teachers by FirstName
        public Teacher this[string firstName]
        {
            get
            {
                foreach (Teacher teacher in teachers)
                {
                    if (teacher.FirstName == firstName)
                        return teacher;
                }
                return null;
            }
        }

        // Indexer for searching teachers by LastName
        public Teacher this[string firstName, string lastName]
        {
            get
            {
                foreach (Teacher teacher in teachers)
                {
                    if (teacher.FirstName == firstName && teacher.LastName == lastName)
                        return teacher;
                }
                return null;
            }
        }
    }
}
