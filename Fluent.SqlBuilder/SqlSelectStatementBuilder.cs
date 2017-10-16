using System;
using System.Linq;

namespace Fluent.SqlBuilder
{
    public class SqlSelectStatementBuilder : ISqlSelectStatementBuilder
    {
        private string _tableName;
        private string _customColumn;
        private string[] _tableColumns;

        public ISqlSelectStatementBuilder WithTableName(string tableName)
        {
            _tableName = tableName;
            return this;
        }

        public ISqlSelectStatementBuilder WithCustomColumn(string customColumn)
        {
            _customColumn = customColumn;
            return this;
        }

        public ISqlSelectStatementBuilder WithSelectedColumns(string[] columns)
        {
            _tableColumns = columns;
            return this;
        }

        public string Build(SqlSelectType type)
        {
            if (string.IsNullOrWhiteSpace(_tableName))
            {
                throw new ArgumentNullException(nameof(_tableName));
            }

            var alias = _tableName.TableNameAlias();

            switch (type)
            {
                case SqlSelectType.All:
                    return $"SELECT {alias}.*";
                case SqlSelectType.Count:
                    return "SELECT COUNT(*)";
                case SqlSelectType.Specific:
                    return BuildSpecifcSelectStatment(alias);
                case SqlSelectType.Custom:
                    return BuildCustomSelectStatment(alias);
                default:
                    return $"SELECT {alias}.*";

            }
        }

        private string BuildCustomSelectStatment(string alias)
        {
            if (_customColumn == null)
            {
                throw new ArgumentNullException(nameof(_customColumn));
            }

            if (_tableColumns == null || !_tableColumns.Any())
            {
                return $"SELECT {_customColumn}, {alias}.*";
            }

            var columnsWithAliasTag = _tableColumns.Select(column => $"{alias}.{column}");
            return $"SELECT {_customColumn}, {string.Join(",", columnsWithAliasTag)}";
        }

        private string BuildSpecifcSelectStatment(string alias)
        {
            if (_tableColumns == null || !_tableColumns.Any())
            {
                throw new ArgumentNullException(nameof(_tableColumns));
            }

            var columnsWithAliasTag = _tableColumns.Select(column => $"{alias}.{column}");
            return $"SELECT {string.Join(",", columnsWithAliasTag)}";
        }
    }
}