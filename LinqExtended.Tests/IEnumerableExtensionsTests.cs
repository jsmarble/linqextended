using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqExtended.Tests
{
    public class IEnumerableExtensionsTests
    {
        [Fact]
        public void ContainsAtLeast_Checking_Less_Than_Actual_Returns_False()
        {
            int count = 10;
            var numbers = Enumerable.Range(1, count - 1);
            bool expected = false;
            bool actual = numbers.ContainsAtLeast(count);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void ContainsAtLeast_Checking_More_Than_Actual_Returns_True()
        {
            int count = 10;
            var numbers = Enumerable.Range(1, count + 1);
            bool expected = true;
            bool actual = numbers.ContainsAtLeast(count);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ContainsAtLeast_Checking_Exactly_Actual_Returns_True()
        {
            int count = 10;
            var numbers = Enumerable.Range(1, count);
            bool expected = true;
            bool actual = numbers.ContainsAtLeast(count);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ContainsAtMost_Checking_Less_Than_Actual_Returns_True()
        {
            int count = 10;
            var numbers = Enumerable.Range(1, count - 1);
            bool expected = true;
            bool actual = numbers.ContainsAtMost(count);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void ContainsAtMost_Checking_More_Than_Actual_Returns_False()
        {
            int count = 10;
            var numbers = Enumerable.Range(1, count + 1);
            bool expected = false;
            bool actual = numbers.ContainsAtMost(count);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ContainsAtMost_Checking_Exactly_Actual_Returns_True()
        {
            int count = 10;
            var numbers = Enumerable.Range(1, count);
            bool expected = true;
            bool actual = numbers.ContainsAtMost(count);
            Assert.Equal(expected, actual);
        }
    }
}
