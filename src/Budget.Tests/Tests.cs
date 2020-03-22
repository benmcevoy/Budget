using Budget.Westpac;
using NUnit.Framework;

namespace Budget.Tests
{
    public class Tests
    {
        [Test]
        public void Can_create_westpac_statement()
        {
            // arrange
            var sut = new StatementProvider(new Configuration { StatementsPath = @"D:\Dev\git\Budget\data\statements" });

            // act
            var result = sut.Statement();

            // assert
            Assert.True(result.Transactions.Count > 0);
        }

        [Test]
        public void Normalized_narrative_strings_are_normal()
        {
            // arrange
            var sut = "\"EFTPOS DEBIT 1234556 MV Chi Pty Ltd Seddon        07/12\"";

            // act
            var result = sut.NormalizeString();

            // assert
            Assert.AreEqual("eftpos debit 1234556 mv chi pty ltd seddon 07 12", result);
        }

        [Test]
        public void Classifier_can_classify()
        {
            // arrange       
            var sut = Classifier.Contains("Coles", "FOODWORKS", "Happy Apple")("Food");

            // act
            var result = sut(@"EFTPOS DEBIT 1234556 HAPPY APPLE SEDDON \ SEDDON          16 / 01");

            // assert
            Assert.AreEqual("Food", result);
        }
    }
}