using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NamelessGame.Combat
{
    public class Combatant
    {
        public int Team { get; set; }
        public Health CombatantHealth { get; set; }
        public Resistances CombatantResistances { get; set; }
        public Stats CombatantStats { get; set; }


        public Combatant(int team, float healthPool, float magicResist, float physicalResist, float attackPower, float magicPower)
        {
            this.Team = team;

            var comHealth = new Health(healthPool);
            var comResist = new Resistances(physicalResist, magicResist);
            var comStats = new Stats(attackPower, magicPower);

            this.CombatantHealth = comHealth;
            this.CombatantResistances = comResist;
            this.CombatantStats = comStats;
        }

        public void DamageCombatant(CombatAttack atk)
        {
            float resistDamage = CombatantResistances.CalcResisted(atk);
            //  Debug.Log(resistDamage);
            CombatantHealth.HealthDamaged(resistDamage);
        }

        public void AddEffect(CombatBuff effect)
        {
            this.CombatantResistances.AddPhysicalResist(effect.ResistancesMod.PhysicalResist);
            this.CombatantResistances.AddMagicResist(effect.ResistancesMod.MagicalResist);

        }

        public void RemoveEffect(CombatBuff effect)
        {
            this.CombatantResistances.RemovePhysicalResist(effect.ResistancesMod.PhysicalResist);
            this.CombatantResistances.RemoveMagicResist(effect.ResistancesMod.MagicalResist);

        }

    }
}