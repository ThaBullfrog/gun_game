using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public static class Extensions
{
	public static Vector2 Vector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

	public static Vector3 Vector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0f);
    }

    public static T RequireComponent<T>(this Component component)
    {
        T returnVal = component.gameObject.GetComponent<T>();
        RaiseRequireComponentErrorIfNull(returnVal, component.GetType().Name, typeof(T).Name, component.gameObject.name);
        return returnVal;
    }

    public static Component RequireComponent(this Component component, System.Type type)
    {
        Component returnVal = component.gameObject.GetComponent(type);
        RaiseRequireComponentErrorIfNull(returnVal, component.GetType().Name, type.Name, component.gameObject.name);
        return returnVal;
    }

    public static T RequireComponent<T>(this GameObject gameObject)
    {
        T returnVal = gameObject.GetComponent<T>();
        RaiseRequireComponentErrorIfNullGameObject(returnVal, gameObject.name, typeof(T).Name);
        return returnVal;
    }

    public static Component RequireComponent(this GameObject gameObject, System.Type type)
    {
        Component returnVal = gameObject.GetComponent(type);
        RaiseRequireComponentErrorIfNullGameObject(returnVal, gameObject.name, type.Name);
        return returnVal;
    }

    private static void RaiseRequireComponentErrorIfNull(object obj, string componentName, string requiredComponentName, string gameObjectName)
    {
        if(obj == null)
        {
            Debug.LogError("Component '" + componentName + "' requires component '" + requiredComponentName + "' but none were found on game object '" + gameObjectName + "'");
        }
    }

    private static void RaiseRequireComponentErrorIfNullGameObject(object obj, string gameObjectName, string requiredComponentName)
    {
        if (obj == null)
        {
            Debug.LogError("GameObject '" + gameObjectName + "' requires component '" + requiredComponentName + "' but none were found'");
        }
    }

    public static T InstantiateAndRequireComponent<T>(this GameObject i, Vector3 location) where T : class, IInterfaceWithGameObject
    {
        T returnVal = Object.Instantiate(i.gameObject, location, Quaternion.identity).RequireComponent(typeof(T)) as T;
        returnVal.gameObject.transform.parent = Game.clones;
        return returnVal;
    }

    public static string ToCodeString(this Vector2 vec)
    {
        return "new Vector2(" + vec.x + "f, " + vec.y + "f)";
    }

    public static string ToCodeString(this Vector3 vec)
    {
        return "new Vector3(" + vec.x + "f, " + vec.y + "f, " + vec.z + "f)";
    }

    public static string ToCodeString(this Quaternion rot)
    {
        return "new Quaternion(" + rot.x + "f, " + rot.y + "f, " + rot.z + "f, " + rot.w + "f)";
    }

    public static string Join(this string[] strings, string separator = "")
    {
        string str = "";
        bool first = true;
        for (int i = 0; i < strings.Length; i++)
        {
            if (!first)
            {
                str += separator;
            }
            else
            {
                first = false;
            }
            str += strings[i];
        }
        return str;
    }

    public static void ClearLocalTransform(this Transform transform)
    {
        transform.localPosition = UnityEngine.Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = UnityEngine.Vector3.one;
    }

    public static void ParentToThenClearLocal(this Transform transform, Transform parent)
    {
        transform.parent = parent;
        transform.ClearLocalTransform();
    }

    public static AudioSource AddAudioSource(this GameObject obj)
    {
        AudioSource returnVal = obj.AddComponent<AudioSource>();
        returnVal.playOnAwake = false;
        returnVal.spatialBlend = 0f;
        return returnVal;
    }
}