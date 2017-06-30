using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CharacterInputAI : MonoBehaviour, ICharacterInput
{

    public float reactionDelay = 0.7f;
    public float distanceCanSee = 2000f;

    public float move { get { return 0f; } }
    public bool jump { get { return false; } }
    public bool grapple { get { return false; } }

    private CharacterGunController gunController;
    private float timeWhenCantReact = 0f;
    private float timeWhenCanReact = 0f;
    private bool canReact { get { return timeWhenCanReact != 0f && Time.time > timeWhenCanReact; } }

    private void Start()
    {
        gunController = GetComponent<CharacterGunController>();
    }

    public bool shooting
    {
        get
        {
            if (Game.player != null)
            {
                Vector3 targetPos = Game.player.transform.position;
                Vector2 origin = gunController != null ? gunController.PredictBulletSpawnLocation() : transform.position;
                Vector2 direction = targetPos - transform.position;
                float distance = direction.magnitude <= distanceCanSee ? direction.magnitude : distanceCanSee;
                direction.Normalize();
                int storedLayer = gameObject.layer;
                gameObject.layer = LayerMask.NameToLayer("NoCollide");
                RaycastHit2D hit = Physics2D.CircleCast(origin, 11f, direction, distance, LayerMask.GetMask("Default", "Characters"));
                gameObject.layer = storedLayer;
                if(hit.collider != null && hit.collider.gameObject == Game.player)
                {
                    timeWhenCantReact = 0f;
                    if(timeWhenCanReact == 0f)
                    {
                        timeWhenCanReact = Time.time + reactionDelay;
                    }
                    if(canReact)
                    {
                        return true;
                    }
                }
                else
                {
                    if(canReact)
                    {
                        if(timeWhenCantReact == 0f)
                        {
                            timeWhenCantReact = Time.time + reactionDelay;
                        }
                        if(Time.time > timeWhenCantReact)
                        {
                            timeWhenCanReact = 0f;
                        }
                    }
                }
            }
            return false;
        }
    }

    public Vector2 aimLocation { get { return Game.player != null ? Game.player.transform.position.Vector2() : Vector2.zero; } }

}