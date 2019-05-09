using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamelessGame.Combat.Abilities;
using UnityEngine;

namespace NamelessGame.Combat
{
    public class DurrationEffect : MonoBehaviour
    {
        public CombatBuff Effect;
        public float Durration { get; set; }
        public float RemainingTime { get; set; }
        public bool Running { get; private set; }
        public bool Ended { get; set; }

        public DurrationEffect(CombatBuff effect, float durration, float remainingTime)
        {
            this.Effect = effect;
            this.Durration = durration;
            this.RemainingTime = remainingTime;
            this.Running = true;
            this.Ended = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Running)
            {
                this.Tick(Time.deltaTime);

                if (this.RemainingTime >= 0f)
                {
                    End();
                }
            }         
        }

        public void StartRefresh()
        {
            this.RemainingTime = Durration;
        }

        private void End()
        {
            this.Ended = true;
        }

        private float Tick(float tick)
        {
            if (this.RemainingTime > 0.0f)
                this.RemainingTime -= tick;
            else
                this.RemainingTime = 0.0f;

            return this.RemainingTime;
        }
    }
}
