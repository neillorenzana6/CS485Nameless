using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private Image foregroundImage;
    [SerializeField]
    private float updateSpeedSeconds = 0.5f;
    public CombatantScript player;


    // Start is called before the first frame update
    void Start()
    {
        player.OnHealthChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float health)
    {
        StartCoroutine(ChangeHealthBar(health));
    }

    private IEnumerator ChangeHealthBar(float health)
    {
        float preChanged = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChanged, health, elapsed / updateSpeedSeconds);
            yield return null;
        }

        foregroundImage.fillAmount = health;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
