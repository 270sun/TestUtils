using System;
using System.Collections.Generic;
using Xunit;

namespace TestUtils.Test
{
    public class TestUtilsTest
    {
        private enum SomeEnum { A, B, C }
        private class Person { public string Name { get; set; } public int Age { get; set; } }

        [Fact]
        public void demo()
        {
            var randomString = TestUtils.RandomString(100);
            
            var randomInt1 = TestUtils.RandomInt();
            var randomInt2 = TestUtils.RandomInt(100);
            var randomInt3 = TestUtils.RandomInt(-2, 2);
            
            var randomLong1 = TestUtils.RandomLong();
            var randomLong2 = TestUtils.RandomLong(100);
            var randomLong3 = TestUtils.RandomLong(-2, 2);
            
            var randomDate = TestUtils.RandomDate();
            
            var randomDouble1 = TestUtils.RandomDouble();
            var randomDouble2 = TestUtils.RandomDouble(2, 100);
            
            var randomEnum = TestUtils.RandomEnum<SomeEnum>();    
            
            var person = TestUtils.RandomObject<Person>();
            var personList = TestUtils.RandomList<Person>(2); Assert.Equal(2, personList.Count);
            var person2 = TestUtils.RandomObject<Person>().Build(t => t.Age = 1).Build(t => t.Name = "Tom");
            var personList2 = TestUtils.RandomList<Person>().BuildAll(t => t.Age = 1);
            
            var range1 = TestUtils.Range(10);
            var range2 = TestUtils.Range(2, 8);
            var range3 = TestUtils.Range(1, 31, t => new DateTime(2020, 1, t));
            TestUtils.Range(10, t => Console.WriteLine(t));
            TestUtils.Range(2, 8, t => Console.WriteLine(t));
            
            var list1 = 1.AsList();
            var list2 = TestUtils.RandomInt().AsList();
        }
    }
}