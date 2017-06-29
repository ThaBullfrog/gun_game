using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{

    public LayerMask layerMask;
    public float timeToLive = 5f;
    public int damage = 1;
    
    public Vector2 startingVelocity { get; set; }
    public GameObject owner { get; set; }

    private Vector2 velocity;
    private float timeSpawned;
    private Collider2D bulletCollider;

    private void Start()
    {
        velocity = startingVelocity;
        transform.right = velocity.normalized;
        timeSpawned = Time.time;
        bulletCollider = GetComponent<Collider2D>();
        CheckForCollisions();
    }

    private void Update()
    {
        transform.position += (velocity * Time.deltaTime).Vector3();
        if(Time.time - timeSpawned > timeToLive)
        {
            Destroy(gameObject);
        }
        CheckForCollisions();
    }

    private void CheckForCollisions()
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;
        Collider2D[] overlaps = new Collider2D[5];
        int numOfOverlaps = bulletCollider.OverlapCollider(contactFilter, overlaps);
        for (int i = 0; i < numOfOverlaps; i++)
        {
            if (overlaps[i].gameObject != owner)
            {
                INeedsDirectionDamagedFrom[] componentsThatNeedDirection = overlaps[i].GetComponents<INeedsDirectionDamagedFrom>();
                foreach (INeedsDirectionDamagedFrom componentThatNeedsDirection in componentsThatNeedDirection)
                {
                    componentThatNeedsDirection.directionDamagedFrom = transform.right.Vector2().normalized;
                }
                IDamageable damageable = overlaps[i].GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(damage);
                }
            }
        }
        if (numOfOverlaps > 0)
        {
            Destroy(gameObject);
        }
    }

}