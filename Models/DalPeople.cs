using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace Models
{
    public class DalPeople
    {
        public Connection<SqlConnection> Connection { get; }

        public DalPeople(Connection<SqlConnection> connection)
        {
            Connection = connection;
        }

        public People Insert(People people)
        {
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = " INSERT INTO [People]([Name],[Created],[DateBirthday],[Active],[Salary])";
                command.CommandText += " VALUES(@Name,@Created,@DateBirthday,@Active,@Salary);SELECT SCOPE_IDENTITY()";
                LoadParams(command, people, Operation.Insert);
                Connection.Open();
                if (int.TryParse(command.ExecuteScalar().ToString(), out int id))
                {
                    people.Id = id;
                }
                Connection.Close();
            }
            return people;
        }       

        public bool Edit(People people)
        {
            bool status = false;
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = " UPDATE [People] SET [Name]=@Name,[DateBirthday]=@DateBirthday,";
                command.CommandText += " [Active]=@Active,[Salary]=@Salary WHERE [People].[Id]=@Id";                
                LoadParams(command, people, Operation.Edit);
                Connection.Open();
                status = command.ExecuteNonQuery() > 0;
                Connection.Close();
            }
            return status;
        }

        public bool Delete(int id)
        {
            bool status = false;
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM [People] WHERE [People].[Id]=@Id";
                LoadCreateIdParam(command, id);
                Connection.Open();
                status = command.ExecuteNonQuery() > 0;
                Connection.Close();
            }
            return status;
        }

        public IEnumerable<People> Get()
        {
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id],[Name],[Created],[DateBirthday],[Active],[Salary] FROM [People]";
                Connection.Open();
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new People
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Created = reader.GetDateTime(2),
                            DateBirthday = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3),
                            Active = reader.GetBoolean(4),
                            Salary = reader.GetDecimal(5)
                        };
                    }
                }
                Connection.Close();
            }
        }

        public DataTable Get(string filter, params string[] select)
        {
            DataTable dataTable = new DataTable("People");
            using (IDbCommand command = Connection.CreateCommand())
            {                
                command.CommandText = $"SELECT {"[" + string.Join("],[", select) + "]"} FROM [People] ";
                if (filter != null)
                {
                    command.CommandText += " WHERE [People].[Name] LIKE @Filter";
                    IDbDataParameter paramFilter = command.CreateParameter();
                    paramFilter.DbType = DbType.String;
                    paramFilter.ParameterName = "@Filter";
                    paramFilter.Value = $"%{filter}%";
                    command.Parameters.Add(paramFilter);
                }
                command.CommandText += " ORDER BY [People].[Name] ASC";
                Connection.Open();
                dataTable.Load(command.ExecuteReader());
                Connection.Close();
            }
            return dataTable;
        }

        public People Find(int id)
        {
            People people = null;
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = " SELECT [Id],[Name],[Created],[DateBirthday],[Active],[Salary] FROM [People]";
                command.CommandText += " WHERE [People].[Id]=@Id";
                LoadCreateIdParam(command, id);
                Connection.Open();
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        people = new People
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Created = reader.GetDateTime(2),
                            DateBirthday = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3),
                            Active = reader.GetBoolean(4),
                            Salary = reader.GetDecimal(5)
                        };
                    }
                }
                Connection.Close();
            }
            return people;
        }

        private void LoadCreateIdParam(IDbCommand command, int id)
        {
            IDbDataParameter paramId = command.CreateParameter();
            paramId.DbType = DbType.Int32;
            paramId.ParameterName = "@Id";
            paramId.SourceColumn = "Id";
            paramId.Value = id;
            command.Parameters.Add(paramId);
        }

        private void LoadParams(IDbCommand command, People people, Operation operation)
        {
            IDbDataParameter paramName = command.CreateParameter();
            paramName.DbType = DbType.String;
            paramName.ParameterName = "@Name";
            paramName.SourceColumn = "Name";
            paramName.Value = people.Name;

            if (operation == Operation.Insert)
            {
                IDbDataParameter paramCreated = command.CreateParameter();
                paramCreated.DbType = DbType.DateTime;
                paramCreated.ParameterName = "@Created";
                paramCreated.SourceColumn = "Created";
                paramCreated.Value = people.Created;
                command.Parameters.Add(paramCreated);
            }

            IDbDataParameter paramDateBirthday = command.CreateParameter();
            paramDateBirthday.DbType = DbType.DateTime;
            paramDateBirthday.ParameterName = "@DateBirthday";
            paramDateBirthday.SourceColumn = "DateBirthday";
            paramDateBirthday.Value = (object)people.DateBirthday ?? DBNull.Value;

            IDbDataParameter paramActive = command.CreateParameter();
            paramActive.DbType = DbType.Boolean;
            paramActive.ParameterName = "@Active";
            paramActive.SourceColumn = "Active";
            paramActive.Value = people.Active;

            IDbDataParameter paramSalary = command.CreateParameter();
            paramSalary.DbType = DbType.Decimal;
            paramSalary.Precision = 18;
            paramSalary.Scale = 2;
            paramSalary.ParameterName = "@Salary";
            paramSalary.SourceColumn = "Salary";
            paramSalary.Value = people.Salary;

            command.Parameters.Add(paramName);            
            command.Parameters.Add(paramDateBirthday);
            command.Parameters.Add(paramActive);
            command.Parameters.Add(paramSalary);

            if (operation == Operation.Edit)
            {
                IDbDataParameter paramId = command.CreateParameter();
                paramId.DbType = DbType.Int32;
                paramId.ParameterName = "@Id";
                paramId.SourceColumn = "Id";
                paramId.Value = people.Id;
                command.Parameters.Add(paramId);
            }
        }
    }
}
