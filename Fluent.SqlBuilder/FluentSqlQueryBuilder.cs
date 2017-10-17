using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Fluent.SqlBuilder
{
    public class FluentSqlQueryBuilder : IFluentSqlQueryBuilder
    {
        private readonly ISqlSelectStatementBuilder _sqlSelectStatementBuilder;
        private readonly ISqlFromStatementBuilder _sqlFromStatementBuilder;
        private readonly ISqlOrderByStatementBuilder _sqlOrderByStatementBuilder;
        private readonly ISqlWhereStatementBuilder _sqlWhereStatementBuilder;

        private readonly IList<string> _filters = new List<string>();
        private SqlSelectType _selectType = SqlSelectType.All;
        private SqlOrderType _orderType = SqlOrderType.Descending;
        private string[] _selectedColumns;
        private string _selectCustomColumn;
        private string _primaryTableName;
        private string _orderByColumn;
        private string _primaryFilterKey;

        public FluentSqlQueryBuilder(
            ISqlSelectStatementBuilder sqlSelectStatementBuilder, 
            ISqlFromStatementBuilder sqlFromStatementBuilder, 
            ISqlOrderByStatementBuilder sqlOrderByStatementBuilder,
            ISqlWhereStatementBuilder sqlWhereStatementBuilder)
        {
            _sqlSelectStatementBuilder = sqlSelectStatementBuilder;
            _sqlFromStatementBuilder = sqlFromStatementBuilder;
            _sqlOrderByStatementBuilder = sqlOrderByStatementBuilder;
            _sqlWhereStatementBuilder = sqlWhereStatementBuilder;
        }


        public IFluentSqlQueryBuilder SelectAll()
        {
            _selectType = SqlSelectType.All;
            return this;
        }

        public IFluentSqlQueryBuilder SelectCount()
        {
            _selectType = SqlSelectType.Count;
            return this;
        }

        public IFluentSqlQueryBuilder SelectSpecific(params string[] columns)
        {
            _selectType = SqlSelectType.Specific;
            _selectedColumns = columns;
            return this;
        }

        public IFluentSqlQueryBuilder SelectCustom(string selectCustomColumn, params string[] columns)
        {
            _selectType = SqlSelectType.Custom;
            _selectCustomColumn = selectCustomColumn;
            _selectedColumns = columns;
            return this;
        }

        public IFluentSqlQueryBuilder InnerJoin(string innerTableName, string leftKey, string rightKey)
        {
            throw new System.NotImplementedException();
        }

        public IFluentSqlQueryBuilder LeftJoin(string leftTableName, string leftKey, string rightKey)
        {
            throw new System.NotImplementedException();
        }

        public IFluentSqlQueryBuilder From(string primaryTableName)
        {
            _primaryTableName = primaryTableName;
            return this;
        }

        public IFluentSqlQueryBuilder Where(string key)
        {
            _primaryFilterKey = key;
            return this;
        }

        public IFluentSqlQueryBuilder And(string key)
        {
            _filters.Add(key);
            return this;
        }

        public IFluentSqlQueryBuilder OrderBy(string columnName)
        {
            _orderType = SqlOrderType.Ascending;
            _orderByColumn = columnName;
            return this;
        }

        public IFluentSqlQueryBuilder OrderByDescending(string columnName)
        {
            _orderType = SqlOrderType.Descending;
            _orderByColumn = columnName;
            return this;
        }

        public string Build()
        {
            if (string.IsNullOrWhiteSpace(_primaryTableName))
            {
                throw new ArgumentNullException(nameof(_primaryTableName));
            }

            var sqlSelectStatement =
                _sqlSelectStatementBuilder
                    .WithTableName(_primaryTableName)
                    .WithCustomColumn(_selectCustomColumn)
                    .WithSelectedColumns(_selectedColumns)
                    .Build(_selectType);

            var sqlFromStatement =
                _sqlFromStatementBuilder
                    .WithTableName(_primaryTableName)
                    .Build();

            var sqlOrderByStatement =
              _sqlOrderByStatementBuilder
                  .WithTableName(_primaryTableName)
                  .WithColumnName(_orderByColumn)
                  .Build(_orderType);

            var sqlWhereStatement =
                _sqlWhereStatementBuilder
                .WithTableName(_primaryTableName)
                .WithPrimaryFilter(_primaryFilterKey)
                .WithSecondaryFilters(_filters)
                .Build();

            var sql = new StringBuilder();
            sql.Append(sqlSelectStatement);
            sql.Append(sqlFromStatement);
            sql.Append(sqlWhereStatement);
            sql.Append(sqlOrderByStatement);
            return sql.ToString();
        }

    }
}