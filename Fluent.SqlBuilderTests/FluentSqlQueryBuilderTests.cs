using System;
using Fluent.SqlBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
                new SqlOrderByStatementBuilder());
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
             .Where("TestCol", 1)
             .Build();

            Assert.AreEqual("SELECT t.* FROM [TestTable] t WHERE t.TestCol = 1", sqlQuery);
        }


        [TestMethod]
        public void Build_SelectAllColumns_WithMultipleFilters_ItsShouldBuildAValidSqlQuery()
        {
            var sqlQuery = _builder
             .SelectAll()
             .From("TestTable")
             .Where("TestCol", 1)
             .And("TestCol2", 2)
             .Build();

            Assert.AreEqual("SELECT t.* FROM [TestTable] t WHERE t.TestCol = 1 AND t.TestCol2 = 2", sqlQuery);
        }
    }
}
