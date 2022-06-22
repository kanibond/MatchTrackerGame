using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SharpEcho.CodeChallenge.Data
{
    public class GenericRepository : IRepository
    {
        public GenericRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public void Delete<T>(long id) where T : new()
        {
            var t = new T();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute("DELETE FROM " + typeof(T).Name + " WHERE Id = @Id", new { Id = id });
            }
        }

        public T Get<T>(long id)  where T : new()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var t = new T();

                connection.Open();
                return connection.Query<T>("SELECT * FROM " + typeof(T).Name + " WHERE Id = @Id",
                    new { Id = id }).FirstOrDefault();
            }
        }

        public IEnumerable<T> Get<T>() where T : new()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var t = new T();

                connection.Open();
                return connection.Query<T>("SELECT * FROM " + typeof(T).Name);
            }
        }

        public IEnumerable<T> Query<T>(string query, object parameters) where T : new ()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var t = new T();

                connection.Open();
                return connection.Query<T>(query, parameters);
            }
        }

        public long Insert<T>(T entity) where T : new()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var properties = entity.GetType().GetProperties().Where(x => x.Name != "Id").
                    Select(x => x.Name);

                connection.Open();

                return connection.Query<long>("INSERT INTO " + entity.GetType().Name + " " +
                    "(" + string.Join(",", properties) + ") VALUES (" +
                    string.Join(",", properties.Select(x => "@" + x)) + "); " +
                    "SELECT SCOPE_IDENTITY()", entity).First();
            }
        }

        public void Update<T>(T entity) where T : new()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var properties = entity.GetType().GetProperties().Where(x => x.Name != "Id").
                    Select(x => x.Name);

                connection.Open();
                connection.Query<T>("UPDATE " + entity.GetType().Name +
                    " SET " + string.Join(",", properties.Select(x => x + " = @" + x)) +
                    " WHERE Id = @Id", entity);
            }
        }
    }
}
