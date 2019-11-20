using System;
using System.ComponentModel.DataAnnotations;

namespace UnitTestsPOC.Domain
{
    public class Customer
    {
        public Customer()
        {
            throw new Exception("This constructor shoudn't be invoked!");
        }

        public Customer(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        [StringLength(12, MinimumLength = 6)]
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsVip { get; set; }


        [RegularExpression(@"\d{2}-\d{3}")]
        public string PostalCodePolish { get; set; }

        [RegularExpression(@"^[A-CEGHJ-PR-TW-Za-ceghj-pr-tw-z]{1}[A-NP-Za-np-z]{1}[0-9]{6}[A-Da-d]{0,1}$")]
        public string NinoNumber { get; set; }

        public Address CustomerAddress { get; set; }

        /// <summary>
        /// Circular reference
        /// </summary>
        public Order CustomerOrder { get; set; }
    }
}
