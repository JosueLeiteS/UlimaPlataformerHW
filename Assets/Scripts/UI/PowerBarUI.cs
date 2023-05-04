using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PowerBarUI : MonoBehaviour
{
    private Image powerBarImage;
    public float powerGain = 0.5f;

    private void Awake() {
        powerBarImage = GameObject.Find("PowerBar").GetComponent<Image>();

        powerBarImage.fillAmount = 0f;
    }

    private void Start() {
        GameManager.Instance.OnPowerGain += OnPowerGainDelegate;
    }

    private void OnPowerGainDelegate(object sender, EventArgs e)
    {
        if(powerBarImage.fillAmount < 1)
        {
            powerBarImage.fillAmount += powerGain;
        } 
    }
}
