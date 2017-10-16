using System;

namespace Fluent.SqlBuilder
{
    public class SqlOrderByStatementBuilder : ISqlOrderByStatementBuilder
    {
        private string _tableName;
        private string _columnName;
        public ISqlOrderByStatementBuilder WithTableName(string tableName)
        {
            _tableName = tableName;
            return this;
        }

        public ISqlOrderByStatementBuilder WithColumnName(string columnName)
        {
            _columnName = columnName;
            return this;
        }

        public string Build(SqlOrderType type)
        {
            if (string.IsNullOrWhiteSpace(_tableName))
            {
                throw new ArgumentNullException(nameof(_tableName));
            }

            if (string.IsNullOrWhiteSpace(_columnName))
            {
                return string.Empty;
            }

            var tableAlias = _tableName.TableNameAlias();

            switch (type)
            {
                case SqlOrderType.Ascending:
                    return $" ORDER BY {tableAlias}.{_columnName}";
                case SqlOrderType.Descending:
                    return $" ORDER BY {tableAlias}.{_columnName} DESC";
                default:
                    return string.Empty;
            }

        }
    }
}