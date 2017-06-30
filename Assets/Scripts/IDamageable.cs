using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface IDamageable
{
    void Damage(int amount);
    void Kill();
}