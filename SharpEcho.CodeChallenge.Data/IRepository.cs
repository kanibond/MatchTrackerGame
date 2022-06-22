using System.Collections.Generic;

namespace SharpEcho.CodeChallenge.Data
{
    public interface IRepository
    {
        T Get<T>(long id) where T : new();
        IEnumerable<T> Get<T>() where T : new();
        IEnumerable<T> Query<T>(string query, object parameters) where T : new();

        long Insert<T>(T entity) where T : new();
        void Update<T>(T entity) where T : new();

        void Delete<T>(long id) where T : new();
    }
}
