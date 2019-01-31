using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentWProject.Models;

namespace StudentWProject.Models
{
    public class StudentWProjectContext : DbContext
    {
        public StudentWProjectContext (DbContextOptions<StudentWProjectContext> options)
            : base(options)
        {
        }

        public DbSet<StudentWProject.Models.Grade> Grade { get; set; }
        public DbSet<StudentWProject.Models.Student> Student { get; set; }
        public DbSet<StudentWProject.Models.Ambion> Ambion { get; set; }
    }
}
