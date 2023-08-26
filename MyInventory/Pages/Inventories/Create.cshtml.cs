using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyInventory.Pages.Inventories
{
    public class CreateModel : PageModel
    {
        public InventoryInfo inventoryInfo = new InventoryInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            inventoryInfo.name = Request.Form["name"];
            inventoryInfo.type = Request.Form["type"];
            inventoryInfo.stock = Request.Form["name"];
            inventoryInfo.description = Request.Form["description"];

            if(inventoryInfo.name.Length == 0 || inventoryInfo.type.Length == 0 ||
                inventoryInfo.stock.Length == 0 || inventoryInfo.description.Length == 0)
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
                    String sql = "INSERT INTO inventories" +
                                 "(name, type, stock, description) VALUES" +
                                 "(@name, @type, @stock, @description)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", inventoryInfo.name);
                        command.Parameters.AddWithValue("@type", inventoryInfo.type);
                        command.Parameters.AddWithValue("@stock", inventoryInfo.stock);
                        command.Parameters.AddWithValue("@description", inventoryInfo.description);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            inventoryInfo.name = "";
            inventoryInfo.type = "";
            inventoryInfo.stock = "";
            inventoryInfo.description = "";
            successMessage = "New Client Added Correctly";

            Response.Redirect("/Inventories/index");
        }

    }
}
