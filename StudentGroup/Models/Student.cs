using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentGroup.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Group Group { get; set; }
    }
}