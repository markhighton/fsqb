using System.Collections.Generic;

namespace Fluent.SqlBuilder.Portable
{
    public interface ISqlSelectStatementBuilder
    {

        ISqlSelectStatementBuilder WithTableName(string tableName);

        ISqlSelectStatementBuilder WithCustomColumn(string customColumn);

        ISqlSelectStatementBuilder WithSelectedColumns(string[] columns);

        string Build(SqlSelectType type);
        ISqlSelectStatementBuilder WithInnerJoinTables(IDictionary<string, string[]> innerJoinTables);
        ISqlSelectStatementBuilder WithLeftJoinTables(IDictionary<string, string[]> innerJoinTables);
    }
}