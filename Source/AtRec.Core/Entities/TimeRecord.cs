using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtRec.Core.Entities
{
    public class TimeRecord
    {
        public DateTime Time
        {
            get;
            set;
        }

        public RecordTrigger Trigger
        {
            get;
            set;
        }

        public CheckFlags Flags
        {
            get;
            set;
        }

        public string Memo
        {
            get;
            set;
        }
    }
}
