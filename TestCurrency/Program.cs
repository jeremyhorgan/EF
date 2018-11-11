using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;

namespace TestCurrency
{
    internal class Program
    {
        private static void Main()
        {
            using (var context = new SchoolDbContext())
            {
                context.Database.Log = Console.Write;

                var chemistryCourse = context.Courses.FirstOrDefault(c => c.Title == "Chemistry");
                Debug.Assert(chemistryCourse != null, nameof(chemistryCourse) + " != null");

                chemistryCourse.Credits = 4;
                context.Entry(chemistryCourse).OriginalValues["RowVersion"] = chemistryCourse.RowVersion;

                var result = context.SaveChanges();

                Console.WriteLine(result);
            }

            using (var context = new SchoolDbContext())
            {
                context.Database.Log = Console.Write;

                var chemistryCourse = context.Courses.FirstOrDefault(c => c.Title == "Chemistry");
                Debug.Assert(chemistryCourse != null, nameof(chemistryCourse) + " != null");

                Console.WriteLine($"{chemistryCourse.Title} Credits: {chemistryCourse.Credits}");
            }
        }
    }

    internal class SchoolDbContext : DbContext
    {
        private static readonly string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Work\GitHub\EF\TestCurrency.mdf;Integrated Security=True;Connect Timeout=30";

        public SchoolDbContext() : base(ConnectionString)
        {
            // Database.SetInitializer(new SchoolDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
    }

    internal class SchoolDbInitializer : DropCreateDatabaseAlways<SchoolDbContext>
    {
        protected override void Seed(SchoolDbContext context)
        {
            var departments = new List<Department>
            {
                new Department {Name = "Engineering"},
                new Department {Name = "English"},
                new Department {Name = "Economics"},
                new Department {Name = "Mathematics"}
            };
            var courses = new List<Course>
            {
                new Course {Title = "Chemistry", Credits = 4, Department = departments[0]},
                new Course {Title = "Physics", Credits = 4, Department = departments[0]},
                new Course {Title = "Calculus", Credits = 4, Department = departments[3]},
                new Course {Title = "Poetry", Credits = 2, Department = departments[1]},
                new Course {Title = "Composition", Credits = 3, Department = departments[1]},
                new Course {Title = "Literature", Credits = 4, Department = departments[1]},
                new Course {Title = "Microeconomics", Credits = 3, Department = departments[2]},
                new Course {Title = "Macroeconomics", Credits = 3, Department = departments[2]},
                new Course {Title = "Quantitative", Credits = 2, Department = departments[3]},
                new Course {Title = "Trigonometry", Credits = 4, Department = departments[3]}
            };

            context.Departments.AddRange(departments);
            context.Courses.AddRange(courses);

            base.Seed(context);
        }
    }

    internal class Department
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
        public ICollection<Course> Courses { get; set; }
    }

    internal class Course
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
    }
}
