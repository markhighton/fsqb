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
        private readonly IDictionary<string, string[]> _innerJoinTables = new Dictionary<string, string[]>();
        private readonly IDictionary<string, string[]> _leftJoinTables = new Dictionary<string, string[]>();

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
            _innerJoinTables.Add(innerTableName, new []{ leftKey, rightKey });
            return this;
        }

        public IFluentSqlQueryBuilder LeftJoin(string leftTableName, string leftKey, string rightKey)
        {
            _leftJoinTables.Add(leftTableName, new []{  leftKey, rightKey });
            return this;
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
                    .WithInnerJoinTables(_innerJoinTables)
                    .WithLeftJoinTables(_leftJoinTables)
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

            var sqlJoinStatment = new StringBuilder();

            var tableAlias = _primaryTableName.TableNameAlias();

            var joinIndex = 0;
            if (_innerJoinTables.Any())
            {
                
                foreach (var innerJoinTable in _innerJoinTables)
                {
                    var innerTableName = innerJoinTable.Key;
                    var innerTableAlias = innerTableName.TableNameAlias() + joinIndex;
                    var leftKey = innerJoinTable.Value[0];
                    var rightKey = innerJoinTable.Value[1];                
                    sqlJoinStatment.Append($" INNER JOIN [{innerTableName}] {innerTableAlias} ON {tableAlias}.{leftKey} = {innerTableAlias}.{rightKey}");
                    joinIndex++;
                }

            }


            var sql = new StringBuilder();
            sql.Append(sqlSelectStatement);
            sql.Append(sqlFromStatement);
            sql.Append(sqlJoinStatment);
            sql.Append(sqlWhereStatement);
            sql.Append(sqlOrderByStatement);
            return sql.ToString();
        }

    }
}