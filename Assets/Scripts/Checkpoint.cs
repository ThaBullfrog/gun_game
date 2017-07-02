using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Sprite unactiveSprite;
    public Sprite activeSprite;

    public bool active { get; private set; }

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        SavedObjectsTracker.AddCheckpointReference(gameObject);
        spriteRenderer = this.RequireComponent<SpriteRenderer>();
        spriteRenderer.sprite = unactiveSprite;
        active = false;
    }

    private void OnDestroy()
    {
        SavedObjectsTracker.RemoveCheckpointReference(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!active && other.gameObject == Game.player)
        {
            Activate();
        }
    }

    public void Activate()
    {
        ActivateWithoutSave();
        Game.Save();
    }

    public void ActivateWithoutSave()
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = activeSprite;
        active = true;
    }
}