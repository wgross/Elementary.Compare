using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Elementary.Compare.Test
{
    public class EnumerableEqualityComparerTest
    {
        [Fact]
        public void EnumerableEqualityComparer_accepts_two_empty_enumerables()
        {
            // ACT

            var result = EnumerableEqualityComparer.Default.Equals(new List<int>(), new int[0]);

            // ASSERT

            Assert.True(result);
        }

        [Fact]
        public void EnumerableEqualityComparer_accepts_bopthy_null()
        {
            // ACT

            var result = EnumerableEqualityComparer.Default.Equals(null, null);

            // ASSERT

            Assert.True(result);
        }


        [Fact]
        public void EnumerableEqualityComparer_rejects_null()
        {
            // ACT

            var result = EnumerableEqualityComparer.Default.Equals(null, new int[0]);

            // ASSERT

            Assert.False(result);
        }
    }
}
