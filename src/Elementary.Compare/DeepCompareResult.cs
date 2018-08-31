using System.Collections.Generic;
using System.Linq;

namespace Elementary.Compare
{
    public class DeepCompareResult
    {
        public class Differences
        {
            public bool Any() => this.Values.Any() || this.Types.Any();

            public IList<string> Values { get; } = new List<string>();

            public IList<string> Types { get; } = new List<string>();
        }

        public class Missings
        {
            public bool Any() => this.Left.Any() || this.Right.Any();

            public IList<string> Left { get; } = new List<string>();

            public IList<string> Right { get; } = new List<string>();
        }

        public bool AreEqual => !(this.Different.Any() || this.Missing.Any());

        public Differences Different { get; } = new Differences();

        public Missings Missing { get; } = new Missings();

        public IList<string> EqualValues { get; } = new List<string>();
    }
}