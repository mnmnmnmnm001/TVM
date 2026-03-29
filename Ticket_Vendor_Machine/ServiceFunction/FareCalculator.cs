using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ticket_Vendor_Machine.ServiceFunction
{
    public class FareCalculator
    {
        
        private const int BaseStationId = 1;   
        private const decimal BaseFare = 7000; 
        private const decimal FarePerStop = 5000;
        private const decimal MaxFare = 30000; 

        public decimal Calculate(int destinationId)
        {
            int stops = destinationId - BaseStationId; 
            if (stops <= 0) return 0;

            decimal fare = BaseFare + (stops - 1) * FarePerStop;
            return Math.Min(fare, MaxFare);
        }
    }
}