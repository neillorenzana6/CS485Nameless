using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using NamelessGame.Combat;
using NamelessGame.Combat.Abilities;
using UnityEngine.UI;


public class CooldownCounter : MonoBehaviour
{
    public PlayerController player;
    public Text cooldownText;
    public string abilityToTrack = "";
    private CombatAbility trackAbility;

    // Start is called before the first frame update
    void Start()
    {
        var abilityList = player.GetComponents<CombatAbility>();
        Debug.Log(abilityList.Length);
        foreach (var ability in abilityList)
        {
            if (ability.AbilityRefName == abilityToTrack)
            {
                Debug.Log("hit2");
                trackAbility = ability;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (trackAbility != null)
        {
            Debug.Log("hit3");
            double time = Math.Round(trackAbility.Cooldown.Time - trackAbility.Cooldown.TimeRemaining(), 0);
            if (time <= 0)
            {
                cooldownText.text = "";
            }
            else
            {
                cooldownText.text = Math.Round(trackAbility.Cooldown.Time - trackAbility.Cooldown.TimeRemaining(), 0).ToString();
            }
            
        }
    }
}
