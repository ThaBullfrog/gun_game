using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CodeGenerator : EditorWindow
{
    [MenuItem("Window/Code Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CodeGenerator));
    }

    private string buttonText = "Generate code and copy to clipboard.";

    private GameObject nestedEntGameObject;

    private PolygonCollider2D poly;
    private EdgeCollider2D edge;

    void OnEnable()
    {
        nestedEntGameObject = null;
        poly = null;
        edge = null;
    }

    void OnGUI()
    {
        GUILayout.Label("These code generators copy code to your clipboard to be pasted in an entities 'Build()' function.");
        NewLine();
        NestedEntitiesCodeGenerator();
        NewLine();
        ColliderCodeGenerators();
    }

    private void NewLine()
    {
        GUILayout.Label("");
    }

    private void NestedEntitiesCodeGenerator()
    {
        GUILayout.Label("Generates code for the given game object's nested entities.");
        nestedEntGameObject = (GameObject)EditorGUILayout.ObjectField("Game Object", nestedEntGameObject, typeof(GameObject), true);
        if(nestedEntGameObject && GUILayout.Button(buttonText))
        {
            EntityBuilder[] entityChildren = new List<EntityBuilder>(nestedEntGameObject.GetComponentsInChildren<EntityBuilder>()).FindAll
                (x => x.transform.parent == nestedEntGameObject.transform).ToArray();
            string clipboard = "";
            Dictionary<System.Type, int> numberOfEachType = new Dictionary<System.Type, int>();
            for(int i = 0; i < entityChildren.Length; i++)
            {
                System.Type type = entityChildren[i].GetType();
                if(i != 0)
                {
                    clipboard += System.Environment.NewLine;
                }
                if(!numberOfEachType.ContainsKey(type))
                {
                    numberOfEachType.Add(type, 0);
                }
                numberOfEachType[type] += 1;
                clipboard += GenerateChildEntityCode(entityChildren[i], numberOfEachType);
            }
            EditorGUIUtility.systemCopyBuffer = clipboard;
        }
    }

    private string GenerateChildEntityCode(EntityBuilder child, Dictionary<System.Type, int> numberOfEachType)
    {
        string nl = System.Environment.NewLine;
        System.Type type = child.GetType();
        string varName = LowerCaseOfFirstLetter(type.Name) + numberOfEachType[type].ToString();
        string code = type.Name + " " + varName + " = EntityBuilder.CreateEntity<" + type.Name + ">();" + nl;
        code += varName + ".transform.parent = transform;" + nl;
        code += varName + ".transform.localPosition = " + child.transform.localPosition.ToCodeString() + ";" + nl;
        code += varName + ".transform.localRotation = " + child.transform.localRotation.ToCodeString() + ";" + nl;
        code += varName + ".transform.localScale = " + child.transform.localScale.ToCodeString() + ";";
        return code;
    }

    private string LowerCaseOfFirstLetter(string str)
    {
        string firstLetter = str.Substring(0, 1);
        str = str.Remove(0, 1).Insert(0, firstLetter.ToLower());
        return str;
    }

    private void ColliderCodeGenerators()
    {
        GUILayout.Label("These generator code for the shape of an edge collider or a polygon collider.");
        poly = (PolygonCollider2D)EditorGUILayout.ObjectField("Polygon Collider", poly, typeof(PolygonCollider2D), true, new GUILayoutOption[0]);
        if (poly != null && GUILayout.Button(buttonText))
        {
            EditorGUIUtility.systemCopyBuffer = GenerateLines(poly).Join(System.Environment.NewLine);
        }
        edge = (EdgeCollider2D)EditorGUILayout.ObjectField("Edge Collider", edge, typeof(EdgeCollider2D), true);
        if (edge != null && GUILayout.Button(buttonText))
        {
            EditorGUIUtility.systemCopyBuffer = GenerateLines(edge).Join(System.Environment.NewLine);
        }
    }

    private string[] GenerateLines(PolygonCollider2D poly)
    {
        string[] lines = new string[poly.points.Length + 3];
        lines[0] = "PolygonCollider2D poly = gameObject.AddComponent<PolygonCollider2D>();";
        lines[1] = "Vector2[] points = new Vector2[" + poly.points.Length + "];";
        for (int i = 2; i < lines.Length - 1; i++)
        {
            lines[i] = "points[" + (i - 2) + "] = " + poly.points[i - 2].ToCodeString() + ";";
        }
        lines[lines.Length - 1] = "poly.points = points;";
        return lines;
    }

    private string[] GenerateLines(EdgeCollider2D edge)
    {
        string[] lines = new string[edge.points.Length + 3];
        lines[0] = "EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D>();";
        lines[1] = "Vector2[] points = new Vector2[" + edge.points.Length + "];";
        for (int i = 2; i < lines.Length - 1; i++)
        {
            lines[i] = "points[" + (i - 2) + "] = " + edge.points[i - 2].ToCodeString() + ";";
        }
        lines[lines.Length - 1] = "edge.points = points;";
        return lines;
    }
}