
using System;
using System.Collections.Generic;
using fonenako_service;
using fonenako_service.Daos;
using fonenako_service.Exceptions;
using NUnit.Framework;

namespace finenako_service_tests
{
    [TestFixture]
    public class FilterParserTest
    {
        private FilterParser _filterParser;

        private static Dictionary<string, Type> FilterFieldTypeMap = new()
        {
            { "field1", typeof(int)},
            { "field2", typeof(double) },
            { "field3", typeof(string) },
            { "field4", typeof(Order) },
            { "field5", typeof(bool) }
        };

        [OneTimeSetUp]
        public void SetUp()
        {
            _filterParser = new FilterParser();
        }

        [Test]
        public void ParseFilter_should_throw_argumentnullexception_when_the_filter_field_type_map_is_null()
        {
            Assert.Throws<ArgumentNullException>(()=> _filterParser.ParseFilter("myField=6", null));
        }

        [TestCase("unknownField=9", typeof(UnknownFilterFieldException), TestName = "Unknown filter field name shloud throw UnknownFilterFieldException")]
        [TestCase("field1=9&field1=10", typeof(DuplicateFilterFieldException), TestName = "Filter field declared twice shloud throw DuplicateFilterFieldException")]
        [TestCase("field1=9&field2", typeof(InvalidFilterFieldValueException), TestName = "Filter field declared alone without any thing should throw InvalidFilterFieldValueException")]
        [TestCase("field2=", typeof(InvalidFilterFieldValueException), TestName = "Filter field declared with empty value throw InvalidFilterFieldValueException")]
        [TestCase("field2=wrongValue", typeof(InvalidFilterFieldValueException), TestName = "Filter field with wrong value type should throw InvalidFilterFieldValueException")]
        public void ParseFilter_should_throw_filterexception(string filterParam, Type expectedExceptionType)
        {
            Assert.Throws(Is.TypeOf(expectedExceptionType), () => _filterParser.ParseFilter(filterParam, FilterFieldTypeMap));
        }

        [Test]
        public void ParseFilter_should_throw_argumentexception_when_a_field_is_mapped_with_a_null_as_target()
        {
            Assert.Throws<ArgumentException>(() => _filterParser.ParseFilter("myField=anyValue", new Dictionary<string, Type>() { { "myField", null } }));
        }

        [Test]
        public void ParseFilter_should_parse_corretcly_the_given_param()
        {
            const string filterAsParam = "field1=1&field2=2.5&field3=My test&field4=Desc&field5=true";

            var resultMap = _filterParser.ParseFilter(filterAsParam, FilterFieldTypeMap);

            Assert.IsTrue(resultMap.TryGetValue("field1", out var value));
            Assert.AreEqual(1, value);
            Assert.IsTrue(resultMap.TryGetValue("field2", out value));
            Assert.AreEqual(2.5d, value);
            Assert.IsTrue(resultMap.TryGetValue("field3", out value));
            Assert.AreEqual("My test", value);
            Assert.IsTrue(resultMap.TryGetValue("field4", out value));
            Assert.AreEqual(Order.Desc, value);
            Assert.IsTrue(resultMap.TryGetValue("field5", out value));
            Assert.AreEqual(true, value);
        }
    }
}
