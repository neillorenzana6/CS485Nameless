using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamelessGame.Combat.Abilities;

namespace NamelessGame.Combat
{
    public class CombatBuff
    {
        public Stats StatsMod { get; set; }
        public Resistances ResistancesMod { get; set; }
        public float SpeedMod { get; set; }

        public CombatBuff(Stats statsMod, Resistances resistMod, float speedMod)
        {
            this.StatsMod = statsMod;
            this.ResistancesMod = resistMod;
            this.SpeedMod = speedMod;
        }
    }
}
