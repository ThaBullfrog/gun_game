using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGunController : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Transform pivot;
    public Transform bulletPrefab;
    public Transform bulletSpawnPoint;
    public float lowerWhileStepping = 5f;
    public float delayBetweenShots = 0.075f;
    public float bulletSpeed = 2400f;
    public float bulletSpread = 6f;
    public float kickDistance = 5f;
    public float kickForce = 20f;
    public float kickDuration = 0.0375f;

    private ICharacterInput input;
    private ICharacterAudio characterAudio;
    private CharacterSpriteController characterSpriteController;
    private Vector3 rootPosition;
    private float timeWhenCanShootAgain = 0f;
    private float kickUntilThisTime = 0f;
    private Rigidbody2D rb;
    private bool testingAim = false;

    private void Start()
    {
        characterSpriteController = GetComponent<CharacterSpriteController>();
        if(characterSpriteController != null)
        {
            characterSpriteController.updateGunPosition += UpdateGunPosition;
        }
        rootPosition = pivot.localPosition;
        input = GetComponent<ICharacterInput>();
        characterAudio = GetComponent<ICharacterAudio>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void UpdateGunPosition()
    {
        ResetToRootTransform();
        StayLevelWithTorso();
        Vector3 aimDirection = GetAimDirection();
        if(input != null && (testingAim || input.shooting))
        {
            pivot.right = aimDirection * pivot.parent.localScale.x;
        }
        if (input != null && !testingAim && input.shooting)
        {
            if (Time.time > timeWhenCanShootAgain)
            {
                Shoot(aimDirection);
            }
        }
        if (Time.time < kickUntilThisTime)
        {
            pivot.position += aimDirection * -kickDistance;
        }
    }

    /*private void LateUpdate()
    {
        Vector3 aimDirection = GetAimDirection();
        if (input != null && input.shooting)
        {
            if (Time.time > timeWhenCanShootAgain)
            {
                Shoot(aimDirection);
            }
        }
    }*/

    private void ResetToRootTransform()
    {
        pivot.localPosition = rootPosition;
        pivot.localRotation = Quaternion.identity;
    }

    private void StayLevelWithTorso()
    {
        if (characterSpriteController != null && (characterSpriteController.sprite == characterSpriteController.rightFootForwardSprite || characterSpriteController.sprite == characterSpriteController.leftFootForwardSprite))
        {
            pivot.localPosition += new Vector3(0f, -5f, 0f);
        }
    }

    private void Shoot(Vector3 direction)
    {
        float halfSpread = bulletSpread / 2f;
        direction = Quaternion.AngleAxis(Random.Range(-halfSpread, halfSpread), Vector3.forward) * direction;
        if (characterAudio != null)
        {
            characterAudio.PlayGunshot();
        }
        timeWhenCanShootAgain = Time.time + delayBetweenShots;
        IBullet newBullet = bulletPrefab.gameObject.InstantiateAndRequireComponent<IBullet>(bulletSpawnPoint.position);
        newBullet.startingVelocity = direction * bulletSpeed;
        newBullet.owner = gameObject;
        if(rb != null)
        {
            rb.velocity += -direction.Vector2() * kickForce;
        }
        kickUntilThisTime = Time.time + kickDuration;
    }

    private Vector2 GetAimDirection()
    {
        return input != null ? (input.aimLocation.Vector3() - pivot.position).normalized : Vector3.right;
    }

    public Vector3 PredictBulletSpawnLocation()
    {
        bool wasFacingRight = true;
        if(characterSpriteController != null)
        {
            wasFacingRight = characterSpriteController.facingRight;
            characterSpriteController.FaceInDirectionOfAiming();
        }
        Vector3 savedPivotLocation = pivot.localPosition;
        Quaternion savedPivotRotation = pivot.localRotation;
        testingAim = true;
        UpdateGunPosition();
        testingAim = false;
        Vector3 returnVal = bulletSpawnPoint.position;
        pivot.localPosition = savedPivotLocation;
        pivot.localRotation = savedPivotRotation;
        if(characterSpriteController != null)
        {
            characterSpriteController.facingRight = wasFacingRight;
        }
        return returnVal;
    }

}
