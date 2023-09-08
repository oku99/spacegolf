using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creates a Mesh for the Planet's Terrain on each Face (6 per planet)

public class TerrainFaceMesh
{
    private ShapeGenerator _shapeGenerator;
    private Mesh _mesh;
    private int _resolution;
    private Vector3 _localUp, _axisA, _axisB;  // LocalUp is the Normal. If localUp is Y Axis, the others are X and Z

    private bool _seamSolution = false; // test (*ss --> seam solution)   ||  might mess with generation later on. Leave false until sure
    
    public TerrainFaceMesh(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this._shapeGenerator = shapeGenerator;
        this._mesh = mesh;
        this._resolution = resolution;
        this._localUp = localUp;

        this._axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        this._axisB = Vector3.Cross(localUp, _axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[_resolution * _resolution];
        int[] triangles = new int[(_resolution - 1) * (_resolution - 1) * 2 * 3];  // 2 triangles in one Pixel/Resolution, and 3 vertices to make a triangle
        int triangleIndex = 0;
        Vector3[] normals = new Vector3[_resolution * _resolution];  // (*ss)

        // Adding Vertices
        int i = 0;
        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);  // percentage of completion on each axis
                Vector3 pointOnUnitCube = _localUp + (percent.x - 0.5f) * 2 * _axisA + (percent.y - 0.5f) * 2 * _axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;  // turns the points into a sphere shape
                vertices[i] = _shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere); 
                
                if (x != _resolution - 1 && y != _resolution - 1)
                {
                    // First Triangle of Pixel
                    triangles[triangleIndex] = i;
                    triangles[triangleIndex+1] = i+_resolution+1;
                    triangles[triangleIndex+2] = i+_resolution;

                    // Second Triangle of Pixel
                    triangles[triangleIndex+3] = i;
                    triangles[triangleIndex+4] = i+1;
                    triangles[triangleIndex+5] = i+_resolution+1;

                    triangleIndex += 6;
                }
                
                normals[i] = vertices[i].normalized;  // (*ss)
                
                i++;
            }
        }

        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        

        // (*ss) 
        if (_seamSolution) { _mesh.normals = normals; }
        else { _mesh.RecalculateNormals(); }
    }
}


/*
 * CODE ADAPTED FROM
 *      - "[Unity] Procedural Planets (E01 the sphere)" by Sebastian Lague
 *          - https://www.youtube.com/watch?v=QN39W020LqU
 *      - "[Unity] Procedural Planets (E02 settings editor)" by Sebastian Lague
 *          - https://www.youtube.com/watch?v=LyV7cEQyZMk
 */