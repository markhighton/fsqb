using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.SqlBuilder
{
    public class SqlSelectStatementBuilder : ISqlSelectStatementBuilder
    {
        private string _tableName;
        private string _customColumn;
        private string[] _tableColumns;
        private IDictionary<string, string[]> _innerJoinTables = new Dictionary<string, string[]>();
        private IDictionary<string, string[]> _leftJoinTables = new Dictionary<string, string[]>();

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
                    return BuildSelectAllStatement(alias);
                case SqlSelectType.Count:
                    return "SELECT COUNT(*)";
                case SqlSelectType.Specific:
                    return BuildSpecifcSelectStatment(alias);
                case SqlSelectType.Custom:
                    return BuildCustomSelectStatment(alias);
                default:
                    return BuildSelectAllStatement(alias);

            }
        }

        private string BuildSelectAllStatement(string alias)
        {
            var joiningTables = _innerJoinTables.Any() || _leftJoinTables.Any() ? ", " + BuildSelectJoinColumns() : string.Empty;
            return $"SELECT {alias}.*" + joiningTables;
        }

        private string BuildSelectJoinColumns()
        {
            var joinIndex = 0;

            var tableAliases = new List<string>();
            foreach (var innerJoinTable in _innerJoinTables)
            {
                var innerJoinAlias = innerJoinTable.Key.TableNameAlias();
                var aliasSelect = $"{innerJoinAlias}{joinIndex}.*";
                tableAliases.Add(aliasSelect);
                joinIndex++;
            }

            foreach (var leftJoinTable in _leftJoinTables)
            {
                var leftJoinAlias = leftJoinTable.Key.TableNameAlias();
                var aliasSelect = $"{leftJoinAlias}{joinIndex}.*";
                tableAliases.Add(aliasSelect);
                joinIndex++;
            }

            return string.Join(", ", tableAliases);
        }

        public ISqlSelectStatementBuilder WithInnerJoinTables(IDictionary<string, string[]> innerJoinTables)
        {
            _innerJoinTables = innerJoinTables;
            return this;
        }

        public ISqlSelectStatementBuilder WithLeftJoinTables(IDictionary<string, string[]> leftJoinTables)
        {
            _leftJoinTables = leftJoinTables;
            return this;
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