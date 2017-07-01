using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EntityNazi : EntityCharacter
{

    protected ICharacterInput input;
    protected PrefabNames prefabNames;

    public override void Build()
    {
        base.Build();
        characterGunController.bulletPrefab = GetAsset<Transform>("Assets/Prefabs/NaziBullet.prefab");
        characterGunController.bulletSpread = 0f;
        characterGunController.delayBetweenShots = 1f;
        characterGunController.bulletSpeed = 1400f;
        characterDeathHandler.bodyPrefab = GetAsset<Transform>("Assets/Prefabs/Resources/NaziBody.prefab");
        input = main.AddComponent<CharacterInputAI>();
        prefabNames = main.AddComponent<PrefabNames>();
        prefabNames.prefabName = "Nazi";
        prefabNames.bodyPrefabName = "NaziBody";
    }

    protected override void AssignSprites()
    {
        if (spriteBasePath == "")
        {
            spriteBasePath = "Assets/Sprites/Nazi/";
        }
        neutralSprite = GetSprite("nazi00.png");
        rightFootForwardSprite = GetSprite("nazi00.png");
        leftFootForwardSprite = GetSprite("nazi02.png");
        jumpSprite = neutralSprite;
        gunAndArmsSprite = GetSprite("gun_and_arms.png");
        deadFaceUpSprite = GetSprite("nazi_dead_face_up.png");
        deadFaceDownSprite = GetSprite("nazi_dead_face_down.png");
    }

}