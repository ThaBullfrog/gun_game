using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CharacterInputAI : MonoBehaviour, ICharacterInput
{

    public float move { get { return 0f; } }
    public bool jump { get { return false; } }
    public bool grapple { get { return false; } }

    private CharacterGunController gunController;

    private void Start()
    {
        gunController = GetComponent<CharacterGunController>();
    }

    public bool shooting
    {
        get
        {
            if (Game.player != null && Time.realtimeSinceStartup > 3f)
            {
                Vector3 targetPos = Game.player.transform.position;
                Vector2 origin = gunController != null ? gunController.PredictBulletSpawnLocation() : transform.position;
                Vector2 direction = targetPos - transform.position;
                float distance = direction.magnitude;
                direction.Normalize();
                int storedLayer = gameObject.layer;
                gameObject.layer = LayerMask.NameToLayer("NoCollide");
                RaycastHit2D hit = Physics2D.CircleCast(origin, 11f, direction, distance, LayerMask.GetMask("Default", "Characters"));
                gameObject.layer = storedLayer;
                return hit.collider != null && hit.collider.gameObject == Game.player;
            }
            else
            {
                return false;
            }
        }
    }

    public Vector2 aimLocation { get { return Game.player != null ? Game.player.transform.position.Vector2() : Vector2.zero; } }

}