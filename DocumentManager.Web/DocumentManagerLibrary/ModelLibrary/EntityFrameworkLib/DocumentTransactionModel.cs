using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagerLibrary
{
    public class DocumentTransactionModel
    {
        public long FromUser { get; set; }
        public long ToUser { get; set; }
        public long DocumentID { get; set; }
        public string DocumentName { get; set; }
        public long DocumentDetailID { get; set; }

    }
}
