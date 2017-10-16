namespace Fluent.SqlBuilder
{
    public interface ISqlSelectStatementBuilder
    {

        ISqlSelectStatementBuilder WithTableName(string tableName);

        ISqlSelectStatementBuilder WithCustomColumn(string customColumn);

        ISqlSelectStatementBuilder WithSelectedColumns(string[] columns);

        string Build(SqlSelectType type);
    }
}