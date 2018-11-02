using System;
using System.Data;
namespace Models
{
    public abstract class Connection<T>: IDisposable
        where T: IDbConnection, new()
    {
        protected IDbConnection Connect { get; private set; }
        protected string ConnectionString { get; private set; }

        public Connection(string connectionString)
        {
            ConnectionString = connectionString;
            Connect = Activator.CreateInstance<T>();
            Connect.ConnectionString = connectionString;
        }

        public IDbConnection Open()
        {
            if (Connect?.State == ConnectionState.Closed)
            {
                Connect.Open();
            }
            return Connect;
        }

        public IDbCommand CreateCommand()
        {
            return Connect.CreateCommand();
        }

        public void Close()
        {           
            if (Connect?.State == ConnectionState.Open)
            {
                Connect?.Close();
            }            
        }
        public void Dispose()
        {
            Connect?.Close();
            Connect?.Dispose();
            Connect = null;
            GC.SuppressFinalize(this);
        }
    }
}
