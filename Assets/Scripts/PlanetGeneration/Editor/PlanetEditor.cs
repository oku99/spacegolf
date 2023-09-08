using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Adds the buttons for Planet Generation on the Inspector

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet planet;

    private void OnEnable()
    {
        planet = (Planet)target;
    }

    public override void OnInspectorGUI()
    {
        using (var updated = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (updated.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            planet.GeneratePlanet();
        }

        if (GUILayout.Button("Randomise"))
        {
            planet.RandomisePlanet();
        }
    }
}


/*
 * CODE ADAPTED FROM 
 *      - "[Unity] Procedural Planets (E02 settings editor)" by Sebastian Lague
 *          - https://www.youtube.com/watch?v=LyV7cEQyZMk
 *      - "Custom Editors" from Unity Documentation
 *          - https://docs.unity3d.com/Manual/editor-CustomEditors.html
*/