using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.SqlBuilder.Portable
{
    public class SqlWhereStatementBuilder : ISqlWhereStatementBuilder
    {
        private string _primaryFilterKey;
        private string _tableName;
        private IList<string> _filters = new List<string>();

        public ISqlWhereStatementBuilder WithTableName(string tableName)
        {
            _tableName = tableName;
            return this;
        }

        public ISqlWhereStatementBuilder WithPrimaryFilter(string key)
        {
            _primaryFilterKey = key;
            return this;
        }

        public ISqlWhereStatementBuilder WithSecondaryFilters(IList<string> filters)
        {
            _filters = filters;
            return this;
        }

        public string Build()
        {
            if (string.IsNullOrEmpty(_tableName))
            {
                throw new ArgumentNullException(nameof(_tableName));
            }

            var sql = new StringBuilder();
            var alias = _tableName.TableNameAlias();

            if (!string.IsNullOrWhiteSpace(_primaryFilterKey))
            {
                var sqlWhereStatement = $" WHERE {alias}.{_primaryFilterKey} = @{_primaryFilterKey}";
                sql.Append(sqlWhereStatement);
            }

            if (_filters != null && _filters.Any())
            {
                var sqlAndStatements = $"{string.Concat(_filters.Select(f => $" AND {alias}.{f} = @{f}")) }";
                sql.Append(sqlAndStatements);
            }

            return sql.ToString();
        }
    }
}