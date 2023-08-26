using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyInventory.Pages.Inventories
{
    public class EditModel : PageModel
    {
        public InventoryInfo inventoryInfo = new InventoryInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=myinventory;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM inventories WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                inventoryInfo.id = "" + reader.GetInt32(0);
                                inventoryInfo.name = reader.GetString(1);
                                inventoryInfo.type = reader.GetString(2);
                                inventoryInfo.stock = reader.GetString(3);
                                inventoryInfo.description = reader.GetString(4);
                            }
                        }
                    }
                }


            }

            catch (Exception ex)
            {
                errorMessage = ex.Message;  
            }
        }

        public void OnPost() 
        {
            inventoryInfo.id = Request.Form["id"];
            inventoryInfo.name = Request.Form["name"];
            inventoryInfo.type = Request.Form["type"];
            inventoryInfo.stock = Request.Form["stock"];
            inventoryInfo.description = Request.Form["description"];

            if (inventoryInfo.id.Length == 0 || inventoryInfo.name.Length == 0 || 
                inventoryInfo.type.Length == 0 || inventoryInfo.stock.Length == 0 || 
                inventoryInfo.description.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            //save the new client into the database
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=myinventory;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE inventories " +
                                 "SET name=@name, type=@type, stock=@stock, description=@description " +
                                 "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", inventoryInfo.name);
                        command.Parameters.AddWithValue("@type", inventoryInfo.type);
                        command.Parameters.AddWithValue("@stock", inventoryInfo.stock);
                        command.Parameters.AddWithValue("@description", inventoryInfo.description);
                        command.Parameters.AddWithValue("@id", inventoryInfo.id);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("Index");
        }    
    }
}
