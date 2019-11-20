using System;

namespace UnitTestsPOC.Domain
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsSafe { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
