using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyInventory.Pages.Inventories
{
    public class IndexModel : PageModel
    {
        public List<InventoryInfo> listinventories = new List<InventoryInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=myinventory;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM inventories";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                InventoryInfo inventoryInfo = new InventoryInfo();
                                inventoryInfo.id = "" + reader.GetInt32(0);
                                inventoryInfo.name = reader.GetString(1);
                                inventoryInfo.type = reader.GetString(2);
                                inventoryInfo.stock = reader.GetString(3);
                                inventoryInfo.description = reader.GetString(4);
                                inventoryInfo.created_at = reader.GetDateTime(5).ToString();

                                listinventories.Add(inventoryInfo);
                            }
                        }
                    }
                }

            }
            catch(Exception ex) 
            {
                Console.WriteLine("Exception: "+ex.ToString());
            }
           
        }
    }

    public class InventoryInfo
    {
        public String id;
        public String name;
        public String type;
        public String stock;
        public String description;
        public String created_at;
    }
}
