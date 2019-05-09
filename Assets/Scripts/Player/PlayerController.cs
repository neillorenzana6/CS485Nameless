using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using NamelessGame.Combat;
using NamelessGame.Combat.Abilities;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public AudioSource swordSwing;
    public AudioSource fireBall;
    public AudioSource MagicUp;
    public AudioSource WarCry;
    public AudioSource FireBall3;
    public AudioSource FastSlashes;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Collider meleeCollider;
    public Collider spellCleaveCollider;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    public CombatAbility[] abilities;
    private Animator anim;

    public Collider playerCollider;

    private Combatant combatant;
    private CombatantScript combatantScript;
    public ProjectileLauncherController fireballShooter;

    private float magicBoost = 1f;
    private float phyBoost = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        abilities = GetComponents<CombatAbility>();
        //blSwipe.Cooldown = globalCD;

        anim = this.gameObject.GetComponent<Animator>();

        var combatantStats = GetComponent<CombatantScript>();
        combatantScript = combatantStats;
        combatant = new Combatant(combatantStats.team, combatantStats.healthPool, combatantStats.magicResist, combatantStats.physicalResist, combatantStats.attackPower, combatantStats.magicPower);
        fireballShooter.combatantOwner = combatantStats;
        InvokeRepeating("PassiveHeal", .1f, 1);
    }

    void SetupPlayerGlobalCooldowns()
    {
        var globalCD = new Cooldown(10, false);
        var globalCD2 = new Cooldown(15, false);
        var globalCD3 = new Cooldown(20, false);

        foreach (var ability in abilities.Where(x => x.SharedCooldownPool == 1))
        {
            ability.Cooldown = globalCD;
        }
        foreach (var ability in abilities.Where(x => x.SharedCooldownPool == 2))
        {
            ability.Cooldown = globalCD2;
        }
        foreach (var ability in abilities.Where(x => x.SharedCooldownPool == 3))
        {
            ability.Cooldown = globalCD3;
        }
    }

    private bool ExecuteAbility(string abilityName, string description)
    {
        bool offCD = false;
        var cb = abilities.FirstOrDefault(x => x.AbilityRefName == abilityName);
        if (cb != null)
        {
            offCD = cb.Cooldown.Ready();
            cb.abilitySignaled = true;
        }
        else
            throw (new AbilityNotFoundException(abilityName, description));

        return offCD;
    }

    private bool ExecuteCombatAbility(string abilityName, string description)
    {
        bool offCD = false;
        var cb = abilities.FirstOrDefault(x => x.AbilityRefName == abilityName);
        if (cb != null)
        {
            offCD = cb.Cooldown.Ready();
            cb.abilitySignaled = true;
        }
        else
            throw (new AbilityNotFoundException(abilityName, description));

        return offCD;
    }

    private CombatAttack GenerateAbilityAttack(string abilityName)
    {
        var combatAttack = new CombatAttack(0, DamageType.Physical);

        var cb = abilities.FirstOrDefault(x => x.AbilityRefName == abilityName);
        if (cb != null)
        {
            float dmg = cb.damageMag * cb.damageMod;
            if (combatAttack.DamageType == DamageType.Magic)
                dmg = dmg * magicBoost;
            if (combatAttack.DamageType == DamageType.Physical)
                dmg = dmg * phyBoost;
            combatAttack.SetAttackDamage(dmg);
            combatAttack.SetDamageType(cb.damageType);
        }
        else
            throw (new AbilityNotFoundException(abilityName, ""));

        return combatAttack;
    }

    void MeleeHitCheck(CombatAttack atk, Collider hitbox)
    {
        var hitCheck = hitbox.GetComponent<MeleeHitbox>();
        var cols = hitCheck.GetColliders();

        foreach (var col in cols)
        {
            try
            {
                var enemy = col.GetComponent<CombatantScript>();
                if (enemy.team != combatant.Team)
                    enemy.DamageCombatant(atk);
            }
            catch (Exception ex) { }
        }
    }

    void PassiveHeal()
    {
        combatantScript.HealCombatant(3);
    }

    void CallBasicMelee()
    {
        var attack = GenerateAbilityAttack("BasicMelee");
        MeleeHitCheck(attack, meleeCollider);
    }

    void BuffMagic()
    {
        MagicUp.Play();
        this.magicBoost = 1.5f;
        this.speed = this.speed - 2.0f;
        this.jumpSpeed = this.jumpSpeed + 6.0f;
    }

    void RemoveBuffMagic()
    {
        this.magicBoost = 1f;
        this.speed = this.speed + 2.0f;
        this.jumpSpeed = this.jumpSpeed - 6.0f;
    }

    void BuffMelee()
    {
        this.phyBoost = 1.5f;
        this.speed = this.speed + 5.0f;
        WarCry.Play();
    }

    void RemoveBuffMelee()
    {
        this.phyBoost = 1f;
        this.speed = this.speed - 5.0f;
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            bool willExecute = ExecuteCombatAbility("BasicMelee", "Player Primary Attack");
            if (willExecute)
            {
                anim.SetTrigger("IsMelee");
                swordSwing.Play();
                var attack = GenerateAbilityAttack("BasicMelee");
                Debug.Log("atk = " + attack.AttackDamage);
                MeleeHitCheck(attack, meleeCollider);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            bool willExecute = ExecuteCombatAbility("BasicMagic", "Player Primary Attack");
            if (willExecute)
            {
                anim.SetTrigger("IsMagic");
                var attack = GenerateAbilityAttack("BasicMagic");
                fireballShooter.atk = attack;
                fireballShooter.Shoot();
                fireBall.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bool willExecute = ExecuteCombatAbility("ComboMagic", "Player Primary Attack");
            if (willExecute)
            {
                anim.SetTrigger("IsMagic");
                var attack = GenerateAbilityAttack("BasicMagic");
                fireballShooter.atk = attack;
                fireballShooter.ShootBurst();
                FireBall3.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bool willExecute = ExecuteCombatAbility("ComboMelee", "Player Primary Attack");
            if (willExecute)
            {
                //anim.SetTrigger("IsMelee");
                ApplyQuickAttack();

                Invoke("CallBasicMelee", .1f);
                Invoke("CallBasicMelee", .4f);
                Invoke("CallBasicMelee", .7f);
                FastSlashes.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bool willExecute = ExecuteCombatAbility("BuffMelee", "Buffs Melee for duration");
            if (willExecute)
            {
                StartCoroutine(ApplyMeleePowerUp());

                BuffMelee();
                Invoke("RemoveBuffMelee", 7f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            bool willExecute = ExecuteCombatAbility("BuffMagic", "Buffs Magic for duration");
            if (willExecute)
            {

                StartCoroutine(ApplyMagicPowerUp());

                BuffMagic();
                Invoke("RemoveBuffMagic", 7f);
            }
        }

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                moveDirection.x = moveDirection.x * 1.4f;
                /*
                anim.SetTrigger("StartJump");
                anim.SetBool("Land", true);
                ApplyWalking();
                */
                anim.SetTrigger("StartJump");
                anim.SetBool("Jump", true);
                anim.SetBool("Land", true);
                //anim.SetBool("InAir", true);
                ApplyWalking();


            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //super jump
                moveDirection.x = moveDirection.x * 1.4f;
                moveDirection.y = jumpSpeed * 2;

                //sprint
            }

            if (Input.GetKey(KeyCode.Z))
            {
                moveDirection.x += moveDirection.x * .6f;
                moveDirection.z += moveDirection.z * .6f;
            }



        }
        else
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), moveDirection.y, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x = moveDirection.x * speed;
            moveDirection.z = moveDirection.z * speed;
            //moveDirection.x = Input.GetAxis("Vertical");
            //moveDirection.z = Input.GetAxis("Horizontal");
            //moveDirection = transform.TransformDirection(moveDirection);
            //moveDirection.x = moveDirection.x * speed;
            //moveDirection.z = moveDirection.z * speed;
        }
        // gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
        // move
        controller.Move(moveDirection * Time.deltaTime);
        ApplyWalking();



    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    void ApplyWalking()
    {
        if (Input.GetKey(KeyCode.W))
            anim.SetBool("isWalkingStraight", true);

        else
            anim.SetBool("isWalkingStraight", false);

        if (Input.GetKey(KeyCode.S))

            anim.SetBool("isWalkingBack", true);

        else
            anim.SetBool("isWalkingBack", false);

        if (Input.GetKey(KeyCode.A))
            anim.SetBool("isWalkingLeft", true);
        else
            anim.SetBool("isWalkingLeft", false);

        if (Input.GetKey(KeyCode.D))
            anim.SetBool("isWalkingRight", true);
        else
            anim.SetBool("isWalkingRight", false);
    }
    //----------------------------------------------------
    IEnumerator ApplyMeleePowerUp()
    {

        anim.SetTrigger("meleePowerUp");
        //speed = 0;

        yield return new WaitForSeconds(1.75f);
        //speed = 6.0f;

    }

    IEnumerator ApplyMagicPowerUp()
    {

        anim.SetTrigger("magicPowerUp");

        //speed = 0;

        yield return new WaitForSeconds(2.7f);
       // speed = 6.0f;
    }

    void ApplyQuickAttack()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            anim.SetTrigger("quickAttack");

        }


    }
}
