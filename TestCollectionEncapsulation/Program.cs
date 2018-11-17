using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TestCollectionEncapsulation
{
    internal class Program
    {
        private static void Main()
        {
            using (var context = new SchoolDbContext())
            {
                foreach (var grade in context.Grades.Include("Students"))
                {
                    Console.WriteLine(grade.Name);
                    if (grade.Students != null)
                    {
                        foreach (var student in grade.Students)
                        {
                            Console.WriteLine($" {student.Name}");
                        }
                    }
                }
            }
        }
    }

    internal class Grade
    {
        // https://blog.oneunicorn.com/2016/10/28/collection-navigation-properties-and-fields-in-ef-core-1-1/
        private readonly ObservableCollection<Student> _students = new ObservableCollection<Student>();

        public Grade()
        {
            _students.CollectionChanged += (sender, args) => { Console.WriteLine("CollectionChanged"); };
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ObservableCollection<Student> Students
        {
            get { return _students; }
            // set { _students = value; }
        }
    }

    internal class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int GradeId { get; set; }

        [ForeignKey(nameof(GradeId))]
        public Grade Grade { get; set; }
    }

    internal class SchoolDbInitializer : DropCreateDatabaseAlways<SchoolDbContext>
    {
        protected override void Seed(SchoolDbContext context)
        {
            var grades = new List<Grade>
            {
                new Grade {Name = "A"},
                new Grade {Name = "B"},
                new Grade {Name = "C"},
                new Grade {Name = "D"},
                new Grade {Name = "E"}
            };

            var students = new List<Student>
            {
                new Student {Name = "Jeremy", Grade = grades[0]},
                new Student {Name = "Bill", Grade = grades[0]},
                new Student {Name = "Jeremy2", Grade = grades[1]},
                new Student {Name = "Jeremy3", Grade = grades[0]}
            };

            context.Grades.AddRange(grades);
            context.Students.AddRange(students);

            base.Seed(context);
        }
    }

    internal class SchoolDbContext : DbContext
    {
        private static readonly string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=c:\Work\EF\TestCollectionEncapsulation.mdf;Integrated Security=True;Connect Timeout=30";

        public SchoolDbContext() : base(ConnectionString)
        {
            Database.SetInitializer(new SchoolDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Grade> Grades { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
