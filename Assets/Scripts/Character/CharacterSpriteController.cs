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
    private IGrappleInfo grappleInfo;
    private ICharacterInput input;
    private ICharacterAudio characetAudio;
    private enum Foot { Left, Right }
    private Foot prevFoot = Foot.Left;
    private bool prevOnGround = false;
    private float xPositionWhenLastAnimated;
    private float distanceBetweenAnimations = 35f;
    private float distanceSinceLastAnimated { get { return Mathf.Abs(transform.position.x - xPositionWhenLastAnimated); } }
    private Rigidbody2D rb;

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
        grappleInfo = GetComponent<IGrappleInfo>();
        sprite = neutralSprite;
        UpdateXPositionWhenLastAnimated();
        groundDetector = GetComponent<IGroundDetector>();
        input = GetComponent<ICharacterInput>();
        characetAudio = GetComponent<ICharacterAudio>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
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
        bool grappled = grappleInfo != null ? grappleInfo.grappled : false;
        if (grappleInfo != null && grappled && !onGround)
        {
            spriteRenderer.transform.up = grappleInfo.direction.Vector3();
        }
        else
        {
            spriteRenderer.transform.up = Vector3.up;
        }
        if (updateGunPosition != null)
        {
            updateGunPosition();
        }
        prevOnGround = onGround;
    }
    
    private void AnimateOnGround()
    {
        if (rb != null && rb.velocity.x == 0f)
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
        if (rb != null)
        {
            if (rb.velocity.x > 0f)
            {
                FaceRight();
            }
            if (rb.velocity.x < 0f)
            {
                FaceLeft();
            }
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
