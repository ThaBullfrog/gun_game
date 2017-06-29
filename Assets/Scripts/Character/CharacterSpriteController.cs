using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteController : MonoBehaviour, IDirectionFacing
{

    public SpriteRenderer spriteRenderer;
    public Sprite neutralSprite;
    public Sprite rightFootForwardSprite;
    public Sprite leftFootForwardSprite;
    public Sprite jumpSprite;
    
    private IGroundDetector groundDetector;
    private ICharacterInput input;
    private ICharacterAudio characetAudio;
    private enum Foot { Left, Right }
    private Foot prevFoot = Foot.Left;
    private bool prevOnGround = false;
    private float xPositionWhenLastAnimated;
    private float distanceBetweenAnimations = 35f;
    private float distanceSinceLastAnimated { get { return Mathf.Abs(transform.position.x - xPositionWhenLastAnimated); } }
    private Vector3 prevPosition;

    public event System.Action updateGunPosition;
    public Sprite sprite { get{ return spriteRenderer.sprite; } private set { spriteRenderer.sprite = value; } }
    public bool facingLeft
    {
        get
        {
            return spriteRenderer.transform.localScale.x == -1;
        }
        set
        {
            if(value)
            {
                FaceLeft();
            }
            else
            {
                FaceRight();
            }
        }
    }
    public bool facingRight
    {
        get
        {
            return spriteRenderer.transform.localScale.x == 1;
        }
        set
        {
            if(value)
            {
                FaceRight();
            }
            else
            {
                FaceLeft();
            }
        }
    }

    private void Start()
    {
        sprite = neutralSprite;
        UpdateXPositionWhenLastAnimated();
        groundDetector = GetComponent<IGroundDetector>();
        input = GetComponent<ICharacterInput>();
        characetAudio = GetComponent<ICharacterAudio>();
        prevPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (input != null && input.shooting)
        {
            FaceInDirectionOfAiming();
        }
        else
        {
            FaceInDirectionOfMovement();
        }
        bool onGround = groundDetector != null ? groundDetector.onGround : true;
        if (onGround)
        {
            AnimateOnGround();
        }
        else
        {
            sprite = jumpSprite;
        }
        if (updateGunPosition != null)
        {
            updateGunPosition();
        }
        prevOnGround = onGround;
        prevPosition = transform.position;
    }
    
    private void AnimateOnGround()
    {
        if (prevPosition.x == transform.position.x)
        {
            sprite = neutralSprite;
            UpdateXPositionWhenLastAnimated();
        }
        else
        {
            if (!prevOnGround)
            {
                sprite = neutralSprite;
                UpdateXPositionWhenLastAnimated();
            }
            if (distanceSinceLastAnimated > distanceBetweenAnimations)
            {
                ProgressWalk();
            }
        }
    }

    public void FaceInDirectionOfAiming()
    {
        if(input.aimLocation.x > transform.position.x)
        {
            FaceRight();
        }
        else
        {
            FaceLeft();
        }
    }

    private void FaceInDirectionOfMovement()
    {
        if (transform.position.x > prevPosition.x)
        {
            FaceRight();
        }
        if (transform.position.x < prevPosition.x)
        {
            FaceLeft();
        }
    }

    public void FaceRight()
    {
        Vector3 currentLocalScale = spriteRenderer.transform.localScale;
        spriteRenderer.transform.localScale = new Vector3(1f, currentLocalScale.y, currentLocalScale.z);
    }

    public void FaceLeft()
    {
        Vector3 currentLocalScale = spriteRenderer.transform.localScale;
        spriteRenderer.transform.localScale = new Vector3(-1f, currentLocalScale.y, currentLocalScale.z);
    }

    private void ProgressWalk()
    {
        UpdateXPositionWhenLastAnimated();
        if(sprite == neutralSprite)
        {
            if(characetAudio != null)
            {
                characetAudio.PlayFootstep();
            }
            if (prevFoot == Foot.Left)
            {
                sprite = rightFootForwardSprite;
                prevFoot = Foot.Right;
            }
            else
            {
                sprite = leftFootForwardSprite;
                prevFoot = Foot.Left;
            }
        }
        else
        {
            sprite = neutralSprite;
        }
    }

    private void UpdateXPositionWhenLastAnimated()
    {
        xPositionWhenLastAnimated = transform.position.x;
    }

}
