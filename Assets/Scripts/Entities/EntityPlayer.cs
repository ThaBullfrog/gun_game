using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EntityPlayer : EntityCharacter
{
    protected ICharacterInput input;
    protected LineRenderer grappleLineRenderer;
    protected CharacterGrappleController characterGrappleController;

    public override void Build()
    {
        base.Build();
        input = main.AddComponent<CharacterInputKeyboard>();
        SetupGrappleLineRenderer();
        SetupCharacterGrappleController();
    }

    protected virtual void SetupGrappleLineRenderer()
    {
        grappleLineRenderer = main.AddComponent<LineRenderer>();
        grappleLineRenderer.material = GetAsset<Material>("Assets/Materials/MyDefault.mat");
        Gradient colorGradient = new Gradient();
        GradientColorKey a = new GradientColorKey(Color.red, 0f);
        GradientColorKey b = new GradientColorKey(Color.red, 1f);
        colorGradient.colorKeys = new GradientColorKey[2]{ a, b };
        colorGradient.alphaKeys = new GradientAlphaKey[2] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) };
        grappleLineRenderer.colorGradient = colorGradient;
        grappleLineRenderer.positionCount = 2;
    }

    private void SetupCharacterGrappleController()
    {
        characterGrappleController = main.AddComponent<CharacterGrappleController>();
        characterGrappleController.grappleLayerMask = LayerMask.GetMask(new string[] { "Default" });
        characterGrappleController.line = grappleLineRenderer;
    }
}