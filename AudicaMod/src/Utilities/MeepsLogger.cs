using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudicaModding.MeepsUIEnhancements
{
    class MeepsLogger
    {
        public static void Msg(string msg)
        {
            #if DEBUG
                MelonLoader.MelonLogger.Msg(msg);
            #endif
        }
    }
}
