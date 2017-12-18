using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApplication
{
    interface IGamingMachine
    {
        List<GamingMachine> Get(int page = 0, int skip = 10, string filter = "");
        GamingMachine Get(int gamingSerialNumber);
        Result CreateGamingMachine(GamingMachine gamingMachine);
        Result UpdateGamingMachine(GamingMachine gamingMachine);
        Result DeleteGamingMachine(GamingMachine gamingMachine);
    }

    class GamingMachine
    {
        /// <summary>
        /// unique number which will be used to identify a gaming machine
        /// </summary>
        long GamingSerialNumber { get; set; }
        /// <summary>
        /// range for this should be zero (default) to 1000 position
        /// </summary>        
        string GameName { get; set; }
        DateTime CreateDate { get; set; }
        bool IsDeleted { get; set; }
    }

    class Result
    {
        int ResultCode { get; set; } = 0;
    }
		public int MachinePosition { get; private set; } = 0;
		private string ResultMessage { get; set; } = "Success";
}