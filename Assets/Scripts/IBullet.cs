using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface IBullet : IInterfaceWithGameObject
{
    Vector2 startingVelocity { get; set; }
    GameObject owner { get; set; }
}