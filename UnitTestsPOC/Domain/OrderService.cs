using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UnitTestsPOC.Domain
{
    public class OrderService
    {
        public bool ProcessOrders(IEnumerable<Order> orders)
        {
            if (orders == null || orders.Count() < 20)
                throw new Exception("Not enough orders.", new ArgumentNullException(nameof(orders)));

            if (orders.GroupBy(x => x.Id).Count() != orders.Count())
                throw new Exception("Duplicated orders.");

            foreach (var order in orders)
            {
                ProcessWcf(new
                {
                    client = order.Customer.FirstName + order.Customer.Lastname,
                    amount = order.Assets.Sum(x => x.Amount),
                    country = order.Customer.CustomerAddress.Country
                });
            }


            return true;
        }


        private void ProcessWcf(object p)
        {
        }

        internal void QuickProcessing()
        {
            Thread.Sleep(120);
        }
    }
}
