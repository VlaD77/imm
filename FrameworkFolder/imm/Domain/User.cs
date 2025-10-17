using System;
using imm.Core;

namespace imm.Domain
{
    [Serializable]
    public sealed class User:Observable
    {
        public string displayName;

        public string id;

        public int xp;

        public int level;

        public float balance;

        public float chips;

        public int nextLevelXP;

        public int avatar;
    }
}