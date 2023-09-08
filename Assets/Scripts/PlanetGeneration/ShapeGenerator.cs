using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Generates the Noise for the Planet's Shape
public class ShapeGenerator
{
    private Planet _planet;

    public ShapeGenerator(Planet planet)
    {
        this._planet = planet;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float seedVal = _planet.seed;
        float xVal = pointOnUnitSphere.y + seedVal;
        float yVal = pointOnUnitSphere.z + seedVal;
        float elevation = Mathf.PerlinNoise(xVal, yVal) * _planet.strength;
        
        return pointOnUnitSphere * _planet.radius * (1 + elevation);  // (III)
    }
}


/*
 * CODE ADAPTED FROM
 *      - "[Unity] Procedural Planets (E02 settings editor)" by Sebastian Lague
 *          - https://www.youtube.com/watch?v=LyV7cEQyZMk
 *      - "[Unity] Procedural Planets (E03: layered noise)" by Sebastian Lague
 *          - https://www.youtube.com/watch?v=uY9PAcNMu8s
 *      - from a Comment by blizzy on "Mathf.PerlinNoise() a way to random seed it?" from Unity Forum
 *          - https://forum.unity.com/threads/mathf-perlinnoise-a-way-to-random-seed-it.336147/#post-2175574
 */