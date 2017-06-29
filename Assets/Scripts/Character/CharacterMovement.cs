using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour, IGroundDetector {

    public float speed = 450f;
    public float airControl = 15f;
    public float airControlSpeedLimit = 300f;
    public float jumpStrength = 600f;
    public LayerMask groundCheckLayerMask;

    private Rigidbody2D rb;
    private BoxCollider2D boxColldier;
    private float jumpDelay = 0.25f;
    private float timeWhenCanJumpAgain = 0f;
    private ICharacterInput input;
    private ICharacterAudio characterAudio;
    private ICanDisableAirControl[] componentsThatCanDisableAirControl;
    public bool onGround { get; private set; }

    private bool airControlDisabled
    {
        get
        {
            foreach(ICanDisableAirControl component in componentsThatCanDisableAirControl)
            {
                if(component.disableAirControl)
                {
                    return true;
                }
            }
            return false;
        }
    }

    private void Start()
    {
        rb = this.RequireComponent<Rigidbody2D>();
        boxColldier = this.RequireComponent<BoxCollider2D>();
        input = GetComponent<ICharacterInput>();
        characterAudio = GetComponent<ICharacterAudio>();
        componentsThatCanDisableAirControl = GetComponents<ICanDisableAirControl>();
    }

    private void FixedUpdate()
    {
        onGround = GroundCheck();
        float moveInput = input != null ? input.move : 0f;
        if (onGround)
        {
            GroundMove(moveInput);
        }
        else
        {
            if (!airControlDisabled)
            {
                AirMove(moveInput);
            }
        }
        if(input != null && input.jump && CanJump())
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (characterAudio != null)
        {
            characterAudio.PlayJump();
        }
        rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        timeWhenCanJumpAgain = Time.time + jumpDelay;
    }

    private bool CanJump()
    {
        return Time.time >= timeWhenCanJumpAgain && onGround;
    }

    private void GroundMove(float move)
    {
        rb.velocity = new Vector2(move * speed, rb.velocity.y);
    }

    private void AirMove(float move)
    {
        float changeBy = move * airControl;
        if (Mathf.Abs(rb.velocity.x + changeBy) < Mathf.Abs(rb.velocity.x) || Mathf.Abs(rb.velocity.x) < airControlSpeedLimit)
        {
            rb.velocity = new Vector2(rb.velocity.x + changeBy, rb.velocity.y);
        }
    }

    private bool GroundCheck()
    {
        Vector2 testBoxPosition = transform.position.Vector2() + boxColldier.offset + new Vector2(0f, -1.1f);
        Vector2 testBoxSize = new Vector2(boxColldier.size.x - 2f, boxColldier.size.y - 2f);
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(testBoxPosition, testBoxSize, 0f, groundCheckLayerMask);
        foreach(Collider2D overlap in overlaps)
        {
            if(overlap != boxColldier)
            {
                return true;
            }
        }
        return false;
    }

    private bool Key(KeyCode key)
    {
        return Input.GetKey(key);
    }

}
