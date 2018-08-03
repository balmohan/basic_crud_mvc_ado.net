using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentDbManagement.Models;
using System.Data.SqlClient;
using StudentDbManagement.Database;

namespace StudentDbManagement.Controllers
{
    public class StudentController : Controller
    {
        // GET: All Student
        public ActionResult Index()
        {
            var dataconn = new DatabaseConnection();
            try
            {
                dataconn.getconnection.Open();
                string students = "SELECT * FROM StudentsData";
                SqlCommand command = new SqlCommand(students, dataconn.getconnection);
                SqlDataReader reader = command.ExecuteReader();
                var studentlist = new List<Student>();
                while (reader.Read())
                {
                    studentlist.Add(
                        new Student()
                        {
                            Id = int.Parse(reader["Id"].ToString()),
                            firstName = reader[1].ToString(),
                            lastName = reader[2].ToString(),
                            address = reader[3].ToString(),
                            dateOfBirth = DateTime.Parse(reader[4].ToString())
                        }
                        );
                }
                reader.Close();
                return View(studentlist);
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
            finally
            {
                dataconn.getconnection.Close();
            }
        }

        [HttpGet]
        public ViewResult AddNewStudent()
        {
            return View(new Student());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewStudent([Bind(Include = "firstName,lastName,address,dateOfbirth")] Student student)
        {
            if (ModelState.IsValid)
            {
                var dataconn = new DatabaseConnection();
                var sqlinsert = "INSERT INTO StudentsData(firstName,lastName,address,dateOfBirth) VALUES(@firstName,@lastName,@address,@date)";
                SqlCommand command = new SqlCommand(sqlinsert, dataconn.getconnection);
                command.Parameters.AddWithValue("@firstName", student.firstName);
                command.Parameters.AddWithValue("@lastName", student.lastName);
                command.Parameters.AddWithValue("@address", student.address);
                command.Parameters.AddWithValue("@date", student.dateOfBirth);
                try
                {
                    dataconn.getconnection.Open();
                    command.ExecuteNonQuery();
                    TempData["message"] = "Record added successfully";
                }
                catch (Exception)
                {
                    TempData["message"] = "Something went wrong while adding new student";
                }
                finally
                {
                    dataconn.getconnection.Close();
                }

            }

            return View(new Student());
        }

        public ActionResult Delete(int? id)
        {
            var dataconn = new DatabaseConnection();
            string getStudent = "DELETE FROM StudentsData WHERE Id=@id";
            SqlCommand command = new SqlCommand(getStudent, dataconn.getconnection);
            command.Parameters.AddWithValue("@id", id);
            try
            {
                dataconn.getconnection.Open();
                int numberOfRecordsAffected = command.ExecuteNonQuery();
                TempData["message"] = numberOfRecordsAffected + " Record Deleted Successfully";
                return RedirectToAction("Index");
            }
            catch
            {
                return HttpNotFound();
            }
            finally
            {
                dataconn.getconnection.Close();
            }
        }

        [HttpGet]
        public ActionResult Update(int? id)
        {

            var dataconn = new DatabaseConnection();
            string getStudent = "SELECT * FROM StudentsData WHERE Id=@id";
            SqlCommand command = new SqlCommand(getStudent, dataconn.getconnection);
            command.Parameters.AddWithValue("@id", id);
                dataconn.getconnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
            var student = new Student()
            {
                Id = int.Parse(reader[0].ToString()),
                firstName = reader[1].ToString(),
                lastName = reader[2].ToString(),
                address = reader[3].ToString(),
                dateOfBirth = DateTime.Parse(reader[4].ToString())
                };
                reader.Close();
                return View(student);
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include ="Id,firstname,lastName,address,dateOfbirth")] Student student)
        {
            DatabaseConnection conn = new DatabaseConnection();
            if (ModelState.IsValid)
            {
                try
                {
                    string query = "UPDATE StudentsData SET firstName=@firstname,lastName=@lastname,address=@address,dateOfBirth=@dateOfBirth WHERE id=@id";
                    SqlCommand command = new SqlCommand(query, conn.getconnection);
                    command.Parameters.AddWithValue("@firstname", student.firstName);
                    command.Parameters.AddWithValue("@lastName", student.lastName);
                    command.Parameters.AddWithValue("@address", student.address);
                    command.Parameters.AddWithValue("@dateOfBirth", student.dateOfBirth);
                    command.Parameters.AddWithValue("@id", student.Id);
                    conn.getconnection.Open();
                    command.ExecuteNonQuery();
                    conn.getconnection.Close();
                    TempData["message"] = "Record Updated Successfully";
                    return RedirectToAction("Index");
                }

                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
                finally
                {
                    conn.getconnection.Close();
                }
            }
            return View();
        }
    }
}