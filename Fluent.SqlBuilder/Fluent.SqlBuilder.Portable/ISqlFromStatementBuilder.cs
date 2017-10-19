namespace Fluent.SqlBuilder.Portable
{
    public interface ISqlFromStatementBuilder
    {
        ISqlFromStatementBuilder WithTableName(string tableName);
        string Build();
    }
}