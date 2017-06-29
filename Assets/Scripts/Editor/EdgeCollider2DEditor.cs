using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EdgeCollider2DEditor : EditorWindow
{

    [MenuItem("Window/EdgeCollider2D Tools")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EdgeCollider2DEditor));
    }

    EdgeCollider2D edgeCollider;
    Vector2[] vertices = new Vector2[0];

    private Vector2 scrollPos = Vector2.zero;
    private bool foldout = false;

    void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("EdgeCollider2D point editor", EditorStyles.boldLabel);
        edgeCollider = (EdgeCollider2D)EditorGUILayout.ObjectField("EdgeCollider2D to edit", edgeCollider, typeof(EdgeCollider2D), true);
        if (vertices.Length != 0)
        {
            foldout = EditorGUILayout.Foldout(foldout, "Points");
            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < vertices.Length; ++i)
                {
                    vertices[i] = (Vector2)EditorGUILayout.Vector2Field("Element " + i, vertices[i]);
                }
                EditorGUI.indentLevel--;
            }
        }

        if (GUILayout.Button("Retrieve"))
        {
            vertices = edgeCollider.points;
        }

        if (GUILayout.Button("Set"))
        {
            edgeCollider.points = vertices;
        }
        
        if (edgeCollider != null)
        {
            if (GUILayout.Button("Convert to Polygon Collider"))
            {
                Vector2[] myPoints = edgeCollider.points;
                PolygonCollider2D myPoly = edgeCollider.gameObject.AddComponent<PolygonCollider2D>();
                myPoly.points = myPoints;
                myPoly.pathCount = 1;
                myPoly.SetPath(0, myPoints);
            }
            if (GUILayout.Button("Mirror Across X Axis"))
            {
                List<Vector2> points = new List<Vector2>(edgeCollider.points);
                int initialCount = points.Count;
                for (int i = 0; i < initialCount; i++)
                {
                    if (points[i].x <= 0f)
                    {
                        Vector2 point = points[i];
                        point.x = 0f;
                        points[i] = point;
                    }
                    else
                    {
                        points.Insert(initialCount, new Vector2(-points[i].x, points[i].y));
                    }
                }
                edgeCollider.points = points.ToArray();
            }
            if (GUILayout.Button("Mirror Across Y Axis"))
            {
                List<Vector2> points = new List<Vector2>(edgeCollider.points);
                int initialCount = points.Count;
                for (int i = 0; i < initialCount; i++)
                {
                    if (points[i].y <= 0f)
                    {
                        Vector2 point = points[i];
                        point.y = 0f;
                        points[i] = point;
                    }
                    else
                    {
                        points.Insert(initialCount, new Vector2(points[i].x, -points[i].y));
                    }
                }
                edgeCollider.points = points.ToArray();
            }
        }

        GUILayout.EndScrollView();
    }
}