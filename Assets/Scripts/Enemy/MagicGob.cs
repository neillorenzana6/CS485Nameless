using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using NamelessGame.Combat;
using NamelessGame.Combat.Abilities;

public class MagicGob : MonoBehaviour
{
    private Combatant combatant;
    private CombatAbility[] abilities;
    private Animator anim;
    public ProjectileLauncherController fireballShooter;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        abilities = GetComponents<CombatAbility>();

        var combatantStats = GetComponent<CombatantScript>();
        combatant = new Combatant(combatantStats.team, combatantStats.healthPool, combatantStats.magicResist, combatantStats.physicalResist, combatantStats.attackPower, combatantStats.magicPower);

        InvokeRepeating("MagicAttack", .1f, 2.5f);
    }

    void MeleeHitCheck(CombatAttack atk, Collider hitbox)
    {
        var hitCheck = hitbox.GetComponent<MeleeHitbox>();
        var cols = hitCheck.GetColliders();
        List<CombatantScript> multiHitCheck = new List<CombatantScript>();
        foreach (var col in cols)
        {
            try
            {
                var enemy = col.GetComponent<CombatantScript>();

                if (enemy.team != combatant.Team)
                {
                    if (!multiHitCheck.Contains(enemy))
                    {
                        enemy.DamageCombatant(atk);
                        multiHitCheck.Add(enemy);
                    }
                }

            }
            catch (Exception ex) { }
        }
    }

    private CombatAttack GenerateAbilityAttack(string abilityName)
    {
        var combatAttack = new CombatAttack(0, DamageType.Physical);

        var cb = abilities.FirstOrDefault(x => x.AbilityRefName == abilityName);
        if (cb != null)
        {
            float dmg = cb.damageMag * cb.damageMod;
            combatAttack.SetAttackDamage(dmg);
            combatAttack.SetDamageType(cb.damageType);
        }
        else
            throw (new AbilityNotFoundException(abilityName, ""));

        return combatAttack;
    }

    void MagicAttack()
    {
        if (Vector3.Distance(transform.position, player.position) < 25)
        {
            //  anim.SetTrigger("IsMelee");
            var attack = GenerateAbilityAttack("GobBasicMagic");
            fireballShooter.atk = attack;
            fireballShooter.Shoot();
        }
    }

    // Update is called once per frame
    void Update()
    {
        fireballShooter.transform.LookAt(player);
        //fireballShooter.transform.Rotate(0, 180, 0);
    }
}
