using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ADO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ADO.Controllers
{
    
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }
        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            List<Inventory> inventoryList = new List<Inventory>();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "ReadInventory";
                SqlCommand command = new SqlCommand(sql, connection);
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Inventory inventory = new Inventory();
                        inventory.Id = Convert.ToInt32(dataReader["Id"]);
                        inventory.Name = Convert.ToString(dataReader["Name"]);
                        inventory.Price = Convert.ToDecimal(dataReader["Price"]);
                        inventory.Quantity = Convert.ToInt32(dataReader["Quantity"]);
                        inventory.AddedOn = Convert.ToDateTime(dataReader["AddedOn"]);

                        inventoryList.Add(inventory);
                    }
                }

                connection.Close();
            }
            return View(inventoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Inventory inventory)
        {

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "CreateInventory";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                        command.CommandType = CommandType.StoredProcedure;

                        //adding parameters

                    SqlParameter parameter = new SqlParameter
                    {
                        ParameterName = "@Name",
                        Value = inventory.Name,
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter
                    {
                        ParameterName = "@Price",
                        Value = inventory.Price,
                        SqlDbType = SqlDbType.Money
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter
                    {
                        ParameterName = "@Quantity",
                        Value = inventory.Quantity,
                        SqlDbType = SqlDbType.Int
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter
                    {
                        ParameterName = "@Result",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();
                        ViewData["success"] = "Success";

                    }
                    catch (SqlException ex)
                    {
                        ViewBag.Result = "Operation got error:" + ex.Message;
                    }

                    connection.Close();
                }
            }

            return RedirectToAction("Index");
        }


        public IActionResult Update(int Id)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            Inventory inventory = new Inventory();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "InventoryByID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.CommandType = CommandType.StoredProcedure;

                //adding parameters

                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@Id",
                    Value = Id,
                    SqlDbType = SqlDbType.Int,
                };
                command.Parameters.Add(parameter);

                connection.Open();

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        inventory.Id = Convert.ToInt32(dataReader["Id"]);
                        inventory.Name = Convert.ToString(dataReader["Name"]);
                        inventory.Price = Convert.ToDecimal(dataReader["Price"]);
                        inventory.Quantity = Convert.ToInt32(dataReader["Quantity"]);
                        inventory.AddedOn = Convert.ToDateTime(dataReader["AddedOn"]);
                    }
                }

                connection.Close();
            }
            return View(inventory);
        }

        [HttpPost]
        public IActionResult Update(Inventory inventory, int id)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UpdateInventory";
                
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    //adding parameters
                    SqlParameter parameter = new SqlParameter
                    {
                        ParameterName = "@Id",
                        Value = inventory.Id,
                        SqlDbType = SqlDbType.Int,
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter
                    {
                        ParameterName = "@Name",
                        Value = inventory.Name,
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter
                    {
                        ParameterName = "@Price",
                        Value = inventory.Price,
                        SqlDbType = SqlDbType.Money
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter
                    {
                        ParameterName = "@Quantity",
                        Value = inventory.Quantity,
                        SqlDbType = SqlDbType.Int
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter
                    {
                        ParameterName = "@Result",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();
                        ViewBag.Result = "Success";
                    }
                    catch (SqlException ex)
                    {
                        ViewBag.Result = "Operation got error:" + ex.Message;
                    }

                    connection.Close();

                }

                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "DeleteInventory";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    //adding parameters
                    
                    SqlParameter parameter = new SqlParameter
                    {
                        ParameterName = "@Id",
                        Value = id,
                        SqlDbType = SqlDbType.Int,
                    };
                    command.Parameters.Add(parameter);

                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        ViewBag.Result = "Operation got error:" + ex.Message;
                    }

                    connection.Close();
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult TransferMoney()
        {
            return View();
        }

            [HttpPost]
        public IActionResult TransferMoney(bool throwEx)
        {
            string result = "";

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var cmdRemove = new SqlCommand($"Update Account Set Money=Money-100 Where Name='Putin'", connection);
                var cmdAdd = new SqlCommand($"Update Account Set Money=Money+100 Where Name='Trump'", connection);

                connection.Open();

                // We will get this from the connection object.
                SqlTransaction tx = null;
                try
                {
                    tx = connection.BeginTransaction();

                    // Enlist the commands into this transaction.
                    cmdRemove.Transaction = tx;
                    cmdAdd.Transaction = tx;

                    // Execute the commands.
                    cmdRemove.ExecuteNonQuery();
                    cmdAdd.ExecuteNonQuery();

                    // Simulate error.
                    if (throwEx)
                    {
                        throw new Exception("Sorry! Database error! Transaction failed");
                    }

                    // Commit it!
                    tx.Commit();

                    result = "Success";
                }
                catch (Exception ex)
                {
                    // Any error will roll back transaction. Using the new conditional access operator to check for null.
                    tx?.Rollback();
                    result = ex.Message;
                }
                finally
                {
                    connection.Close();
                }
            }

            return View((object)result);
        }

    }
}
