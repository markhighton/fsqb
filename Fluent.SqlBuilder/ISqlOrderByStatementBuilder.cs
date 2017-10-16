namespace Fluent.SqlBuilder
{
    public interface ISqlOrderByStatementBuilder
    {
        ISqlOrderByStatementBuilder WithTableName(string tableName);

        ISqlOrderByStatementBuilder WithColumnName(string columnName);
        string Build(SqlOrderType type);
    }
}