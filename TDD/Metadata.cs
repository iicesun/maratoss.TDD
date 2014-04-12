using System;

namespace TDD
{
    public static class FilterHelper
    {
        public static Between<DateTime?> Today
        {
            get { return new Between<DateTime?>(DateTime.Today, DateTime.Today); }
        }
    }
    public class Between<T>
    {
        public Between(DateTime from, DateTime to)
        {
        }
    }
    public class DepartmentOrderLiteDto
    {
        public DateTime StateDate { get; set; }
        public long Rn { get; set; }
    }
    public class DepartmentOrderFilter
    {
        public long Rn { get; set; }
        public Between<DateTime?> StateDate { get; set; }
    }
    public class DepartmentOrder
    {
        public DepartmentOrder()
        {
        }

        public DepartmentOrder(long rn)
            : this()
        {
            Rn = rn;
        }

        public long Rn { get; set; }
        public DateTime StateDate { get; set; }
    }
}
