using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Elementary.Compare.Test
{
    public class NoPropertyHasDefaultValueTest
    {
        [Fact]
        public void NoPropertyHasDefaultValue_finds_null_reference()
        {
            // ARRANGE 

            var obj = new
            {
                str = (string)null
            };

            // ACT

            var result = obj.NoPropertyHasDefaultValue();

            // ASSERT

            Assert.False(result);
        }

        [Fact]
        public void NoPropertyHasDefaultValue_finds_null_value()
        {
            // ARRANGE 

            var obj = new
            {
                i = default(int)
            };

            // ACT

            var result = obj.NoPropertyHasDefaultValue();

            // ASSERT

            Assert.False(result);
        }
    }
}
