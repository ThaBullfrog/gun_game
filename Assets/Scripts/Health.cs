using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IDeathAlert
{
    public int startingHp = 1;

    private int _hp = 1;
    public int hp
    {
        get { return _hp; }
        set
        {
            if(value <= 0)
            {
                _hp = 0;
                Die();
            }
            else
            {
                _hp = value > maxHp ? maxHp : value;
            }
        }
    }
    private int maxHp;
    private bool dead = false;
    public event System.Action onDeath;

    private void Start()
    {
        maxHp = startingHp;
        hp = startingHp;
    }

    public void Damage(int amount)
    {
        hp -= amount;
    }

    public void Kill()
    {
        hp = 0;
    }

    public void Heal(int amount)
    {
        hp += amount;
    }

    public void HealToFull()
    {
        hp = maxHp;
    }

    private void Die()
    {
        if(!dead)
        {
            dead = true;
            if (onDeath != null)
            {
                onDeath();
            }
        }
    }
}