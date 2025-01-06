using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using MBAW.TaskManagement.Infrastructure.Model;

namespace MBAW.TaskManagement.Infrastructure.Helpers
{
    public static class SqlDataReaderHelper
    {
        public static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            var schemaTable = reader.GetSchemaTable();
            return schemaTable != null && schemaTable.Rows.Cast<DataRow>().Any(row => row["ColumnName"].ToString() == columnName);
        }

        public static List<T> MapReaderToObjects<T>(this SqlDataReader reader) where T : class, IBaseModel, new()
        {
            var result = new List<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            while (reader.Read())
            {
                var obj = new T();

                foreach (var property in properties)
                {
                    if (reader.HasColumn(property.Name) && !reader.IsDBNull(reader.GetOrdinal(property.Name)))
                    {
                        property.SetValue(obj, reader[property.Name]);
                    }
                }

                result.Add(obj);
            }

            return result;
        }

    }
}
