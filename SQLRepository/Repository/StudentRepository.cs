﻿using Common.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLRepository.Repository;

namespace SQLRepository.Repository
{
    public class StudentRepository : IStudentRepository
    {
        //private  string[] Student = new[]
        //{
        //    "Harish","Naveen","Saketh"
        //};
        //public string[] getStudentNamesAll()
        //{
        //   return Student;
        //}

       public  IDbConnection _connection;
        public StudentRepository(IDbConnection connection) {
            _connection = connection;   
        }

        public async Task<List<Student>> getnames()
        {
            List<Student> students = new List<Student>();
            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Opening Connection----->", ex.Message);
                return students;
            }

            string query = "SELECT * FROM students123";

            SqlCommand cmd = new SqlCommand(query, (SqlConnection)_connection);

            try
            {
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Student student = new Student()
                        {
                            student_id = Convert.ToInt32(reader["student_id"]),
                            sname = Convert.ToString(reader["sname"]),
                            semail = Convert.ToString(reader["semail"])
                        };
                        students.Add(student);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Reading the Data----->", ex.Message);
                return students;
            }
            finally
            {
                _connection.Close();
            }
            return students;
        }
        public async Task<bool> addNewStd(Student student)
        {
            //OPENING DB CONNECTION
            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Opening DB Connection----->", ex.Message);
                return false;
            }
            //PREPARE QUERY & EXECUTE QUERY
            string insertQuery = "INSERT INTO students123 (student_id, sname, semail) VALUES (@value1, @value2, @value3)";
            SqlCommand cmd = new SqlCommand(insertQuery, (SqlConnection)_connection);
            try
            {
                cmd.Parameters.AddWithValue("@value1", student.student_id);
                cmd.Parameters.AddWithValue("@value2", student.sname);
                cmd.Parameters.AddWithValue("@value3", student.semail);
                await cmd.ExecuteNonQueryAsync();
                Console.WriteLine("Student details are  inserted!!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Inserting Student ----->", ex.Message);
                return false;
            }
            finally
            {
                _connection.Close();
            }
            return false;
        }
        public async Task<bool> updateStd(int id, Student student)
        {
            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Opening DB Connection----->", ex.Message);
                return false;
            }

            //PREAPARE COMMAND
            string updateQuery = "UPDATE students123 SET sname=@newName, semail=@newEmail WHERE student_id = @student_id";
            SqlCommand cmd = new SqlCommand(updateQuery, (SqlConnection)_connection);
            try
            {
                cmd.Parameters.AddWithValue("@newName", student.sname);
                cmd.Parameters.AddWithValue("@newEmail", student.semail);
                cmd.Parameters.AddWithValue("@student_id", student.student_id);
                await cmd.ExecuteNonQueryAsync();
                Console.WriteLine("Student Details are updated!!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Updateing Record----->", ex.Message);
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                _connection.Close();
            }
            return false;
        }


    }
}
