using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBarUI : MonoBehaviour
{
    private Image healthBarImage;
    public float damageTick = 0.01f;

    private void Awake() {
        healthBarImage = GameObject.Find("HealthBar").GetComponent<Image>();

        healthBarImage.fillAmount = 1f;
    }

    private void Start() {
        GameManager.Instance.OnPlayerDamage += OnPlayerDamageDelegate;
        GameManager.Instance.OnPlayerDrain += OnPlayerDrainDelegate;

    }

    private void OnPlayerDamageDelegate(object sender, EventArgs e)
    {   
        if(healthBarImage.fillAmount > 0)
        {
            healthBarImage.fillAmount -= damageTick;
        } else
        {
            GameManager.Instance.PlayerDeath();
        }
    }

    private void OnPlayerDrainDelegate(object sender, EventArgs e)
    {   
        healthBarImage.fillAmount = 0;
    }
}
