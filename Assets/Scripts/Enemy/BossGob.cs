using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using NamelessGame.Combat;
using NamelessGame.Combat.Abilities;

public class BossGob : MonoBehaviour
{
    public Collider meleeCollider;
    private Combatant combatant;
    private CombatantScript combatantScript;
    private CombatAbility[] abilities;
    private Animator anim;
    public ProjectileLauncherController fireballShooter;
    public Transform player;
    public float moveSpeed = 5.0f;
    public int maxDist = 35;
    public int minDist = 3;
    private bool phase1Flag = true;
    private bool phase2Flag = false;
    private bool ab2act = false;
    private bool phase3Flag = false;
    private bool ab3act = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        abilities = GetComponents<CombatAbility>();

        var combatantStats = GetComponent<CombatantScript>();
        combatantScript = combatantStats;
        combatant = new Combatant(combatantStats.team, combatantStats.healthPool, combatantStats.magicResist, combatantStats.physicalResist, combatantStats.attackPower, combatantStats.magicPower);

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

    void MeleeAttack()
    {
        //  anim.SetTrigger("IsMelee");
        var attack = GenerateAbilityAttack("GobBasicMelee");
        MeleeHitCheck(attack, meleeCollider);
    }

    void MagicAttack()
    {
        if (Vector3.Distance(transform.position, player.position) < 35)
        {
            //  anim.SetTrigger("IsMelee");
            var attack = GenerateAbilityAttack("GobBasicMagic");
            fireballShooter.atk = attack;
            fireballShooter.Shoot();
        }
    }

    void MagicSpreadAttack()
    {
        if (Vector3.Distance(transform.position, player.position) < 35)
        {
            //  anim.SetTrigger("IsMelee");
            var attack = GenerateAbilityAttack("MagicSpread");
            fireballShooter.atk = attack;
            fireballShooter.Shoot();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        fireballShooter.transform.LookAt(player);
        if (combatantScript.currentHealth > 350)
        {
            if (phase1Flag)
            {
                InvokeRepeating("MagicAttack", .1f, 3);
                phase1Flag = false;
            }
        }
        else if (combatantScript.currentHealth > 220)
        {

            if (!ab2act)
            {
                InvokeRepeating("MagicSpreadAttack", .1f, 8);
                ab2act = true;
            }
            else
            {
                if (!ab3act)
                {
                    InvokeRepeating("MeleeAttack", .1f, 2);
                    ab3act = true;
                }

                transform.LookAt(player);
                if (Vector3.Distance(transform.position, player.position) <= maxDist && Vector3.Distance(transform.position, player.position) >= minDist)
                {
                    anim.SetBool("isWalking", true);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    //  transform.Rotate(0, 180, 0);
                }
            }


        }
    }
}

