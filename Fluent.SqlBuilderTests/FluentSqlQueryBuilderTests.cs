using System;
using Fluent.SqlBuilder;
using Fluent.SqlBuilder.Portable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fluent.SqlBuilderTests
{
    [TestClass]
    public class FluentSqlQueryBuilderTests
    {
        private IFluentSqlQueryBuilder _builder;

        [TestInitialize]
        public void Init()
        {
            _builder = new FluentSqlQueryBuilder(
                new SqlSelectStatementBuilder(), 
                new SqlFromStatementBuilder(), 
                new SqlOrderByStatementBuilder(),
                new SqlWhereStatementBuilder());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Build_ItShouldThrowExceptionWithNoPrimaryTable()
        {
               _builder
                .SelectAll()
                .Build();
        }

        [TestMethod]
        public void Build_SelectAllQuery_ItShouldBuildAValidSqlQuery()
        {
            var sqlQuery =  _builder
                .SelectAll()
                .From("TestTable")
                .Build();

            Assert.AreEqual("SELECT t.* FROM [TestTable] t", sqlQuery);

        }



        [TestMethod]
        public void Build_SelectAllOrderedAscendingQuery_ItShouldBuildAValidSqlQuery()
        {
            var sqlQuery = _builder
                .SelectAll()
                .From("TestTable")
                .OrderBy("TestColAsc")
                .Build();

            Assert.AreEqual("SELECT t.* FROM [TestTable] t ORDER BY t.TestColAsc", sqlQuery);

        }

        [TestMethod]
        public void Build_SelectAllOrderedDescendingQuery_ItShouldBuildAValidSqlQuery()
        {
            var sqlQuery = _builder
                .SelectAll()
                .From("TestTable")
                .OrderByDescending("TestColDesc")
                .Build();

            Assert.AreEqual("SELECT t.* FROM [TestTable] t ORDER BY t.TestColDesc DESC", sqlQuery);

        }

        [TestMethod]
        public void Build_SelectCountQuery_ItsShouldBuildAValidSqlQuery()
        {
            var sqlQuery = _builder
             .SelectCount()
             .From("TestTable")
             .Build();

            Assert.AreEqual("SELECT COUNT(*) FROM [TestTable] t", sqlQuery);
        }


        [TestMethod]
        public void Build_SelectSpecificColumnsQuery_ItsShouldBuildAValidSqlQuery()
        {
            var sqlQuery = _builder
             .SelectSpecific("TestCol1", "TestCol2")
             .From("TestTable")
             .Build();

            Assert.AreEqual("SELECT t.TestCol1,t.TestCol2 FROM [TestTable] t", sqlQuery);
        }

        [TestMethod]
        public void Build_SelectCustomColumnsQuery_ItsShouldBuildAValidSqlQuery()
        {
            var sqlQuery = _builder
             .SelectCustom("(SELECT COUNT(*) FROM TestTableTwo) AS TestTableTwoCount")
             .From("TestTable")
             .Build();

            Assert.AreEqual("SELECT (SELECT COUNT(*) FROM TestTableTwo) AS TestTableTwoCount, t.* FROM [TestTable] t", sqlQuery);
        }


        [TestMethod]
        public void Build_SelectCustomColumnAndSpecificColumnsQuery_ItsShouldBuildAValidSqlQuery()
        {
            var sqlQuery = _builder
             .SelectCustom("(SELECT COUNT(*) FROM TestTableTwo) AS TestTableTwoCount", "TestCol1", "TestCol2")
             .From("TestTable")
             .Build();

            Assert.AreEqual("SELECT (SELECT COUNT(*) FROM TestTableTwo) AS TestTableTwoCount, t.TestCol1,t.TestCol2 FROM [TestTable] t", sqlQuery);
        }


        [TestMethod]
        public void Build_SelectAllColumns_WithWhereFilter_ItsShouldBuildAValidSqlQuery()
        {
            var sqlQuery = _builder
             .SelectAll()
             .From("TestTable")
             .Where("TestCol")
             .Build();

            Assert.AreEqual("SELECT t.* FROM [TestTable] t WHERE t.TestCol = @TestCol", sqlQuery);
        }


        [TestMethod]
        public void Build_SelectAllColumns_WithMultipleFilters_ItsShouldBuildAValidSqlQuery()
        {
            var sqlQuery = _builder
             .SelectAll()
             .From("TestTable")
             .Where("TestCol")
             .And("TestCol2")
             .Build();

            Assert.AreEqual("SELECT t.* FROM [TestTable] t WHERE t.TestCol = @TestCol AND t.TestCol2 = @TestCol2", sqlQuery);
        }

        [TestMethod]
        public void Build_SelectAllColumnsFromASingleTableUsingInnerJoin_ItShouldBuildAValidSqlQuery()
        {
            var sqlQuery = _builder
                .SelectAll()
                .From("TestTable")
                .InnerJoin("TestTableInner", "TestTableKey1", "TestTableInnerKey2")
                .Build();

            Assert.AreEqual("SELECT t.*, t0.* FROM [TestTable] t INNER JOIN [TestTableInner] t0 ON t.TestTableKey1 = t0.TestTableInnerKey2",  sqlQuery);
        }

        [TestMethod]
        public void Build_SelectAllColumnsFromMultipleTablesUsingInnerJoin_ItShouldBuildAValidSqlQuery()
        {
            var sqlQuery = _builder
                .SelectAll()
                .From("TestTable")
                .InnerJoin("TestTableInner", "TestTableKey1", "TestTableInnerKey2")
                .InnerJoin("TestTableInner2", "TestTableKey1", "TestTableInner2Key1")
                .Build();

            Assert.AreEqual("SELECT t.*, t0.*, t1.* FROM [TestTable] t INNER JOIN [TestTableInner] t0 ON t.TestTableKey1 = t0.TestTableInnerKey2 INNER JOIN [TestTableInner2] t1 ON t.TestTableKey1 = t1.TestTableInner2Key1", sqlQuery);
        }
    }
}
