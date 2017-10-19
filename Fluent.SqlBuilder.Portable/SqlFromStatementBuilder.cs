using System;

namespace Fluent.SqlBuilder.Portable
{
    public class SqlFromStatementBuilder : ISqlFromStatementBuilder
    {
        private string _tableName;
        public ISqlFromStatementBuilder WithTableName(string tableName)
        {
            _tableName = tableName;
            return this;
        }

        public string Build()
        {
            if (string.IsNullOrWhiteSpace(_tableName))
            {
                throw new ArgumentNullException(nameof(_tableName));
            }

            var tableAlias = _tableName.TableNameAlias();

            return $" FROM [{_tableName}] {tableAlias}";
        }
    }
}