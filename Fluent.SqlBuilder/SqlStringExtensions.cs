using System.Linq;

namespace Fluent.SqlBuilder
{
    public static class SqlStringExtensions
    {
        public static string TableNameAlias(this string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return string.Empty;
            }

            var firstChar = tableName.FirstOrDefault();
            return firstChar.ToString().ToLower();
        }
    }
}
