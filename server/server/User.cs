using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    class User
    {
        public int id;
        DateTime start;

        public User(int id, DateTime time)
        {
            this.id = id;
            this.start = time;
        }

        public string parking_price(DateTime end, int tariff)
        {
            int price;
            var diff = end - start;
            int count_min = Convert.ToInt32(diff.TotalMinutes);

            price = count_min * tariff;
            return (price.ToString());
        }
    }
}
