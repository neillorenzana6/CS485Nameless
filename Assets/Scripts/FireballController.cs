using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using NamelessGame.Combat;
using NamelessGame.Combat.Abilities;

public class FireballController : MonoBehaviour
{
    public float speed;
    public float team = 0;
    public CombatAttack atk;
    public ProjectileLauncherController FiredFrom;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Destroy(gameObject, 5);
    }

    void OnTriggerEnter(Collider other)
    {
        try
        {
            Debug.Log("hit " + other.name );
            var enemy = other.GetComponentInParent<CombatantScript>(); 
            if (enemy.team != team)
            {
                enemy.DamageCombatant(atk);
                Destroy(gameObject);
            }
        }
        catch (Exception ex) {
            Debug.Log("err");
        }
    }
}
