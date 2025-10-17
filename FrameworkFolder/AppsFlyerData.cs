using System;

namespace imm
{
    [Serializable]
    public class AppsFlyerData
    {
        public string UUID = "";
        public string AFId = "";

        //Unity analytic
        public string AUUserId = "";
        public string AUCustomUserId = "";
        public string AUCustomDeviceId = "";

        public long firstOpenDate = -1;
        public long startSessionDate = -1;
        public long countOfSessions = 0;

        public bool isNewSession = false;
        

        public float totalUSDPaid = 0;

        
    }
}