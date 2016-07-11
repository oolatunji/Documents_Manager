using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagerLibrary
{
    public class DocumentModel
    {
        public long Catalogue { get; set; }
        public long Location { get; set; }
        public long Uploader { get; set; }
        public long CurrentUser { get; set; }
        public long Date { get; set; }
        public string Name { get; set; }
        public string RawDocument { get; set; }
        public Int32 DocumentID { get; set; }
    }
}
