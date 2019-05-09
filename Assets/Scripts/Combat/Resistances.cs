using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NamelessGame.Combat
{
    public class Resistances
    {
        public float PhysicalResist { get; set; }
        public float MagicalResist { get; set; }

        public Resistances(float physicalResist, float magicalResist)
        {
            SetPhysicalResist(physicalResist);
            SetMagicalResist(magicalResist);
        }

        public void SetPhysicalResist(float physcialResist)
        {
            this.PhysicalResist = physcialResist;
        }

        public void SetMagicalResist(float magicalResist)
        {
            this.MagicalResist = magicalResist;
        }

        public void AddPhysicalResist(float addPhyResist)
        {
            float newResist = this.PhysicalResist + addPhyResist;
            if (newResist < 0)
                newResist = 0;
            this.PhysicalResist = newResist;
        }

        public void AddMagicResist(float addMagResist)
        {
            float newResist = this.MagicalResist + addMagResist;
            if (newResist < 0)
                newResist = 0;
            this.MagicalResist = newResist;
        }

        public void RemovePhysicalResist(float removePhyResist)
        {
            float newResist = this.PhysicalResist - removePhyResist;
            if (newResist < 0)
                newResist = 0;
            this.PhysicalResist = newResist;
        }

        public void RemoveMagicResist(float removeMagResist)
        {
            float newResist = this.MagicalResist - removeMagResist;
            if (newResist < 0)
                newResist = 0;
            this.MagicalResist = newResist;
        }

        private float CalcResistAmmount(float baseDamage, float resistance)
        {
            if (resistance >= 100)
                return 0;
            if (resistance == 0)
                return baseDamage;

            float resistedPercent = resistance * 1/100;
            float postResistDamage = baseDamage * resistedPercent;

            return postResistDamage;
        }

        public float CalcResisted(CombatAttack attack)
        {
            switch (attack.DamageType)
            {
                case DamageType.Magic:
                    return CalcResistAmmount(attack.AttackDamage, this.MagicalResist);
                case DamageType.Physical:
                    return CalcResistAmmount(attack.AttackDamage, this.PhysicalResist);
                default:
                    return 0;
            }
        }
    }
}
