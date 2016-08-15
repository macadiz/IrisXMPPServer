using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreXmppServer.Enums
{
    public enum MessageType
    {
        STREAM,
        IQ,
        MESSAGE,
        PRESENCE,
        UNKNOWN
    }
}
