using System.Collections.Generic;
using System.Linq;

namespace Elementary.Compare
{
    public class DeepCompareResult
    {
        public bool AreEqual => !(this.DifferentValues.Any() || this.DifferentTypes.Any() || this.LeftLeavesMissedAtRightSide.Any() || this.RightLeavesMissedAtLeftSide.Any());

        public IList<string> DifferentValues { get; } = new List<string>();

        public IList<string> DifferentTypes { get; } = new List<string>();

        public IList<string> LeftLeavesMissedAtRightSide { get; } = new List<string>();

        public IList<string> RightLeavesMissedAtLeftSide { get; } = new List<string>();

        public IList<string> EqualValues { get; } = new List<string>();
    }
}