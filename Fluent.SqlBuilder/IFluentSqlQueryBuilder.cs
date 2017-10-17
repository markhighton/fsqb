namespace Fluent.SqlBuilder
{
    public interface IFluentSqlQueryBuilder
    {
        IFluentSqlQueryBuilder SelectAll();
        IFluentSqlQueryBuilder SelectCount();
        IFluentSqlQueryBuilder SelectSpecific(params string[] columns);
        IFluentSqlQueryBuilder SelectCustom(string customColumn, params string[] columns);
        IFluentSqlQueryBuilder InnerJoin(string innerTableName, string leftKey, string rightKey);
        IFluentSqlQueryBuilder LeftJoin(string leftTableName, string leftKey, string rightKey);
        IFluentSqlQueryBuilder From(string tableName);
        IFluentSqlQueryBuilder Where(string key);
        IFluentSqlQueryBuilder And(string key);
        IFluentSqlQueryBuilder OrderBy(string columnName);
        IFluentSqlQueryBuilder OrderByDescending(string columnName);
        string Build();
    }
}