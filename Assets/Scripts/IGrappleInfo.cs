using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface IGrappleInfo
{
    bool grappled { get; }
    Vector2 direction { get; }
}