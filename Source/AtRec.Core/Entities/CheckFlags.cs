using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtRec.Core.Entities
{
    public enum CheckFlags
    {
        None      = 0b0000,

        Checkin   = 0b0001,
        
        Checkout  = 0b0010,

        Break     = 0b0100,

        Reinstatement
                  = 0b1000,
    }
}
