using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ticket_Vendor_Machine.ServiceFunction
{
    public class FareCalculator
    {
        private const int OriginStationId = 1;
        private const decimal BaseFare = 7000;   // first 5 stops
        private const decimal FarePerStop = 5000;   // each stop after 5
        private const decimal MaxFare = 40000;

        public decimal GetUnitFare(int destinationId)
        {
            int stops = destinationId - OriginStationId;

            if (stops <= 0) return 0;

            decimal fare = BaseFare;
            if (stops > 5)
                fare += (stops - 5) * FarePerStop;

            return Math.Min(fare, MaxFare);
        }

        public decimal GetTotalFare(int destinationId, int quantity)
        {
            if (quantity < 1) quantity = 1;
            if (quantity > 9) quantity = 9;
            return GetUnitFare(destinationId) * quantity;
        }
    }
}