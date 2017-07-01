using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface IDead
{
    bool dead { get; }
    GameObject body { get; }
}