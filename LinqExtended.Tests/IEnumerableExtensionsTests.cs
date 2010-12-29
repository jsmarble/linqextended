using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;

namespace System.Linq.Extended.Tests
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

        [Fact]
        public void ForEach_Invokes_Action_For_Each_Item()
        {
            int count = 10;
            int actCount = 0;
            Action<int> act = (x) => actCount++;
            var numbers = Enumerable.Range(1, count);
            numbers.ForEach(act);
            Assert.Equal(count, actCount);
        }

        [Fact]
        public void Batch_Count_Is_Equal_To_Batch_Multiple_Rounded_Up()
        {
            int batchSize = 10;
            double batchMultiple = 2.1;
            int totalSize = Convert.ToInt32(batchSize * batchMultiple);
            var guids = GetGuids(totalSize);
            var batches = guids.Batch(batchSize);
            Assert.Equal(batchSize * Math.Ceiling(batchMultiple) / batchSize, Convert.ToDouble(batches.Count()));
        }

        [Fact]
        public void Batch_Returns_Smaller_Size_For_Last_Batch()
        {
            int batchSize = 10;
            int totalSize = Convert.ToInt32(batchSize * 2.5);
            var guids = GetGuids(totalSize);
            var batches = guids.Batch(batchSize);
            Assert.Equal(batchSize, batches.First().Count());
            Assert.Equal(5, batches.Last().Count());
        }

        [Fact]
        public void Batch_Does_Not_Return_Empty_Last_Batch_When_Total_Size_A_Multiple_Of_Batch_Size()
        {
            int batchSize = 10;
            int totalSize = Convert.ToInt32(batchSize * 3);
            var guids = GetGuids(totalSize);
            var batches = guids.Batch(batchSize).ToList();
            Assert.Equal(batchSize, batches.Last().Count());
        }

        [Fact]
        public void Batch_Does_Not_Enumerate_Past_First_Batch()
        {
            int takeCount = 5;
            int batchSize = 10;
            int totalSize = Convert.ToInt32(batchSize * 3);
            var guids = GetGuidsThrowExceptionAfterCount(totalSize, batchSize);
            var batches = guids.Batch(batchSize);
            var result = batches.First().Take(takeCount); //Enumerate only the first [takeCount] items in the first batch.
            Assert.Equal(takeCount, result.Count());
        }

        [Fact]
        public void Batch_All_Items_Flattened_From_Batches_Equal_Total_Size()
        {
            int batchSize = 10;
            int totalSize = Convert.ToInt32(batchSize * 3);
            var guids = GetGuids(totalSize);
            var batches = guids.Batch(batchSize);
            var flattenedGuids = batches.SelectMany(x => x);
            Assert.Equal(totalSize, flattenedGuids.Count());
        }

        [Fact]
        public void Shuffle_Is_Random()
        {
            var numbers = Enumerable.Range(1, 1000).ToList();
            var shuffled = numbers.Shuffle().ToList();
            int countSharingIndex = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] == shuffled[i])
                    countSharingIndex++;
            }
            Assert.True(countSharingIndex < numbers.Count * 0.005); //Check that less than 0.5% of numbers ended up at the same index.
        }

        [Fact]
        public void Shuffle_Changes_Every_Time()
        {
            var numbers = Enumerable.Range(1, 1000).ToList();
            List<int> shuffle1 = numbers.Shuffle().ToList();
            List<int> shuffle2 = numbers.Shuffle().ToList();
            int countSharingIndex = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                if (shuffle1[i] == shuffle2[i])
                    countSharingIndex++;
            }
            Assert.True(countSharingIndex < numbers.Count * 0.005); //Check that less than 0.5% of numbers ended up at the same index.
        }

        public IEnumerable<string> GetGuids(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return Guid.NewGuid().ToString();
            }
        }

        public IEnumerable<string> GetGuidsThrowExceptionAfterCount(int count, int exceptionCount)
        {
            for (int i = 0; i < count; i++)
            {
                if (i >= exceptionCount && exceptionCount > 0)
                    throw new InvalidOperationException();

                yield return Guid.NewGuid().ToString();
            }
        }
    }
}
