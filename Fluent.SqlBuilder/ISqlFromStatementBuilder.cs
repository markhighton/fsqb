namespace Fluent.SqlBuilder
{
    public interface ISqlFromStatementBuilder
    {
        ISqlFromStatementBuilder WithTableName(string tableName);
        string Build();
    }
}