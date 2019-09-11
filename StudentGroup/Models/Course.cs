using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentGroup.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Group> Groups { get; set; }

        public Course()
        {
            Groups = new List<Group>();
        }
    }
}