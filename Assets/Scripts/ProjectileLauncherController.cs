using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NamelessGame.Combat;
using NamelessGame.Combat.Abilities;

public class ProjectileLauncherController : MonoBehaviour
{
    public bool fire;
    public bool combo;
    public CombatantScript combatantOwner;
    public FireballController fireball;
    public float shootSpeed;
    public float shotInterval;
    public float cooldown;
    private float shotCount;
    private float timer;
    public CombatAttack atk;

    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        timer = cooldown;

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShootBurst()
    {
        Invoke("Shoot", .1f);
        Invoke("Shoot", .4f);
        Invoke("Shoot", .7f);
    }

    public void Shoot()
    {
        shotCount = shotInterval;
        fireball.FiredFrom = this;
        FireballController newFireball = Instantiate(fireball, firePoint.position, firePoint.rotation) as FireballController;
        newFireball.speed = shootSpeed;
        newFireball.team = combatantOwner.team;
        newFireball.atk = atk;
    }

    public void MultiShoot()
    {
        Invoke("Shoot", .1f);
        Invoke("Shoot", .7f);
        Invoke("Shoot", 1.3f);
    }


}
