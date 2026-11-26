




using System;
using System.Data;
using System.Data.SqlClient;

namespace LabAssignment6
{
    internal class Program
    {
        static string cs = @"Data Source=.\SQLEXPRESS;
                            Initial Catalog=COLLEGE;
                            Integrated Security=True;
                            TrustServerCertificate=True";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n===== LAB ASSIGNMENT 6 =====");
                Console.WriteLine("1. Display Students (SqlDataReader)");
                Console.WriteLine("2. Insert Student (Connected)");
                Console.WriteLine("3. Update Student (Connected)");
                Console.WriteLine("4. Delete Student (Connected)");
                Console.WriteLine("5. Display Students (DataSet)");
                Console.WriteLine("6. Insert Student (DataSet)");
                Console.WriteLine("7. Update Student (DataSet)");
                Console.WriteLine("8. Delete Student (DataSet)");
                Console.WriteLine("9. CRUD using SqlCommandBuilder");
                Console.WriteLine("10. Complete CRUD - Both Architectures");
                Console.WriteLine("0. Exit");

                Console.Write("\nEnter your choice: ");
                int choice = int.Parse(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1: Q1(); break;
                        case 2: Q2(); break;
                        case 3: Q3(); break;
                        case 4: Q4(); break;
                        case 5: Q5(); break;
                        case 6: Q6(); break;
                        case 7: Q7(); break;
                        case 8: Q8(); break;
                        case 9: Q9(); break;
                        case 10: Q10(); break;
                        case 0: return;
                        default:
                            Console.WriteLine("Invalid choice!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        // Q1: Display all records using SqlDataReader

        static void Q1()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Student";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                Console.WriteLine("\nStudent Records:");

                while (dr.Read())
                {
                    Console.WriteLine(
                        dr["StudentID"] + " " +
                        dr["Name"] + " " +
                        dr["Age"] + " " +
                        dr["Course"] + " " +
                        dr["City"]);
                }

                dr.Close();
            }
        }

        // Q2: Insert a student using Connected Architecture

        static void Q2()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                Console.Write("Enter Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Age: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Enter Course: ");
                string course = Console.ReadLine();

                Console.Write("Enter City: ");
                string city = Console.ReadLine();

                string query = @"INSERT INTO Student
                (Name, Age, Course, City)
                VALUES (@Name, @Age, @Course, @City)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Course", course);
                cmd.Parameters.AddWithValue("@City", city);

                con.Open();

                int result = cmd.ExecuteNonQuery();

                Console.WriteLine(result + " Student Inserted Successfully!");
            }
        }

        // Q3: Update student using Connected Architecture

        static void Q3()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                Console.Write("Enter Student ID: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter New Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter New Age: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Enter New Course: ");
                string course = Console.ReadLine();

                Console.Write("Enter New City: ");
                string city = Console.ReadLine();

                string query = @"UPDATE Student SET
                Name=@Name, Age=@Age,
                Course=@Course, City=@City
                WHERE StudentID=@ID";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Course", course);
                cmd.Parameters.AddWithValue("@City", city);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();

                int result = cmd.ExecuteNonQuery();

                Console.WriteLine(result > 0 ?
                    "Student Updated Successfully!" :
                    "Student ID Not Found!");
            }
        }

        // Q4: Delete student using Connected Architecture

        static void Q4()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                Console.Write("Enter Student ID to Delete: ");
                int id = int.Parse(Console.ReadLine());

                string query =
                    "DELETE FROM Student WHERE StudentID=@ID";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();

                int result = cmd.ExecuteNonQuery();

                Console.WriteLine(result > 0 ?
                    "Student Deleted Successfully!" :
                    "Student ID Not Found!");
            }
        }

        // Q5: Display records using DataSet and SqlDataAdapter

        static void Q5()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Student";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataSet ds = new DataSet();

                da.Fill(ds, "Student");

                Console.WriteLine("\nStudent Records:");

                foreach (DataRow row in ds.Tables["Student"].Rows)
                {
                    Console.WriteLine(
                        row["StudentID"] + " " +
                        row["Name"] + " " +
                        row["Age"] + " " +
                        row["Course"] + " " +
                        row["City"]);
                }
            }
        }

        // Q6: Insert using DataSet and SqlDataAdapter

        static void Q6()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Student", con);

                DataSet ds = new DataSet();

                da.Fill(ds, "Student");

                Console.Write("Enter Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Age: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Enter Course: ");
                string course = Console.ReadLine();

                Console.Write("Enter City: ");
                string city = Console.ReadLine();

                DataRow row = ds.Tables["Student"].NewRow();

                row["Name"] = name;
                row["Age"] = age;
                row["Course"] = course;
                row["City"] = city;

                ds.Tables["Student"].Rows.Add(row);

                da.InsertCommand = new SqlCommand(
                    @"INSERT INTO Student
                    (Name, Age, Course, City)
                    VALUES (@Name, @Age, @Course, @City)", con);

                da.InsertCommand.Parameters.Add(
                    "@Name", SqlDbType.VarChar, 50, "Name");

                da.InsertCommand.Parameters.Add(
                    "@Age", SqlDbType.Int, 0, "Age");

                da.InsertCommand.Parameters.Add(
                    "@Course", SqlDbType.VarChar, 50, "Course");

                da.InsertCommand.Parameters.Add(
                    "@City", SqlDbType.VarChar, 50, "City");

                da.Update(ds, "Student");

                Console.WriteLine("Student Inserted Successfully!");
            }
        }

        // Q7: Update using DataSet and SqlDataAdapter

        static void Q7()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Student", con);

                DataSet ds = new DataSet();

                da.Fill(ds, "Student");

                DataTable dt = ds.Tables["Student"];

                dt.PrimaryKey = new DataColumn[]
                {
                    dt.Columns["StudentID"]
                };

                Console.Write("Enter Student ID: ");
                int id = int.Parse(Console.ReadLine());

                DataRow row = dt.Rows.Find(id);

                if (row == null)
                {
                    Console.WriteLine("Student Not Found!");
                    return;
                }

                Console.Write("Enter New Name: ");
                row["Name"] = Console.ReadLine();

                Console.Write("Enter New Age: ");
                row["Age"] = int.Parse(Console.ReadLine());

                Console.Write("Enter New Course: ");
                row["Course"] = Console.ReadLine();

                Console.Write("Enter New City: ");
                row["City"] = Console.ReadLine();

                da.UpdateCommand = new SqlCommand(
                    @"UPDATE Student SET
                    Name=@Name, Age=@Age,
                    Course=@Course, City=@City
                    WHERE StudentID=@ID", con);

                da.UpdateCommand.Parameters.Add(
                    "@Name", SqlDbType.VarChar, 50, "Name");

                da.UpdateCommand.Parameters.Add(
                    "@Age", SqlDbType.Int, 0, "Age");

                da.UpdateCommand.Parameters.Add(
                    "@Course", SqlDbType.VarChar, 50, "Course");

                da.UpdateCommand.Parameters.Add(
                    "@City", SqlDbType.VarChar, 50, "City");

                SqlParameter p = da.UpdateCommand.Parameters.Add(
                    "@ID", SqlDbType.Int, 0, "StudentID");

                p.SourceVersion = DataRowVersion.Original;

                da.Update(ds, "Student");

                Console.WriteLine("Student Updated Successfully!");
            }
        }

        // Q8: Delete using DataSet and SqlDataAdapter

        static void Q8()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Student", con);

                DataSet ds = new DataSet();

                da.Fill(ds, "Student");

                DataTable dt = ds.Tables["Student"];

                dt.PrimaryKey = new DataColumn[]
                {
                    dt.Columns["StudentID"]
                };

                Console.Write("Enter Student ID to Delete: ");
                int id = int.Parse(Console.ReadLine());

                DataRow row = dt.Rows.Find(id);

                if (row == null)
                {
                    Console.WriteLine("Student Not Found!");
                    return;
                }

                row.Delete();

                da.DeleteCommand = new SqlCommand(
                    "DELETE FROM Student WHERE StudentID=@ID", con);

                SqlParameter p = da.DeleteCommand.Parameters.Add(
                    "@ID", SqlDbType.Int, 0, "StudentID");

                p.SourceVersion = DataRowVersion.Original;

                da.Update(ds, "Student");

                Console.WriteLine("Student Deleted Successfully!");
            }
        }

        // Q9: CRUD using SqlCommandBuilder

        static void Q9()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Student", con);

                DataSet ds = new DataSet();

                da.Fill(ds, "Student");

                SqlCommandBuilder builder =
                    new SqlCommandBuilder(da);

                DataTable dt = ds.Tables["Student"];

                dt.PrimaryKey = new DataColumn[]
                {
                    dt.Columns["StudentID"]
                };

                // INSERT

                Console.WriteLine("\n--- INSERT STUDENT ---");

                DataRow newRow = dt.NewRow();

                Console.Write("Enter Name: ");
                newRow["Name"] = Console.ReadLine();

                Console.Write("Enter Age: ");
                newRow["Age"] = int.Parse(Console.ReadLine());

                Console.Write("Enter Course: ");
                newRow["Course"] = Console.ReadLine();

                Console.Write("Enter City: ");
                newRow["City"] = Console.ReadLine();

                dt.Rows.Add(newRow);

                // UPDATE

                Console.WriteLine("\n--- UPDATE STUDENT ---");

                Console.Write("Enter Student ID to Update: ");
                int updateId = int.Parse(Console.ReadLine());

                DataRow updateRow = dt.Rows.Find(updateId);

                if (updateRow == null)
                {
                    throw new Exception("Update Student ID Not Found!");
                }

                Console.Write("Enter New Name: ");
                updateRow["Name"] = Console.ReadLine();

                Console.Write("Enter New Age: ");
                updateRow["Age"] = int.Parse(Console.ReadLine());

                Console.Write("Enter New Course: ");
                updateRow["Course"] = Console.ReadLine();

                Console.Write("Enter New City: ");
                updateRow["City"] = Console.ReadLine();

                // DELETE

                Console.WriteLine("\n--- DELETE STUDENT ---");

                Console.Write("Enter Student ID to Delete: ");
                int deleteId = int.Parse(Console.ReadLine());

                DataRow deleteRow = dt.Rows.Find(deleteId);

                if (deleteRow == null)
                {
                    throw new Exception("Delete Student ID Not Found!");
                }

                deleteRow.Delete();

                // Save all changes to SQL Server

                da.Update(ds, "Student");

                Console.WriteLine("\nCRUD Operations Completed Successfully!");
            }
        }

        // Q10: Complete CRUD using both architectures

        static void Q10()
        {
            Console.WriteLine("\nCONNECTED ARCHITECTURE");
            Console.WriteLine("1. Display");
            Q1();

            Console.WriteLine("\n2. Insert");
            Q2();

            Console.WriteLine("\n3. Update");
            Q3();

            Console.WriteLine("\n4. Delete");
            Q4();

            Console.WriteLine("\nDISCONNECTED ARCHITECTURE");
            Console.WriteLine("5. Display");
            Q5();

            Console.WriteLine("\n6. Insert");
            Q6();

            Console.WriteLine("\n7. Update");
            Q7();

            Console.WriteLine("\n8. Delete");
            Q8();

            Console.WriteLine("\nBoth Architectures Demonstrated Successfully!");
        }
    }
}
