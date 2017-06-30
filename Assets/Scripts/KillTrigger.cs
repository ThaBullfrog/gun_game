using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        collider.gameObject.SetDamageDirection(collider.transform.position - transform.position);
        collider.gameObject.KillIfDamageable();
    }
}