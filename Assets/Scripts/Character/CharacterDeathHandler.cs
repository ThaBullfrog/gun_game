using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CharacterDeathHandler : MonoBehaviour, INeedsDirectionDamagedFrom
{
    public Sprite deadFaceUp;
    public Sprite deadFaceDown;
    public Transform bodyPrefab;
    public Transform bodySpawnPoint;
    public Vector2 fallVector;

    private IDirectionFacing directionFacing;
    private enum FallDirection { Left, Right }
    private FallDirection fallDirection = FallDirection.Right;

    public Vector2 directionDamagedFrom
    {
        set
        {
            fallDirection = value.x < 0 ? FallDirection.Left : FallDirection.Right;
        }
    }

    private void Start()
    {
        IDeathAlert alert = this.RequireComponent<IDeathAlert>();
        alert.onDeath += OnDeath;
        directionFacing = GetComponent<IDirectionFacing>();
    }

    private void OnDeath()
    {
        Destroy(GetComponent<DistanceJoint2D>());
        SpawnBody();
        Destroy(gameObject);
    }

    private void SpawnBody()
    {
        bool faceUp = true;
        if (directionFacing != null)
        {
            if (GetDirectionFacing(directionFacing) == fallDirection)
            {
                faceUp = false;
            }
        }
        GameObject body = Instantiate(bodyPrefab).gameObject;
        body.transform.parent = Game.clones;
        body.transform.position = bodySpawnPoint.position;
        Rigidbody2D bodyRb = body.GetComponent<Rigidbody2D>();
        if (bodyRb != null)
        {
            bodyRb.velocity = new Vector2((fallDirection == FallDirection.Right ? 1f : -1f) * fallVector.x, fallVector.y);
        }
        SpriteRenderer spriteRenderer = body.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = faceUp ? deadFaceUp : deadFaceDown;
        }
        float xScale = fallDirection == FallDirection.Left ? -1f : 1f;
        body.transform.localScale = new Vector3(xScale, 1f, 1f);
    }

    private FallDirection GetDirectionFacing(IDirectionFacing detector)
    {
        return detector.facingRight ? FallDirection.Right : FallDirection.Left;
    }
}