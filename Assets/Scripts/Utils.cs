using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public static class Utils
{
    public static Texture2D ColorTexture(Color color)
    {
        Texture2D returnVal = new Texture2D(1, 1);
        returnVal.SetPixel(0, 0, color);
        returnVal.Apply();
        return returnVal;
    }
}