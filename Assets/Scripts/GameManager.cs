using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public event EventHandler OnPlayerDamage;
    public event EventHandler OnPlayerDrain;
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPowerGain;


    private void Awake() 
    {
        Instance = this;
    }

    public void PlayerDamage() 
    {
        OnPlayerDamage?.Invoke(this, EventArgs.Empty);
    }

    public void PlayerDrain() 
    {
        OnPlayerDrain?.Invoke(this, EventArgs.Empty);
    }

    public void PlayerDeath()
    {
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }

    public void PowerGain()
    {
        OnPowerGain?.Invoke(this, EventArgs.Empty);
    }

}
