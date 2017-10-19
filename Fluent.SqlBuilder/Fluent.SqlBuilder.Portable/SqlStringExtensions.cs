using System.Linq;

namespace Fluent.SqlBuilder.Portable
{
    public static class SqlStringExtensions
    {
        public static string TableNameAlias(this string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return string.Empty;
            }

            var firstChar = tableName.Length > 0 ? tableName[0] : new char();
            return firstChar.ToString().ToLower();
        }
    }
}
