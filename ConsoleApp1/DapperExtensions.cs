using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class DapperExtensions
    {
        public static IEnumerable<T> Query<T>(this IDbConnection connection, 
            QueryObject queryObject,
            IDbTransaction transaction = null, 
            bool buffer = true, 
            int? timeout = null, 
            CommandType? type = null)
        {
            return connection.Query<T>(queryObject.Sql,
                queryObject.QueryParametrs,
                transaction, buffer, timeout, type);
        }

        public static int Execute(this IDbConnection connection,
             QueryObject queryObject,
            IDbTransaction transaction = null, 
            int? timeout = null,
            CommandType? type = null)
        {
            return connection.Execute(queryObject.Sql,
              queryObject.QueryParametrs,
              transaction, timeout, type);
        }

        public static IEnumerable<T>Query<T>(this IDbConnection connection, QueryObject queryObject)
        {
            return connection.Query<T>(queryObject.Sql, queryObject.QueryParametrs);
        }
    }

    public class QueryObject
    {
        public QueryObject(string Sql, object QueryParametrs)
        {
            this.Sql = Sql;
            this.QueryParametrs = QueryParametrs;
        }
        public string Sql { get; private set; }
        public object QueryParametrs { get; private set; }
    }
}
