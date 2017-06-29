using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EntityCharacter : EntityBuilder
{
    protected GameObject main;
    protected GameObject sprite;
    protected GameObject gunPivot;
    protected GameObject gunAndArms;
    protected GameObject bulletSpawnPoint;
    protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;
    protected CharacterMovement characterMovement;
    protected SpriteRenderer mainSpriteRenderer;
    protected CharacterSpriteController characterSpriteController;
    protected SpriteRenderer gunAndArmsSpriteRenderer;
    protected CharacterGunController characterGunController;
    protected AudioSource jumpAudioSource;
    protected AudioSource footstepsAudioSource;
    protected AudioSource gunshotsAudioSource;
    protected CharacterAudio characterAudio;
    protected Health health;
    protected CharacterDeathHandler characterDeathHandler;

    protected string spriteBasePath = "";
    protected Sprite neutralSprite;
    protected Sprite rightFootForwardSprite;
    protected Sprite leftFootForwardSprite;
    protected Sprite jumpSprite;
    protected Sprite gunAndArmsSprite;
    protected Sprite deadFaceUpSprite;
    protected Sprite deadFaceDownSprite;

    protected string clipBasePath = "";
    protected AudioClip jumpClip;
    protected AudioClip[] footstepClips;
    protected AudioClip gunshotClip;
    protected AudioClip deathClip;

    public override void Build()
    {
        AssignSprites();
        AssignClips();
        SetupHierarchy();
        SetupRigidbody();
        SetupBoxCollider();
        characterMovement = main.AddComponent<CharacterMovement>();
        characterMovement.groundCheckLayerMask = LayerMask.GetMask(new string[] { "Default", "Characters" });
        SetupMainSpriteRenderer();
        SetupCharacterSpriteController();
        SetupGunAndArmsSpriteRenderer();
        SetupCharacterGunController();
        SetupJumpAudioSource();
        SetupFootstepsAudioSource();
        SetupGunshotsAudioSource();
        SetupCharacterAudio();
        health = gameObject.AddComponent<Health>();
        SetupCharacterDeathHandler();
    }

    private void SetupHierarchy()
    {
        main = gameObject;
        main.layer = LayerMask.NameToLayer("Characters");
        sprite = new GameObject("sprite");
        gunPivot = new GameObject("gun_pivot");
        gunAndArms = new GameObject("gun_and_arms");
        bulletSpawnPoint = new GameObject("bullet_spawn_point");
        sprite.transform.ParentToThenClearLocal(main.transform);
        gunPivot.transform.ParentToThenClearLocal(sprite.transform);
        gunPivot.transform.localPosition = new Vector3(2f, -20.7f, 0f);
        gunAndArms.transform.ParentToThenClearLocal(gunPivot.transform);
        gunAndArms.transform.localPosition = new Vector3(25.8f, -5.1f, 0f);
        bulletSpawnPoint.transform.ParentToThenClearLocal(gunPivot.transform);
        bulletSpawnPoint.transform.localPosition = new Vector3(69f, -2f, 0f);
    }

    private void SetupRigidbody()
    {
        rb = main.AddComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void SetupBoxCollider()
    {
        boxCollider = main.AddComponent<BoxCollider2D>();
        boxCollider.sharedMaterial = GetAsset<PhysicsMaterial2D>("Assets/Physics Materials/ZeroFriction.physicsMaterial2D");
        boxCollider.offset = new Vector2(0.6863236f, -18.81684f);
        boxCollider.size = new Vector2(31.76363f, 90.36632f);
    }

    private void SetupMainSpriteRenderer()
    {
        mainSpriteRenderer = sprite.AddComponent<SpriteRenderer>();
        mainSpriteRenderer.sprite = neutralSprite;
    }

    private void SetupCharacterSpriteController()
    {
        characterSpriteController = main.AddComponent<CharacterSpriteController>();
        characterSpriteController.spriteRenderer = mainSpriteRenderer;
        characterSpriteController.neutralSprite = neutralSprite;
        characterSpriteController.rightFootForwardSprite = rightFootForwardSprite;
        characterSpriteController.leftFootForwardSprite = leftFootForwardSprite;
        characterSpriteController.jumpSprite = jumpSprite;
    }

    private void SetupGunAndArmsSpriteRenderer()
    {
        gunAndArmsSpriteRenderer = gunAndArms.AddComponent<SpriteRenderer>();
        gunAndArmsSpriteRenderer.sprite = gunAndArmsSprite;
        gunAndArmsSpriteRenderer.sortingLayerName = "Above";
    }

    private void SetupCharacterGunController()
    {
        characterGunController = main.AddComponent<CharacterGunController>();
        characterGunController.spriteRenderer = gunAndArmsSpriteRenderer;
        characterGunController.pivot = gunPivot.transform;
        characterGunController.bulletPrefab = GetAsset<Transform>("Assets/Prefabs/Bullet.prefab");
        characterGunController.bulletSpawnPoint = bulletSpawnPoint.transform;
    }

    protected virtual void AssignSprites()
    {
        if (spriteBasePath == "")
        {
            spriteBasePath = "Assets/Sprites/Player/";
        }
        neutralSprite = GetSprite("player00.png");
        rightFootForwardSprite = GetSprite("player01.png");
        leftFootForwardSprite = GetSprite("player02.png");
        jumpSprite = neutralSprite;
        gunAndArmsSprite = GetSprite("gun_and_arms.png");
        deadFaceUpSprite = GetSprite("player_dead_face_up.png");
        deadFaceDownSprite = GetSprite("player_dead_face_down.png");
    }

    protected Sprite GetSprite(string path)
    {
        return GetAsset<Sprite>(spriteBasePath + path);
    }

    private void SetupJumpAudioSource()
    {
        jumpAudioSource = main.AddAudioSource();
        jumpAudioSource.clip = jumpClip;
    }

    private void SetupFootstepsAudioSource()
    {
        footstepsAudioSource = main.AddAudioSource();
        footstepsAudioSource.clip = footstepClips[0];
    }

    private void SetupGunshotsAudioSource()
    {
        gunshotsAudioSource = main.AddAudioSource();
        gunshotsAudioSource.clip = gunshotClip;
    }

    protected virtual void AssignClips()
    {
        if (clipBasePath == "")
        {
            clipBasePath = "Assets/Sounds/";
        }
        jumpClip = GetClip("jump.ogg");
        footstepClips = new AudioClip[2];
        footstepClips[0] = GetClip("footstep00.wav");
        footstepClips[1] = GetClip("footstep01.wav");
        gunshotClip = GetClip("gunshot2.ogg");
        deathClip = GetClip("death.ogg");
    }

    private AudioClip GetClip(string path)
    {
        return GetAsset<AudioClip>(clipBasePath + path);
    }

    private void SetupCharacterAudio()
    {
        characterAudio = main.AddComponent<CharacterAudio>();
        characterAudio.gunshot = gunshotsAudioSource;
        characterAudio.jump = jumpAudioSource;
        characterAudio.footsteps = footstepsAudioSource;
        characterAudio.footstepClips = footstepClips;
        characterAudio.death = deathClip;
    }

    private void SetupCharacterDeathHandler()
    {
        characterDeathHandler = gameObject.AddComponent<CharacterDeathHandler>();
        characterDeathHandler.deadFaceUp = deadFaceUpSprite;
        characterDeathHandler.deadFaceDown = deadFaceDownSprite;
        characterDeathHandler.bodyPrefab = GetAsset<Transform>("Assets/Prefabs/PlayerBody.prefab");
        characterDeathHandler.bodySpawnPoint = main.transform;
        characterDeathHandler.fallVector = new Vector2(300f, 300f);
    }
}