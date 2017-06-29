using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface IDeathAlert
{
    event System.Action onDeath;
}