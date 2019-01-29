using Elementary.Hierarchy;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Elementary.Compare
{
    public class DiffResult
    {
        public class Differences : IEnumerable<string>
        {
            public bool Any() => this.Values.Any() || this.Types.Any();

            public IEnumerator<string> GetEnumerator() => this.Values.Union(this.Types).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            public IList<string> Values { get; } = new List<string>();

            public IList<string> Types { get; } = new List<string>();
        }

        public class Missings : IEnumerable<string>
        {
            public IEnumerator<string> GetEnumerator() => this.Right.Union(this.Left).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            public IList<string> Left { get; } = new List<string>();

            public IList<string> Right { get; } = new List<string>();
        }

        public bool AreEqual => !(this.Different.Any() || this.Missing.Any());

        public Differences Different { get; } = new Differences();

        public Missings Missing { get; } = new Missings();

        public IList<string> EqualValues { get; } = new List<string>();

        public DiffResult Exclude(HierarchyPath<string> propertyPath)
        {
            var propertyPathStr = propertyPath.ToString();

            this.Different.Values
                .Where(d => d.StartsWith(propertyPathStr))
                .ToList()
                .ForEach(d => this.Different.Values.Remove(d));

            this.Different.Types
                .Where(d => d.StartsWith(propertyPathStr))
                .ToList()
                .ForEach(d => this.Different.Values.Remove(d));

            return this;
        }
    }
}