using System.Collections.Generic;

using  imm.Configs;
using  imm.Data.Serialization;
using Match3;
using UnityEngine;

namespace imm
{
    public sealed class AppModel : Config<AppModel, JsonSerializationPolicy>
    {
        public const string FILE_NAME = "model.json";
      
       
        public int runningCounter = 0;
        public int stepCounter = 0;
        public bool needRate = true;
        public bool isEndlessMode = false;
        public int frsp = 0;
        public int adsp = 0;
        public long lsp = -1;
        public long lprivacy = -1;
        public AppsFlyerData AFdata;
        public string FirebaseToken = "";
        public int gameMode = 0;    // normal mode, endless mode
                                    //
        public int currentRoom = 0;
        public List<RoomModel> rooms;
        public bool needShowRoom = false;
        
        public AppModel() : base(FILE_NAME)
        {

        }

        
    }
}