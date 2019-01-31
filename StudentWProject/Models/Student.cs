using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentWProject.Models
{
    public class Student
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        [ForeignKey("Grade")]
        public int GradeRefId { get; set; }
        public Grade Grade { get; set; }

        [ForeignKey("Ambion")]
        public int AmbionRefId { get; set; }
        public Ambion Ambion { get; set; }
    }
}
