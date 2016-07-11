using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagerLibrary
{
    public class StatusUtil
    {
        public enum Status
        {
            Active,
            InActive
        }

        public enum RequestStatus
        {
            Pending,
            Approved,
            Declined
        }
    }
}
