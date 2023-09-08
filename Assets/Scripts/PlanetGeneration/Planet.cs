using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// Planet Procedural Generation
public class Planet : MonoBehaviour
{
    [Range(2, 50)] public int resolution = 30;  // arbitrary default val and range
    [SerializeField, HideInInspector] private MeshFilter[] meshFilters;
    public Color color = Color.gray;
    public float radius = 1;
    public int seed = 1;
    [Range(0, 2)]public float strength = 10f;
    
    private TerrainFaceMesh[] _terrainFaceMeshes;
    private ShapeGenerator _shapeGenerator;
    
    void Initialise()
    {
        _shapeGenerator = new ShapeGenerator(this);
        
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        _terrainFaceMeshes = new TerrainFaceMesh[6];
        
        Vector3[] directions =
            { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        // to display the Mesh Faces
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObject = new GameObject("MeshFace");
                meshObject.transform.parent = transform;

                // Renders Mesh
                meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            _terrainFaceMeshes[i] = new TerrainFaceMesh(_shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    public void GeneratePlanet()
    {
        Initialise();
        GenerateMesh();
        MergeMeshes();
        ChangeColour();
    }

    void GenerateMesh()
    {
        foreach (var face in _terrainFaceMeshes) { face.ConstructMesh(); }
    }
    
    // Randomise Planet Attributes
    public void RandomisePlanet()
    {
        seed = Random.Range(0, 1000);
        strength = (float)Math.Round(Random.Range(0, 2f), 2);
        color = new Color(Random.value, Random.value, Random.value);
        
        GeneratePlanet();
    }

    // Combines the six Mesh Faces into one
    private void MergeMeshes()
    {
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        
        transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
        
        // Reset Mesh Collider
        transform.GetComponent<MeshCollider>().sharedMesh = transform.GetComponent<MeshFilter>().sharedMesh;

        transform.gameObject.SetActive(true);
    }

    // Change Material Colour of Planet without affecting the Shader
    private void ChangeColour()
    {
        Material currMaterial = transform.gameObject.GetComponent<Renderer>().sharedMaterial;
        Material newMaterial = new Material(currMaterial);
        newMaterial.color = color;
        transform.gameObject.GetComponent<Renderer>().sharedMaterial = newMaterial;
    }

    void Start()
    {
        RandomisePlanet();
    }
}


/*
 * CODE ADAPTED FROM
 *      - "[Unity] Procedural Planets (E01 the sphere)" by Sebastian Lague
 *          - https://www.youtube.com/watch?v=QN39W020LqU
 *      - "[Unity] Procedural Planets (E02 settings editor)" by Sebastian Lague
 *          - https://www.youtube.com/watch?v=LyV7cEQyZMk
 *      - "Mesh.CombineMeshes" from Unity Documentation
 *          - https://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html
 */