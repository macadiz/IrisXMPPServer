using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrisXMPPServer.Enums
{
    public enum MessageType
    {
        STREAM,
        IQ,
        MESSAGE,
        PRESENCE,
        AUTH,
        UNKNOWN
    }
}
