using System.Collections.Generic;

namespace Fluent.SqlBuilder.Portable
{
    public interface ISqlWhereStatementBuilder
    {
        ISqlWhereStatementBuilder WithTableName(string tableName);

        ISqlWhereStatementBuilder WithPrimaryFilter(string key);

        ISqlWhereStatementBuilder WithSecondaryFilters(IList<string> filters);

        string Build();
    }
}