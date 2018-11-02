using System.Data.SqlClient;
namespace Models
{
    public class ConnectionSqlServer : Connection<SqlConnection>
    {        
        public ConnectionSqlServer()
            :base("Server=.\\SqlExpress;Database=myDataBase;User Id=sa;Password=senha;MultipleActiveResultSets=true;")
        {
        }
    }
}
