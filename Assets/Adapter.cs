using System;
using UnityEditor.SearchService;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class Adapter : MonoBehaviour
{
    public static void GetSphere(SphereData sphereData, ref Sphere sphere)
    {
        sphere.transform.position = sphereData.position;
    }

    public static void GetCube(CubeData cubeData, ref Cube cube)
    {
        cube.transform.position = cubeData.position;
        cube.transform.localScale = cubeData.scale;
    }

    public static CubeData GetCubeData(Cube cube)
    {
        CubeData cubeData = new CubeData();
        cubeData.position = cube.transform.position;
        cubeData.scale = cube.transform.localScale;
        return cubeData;
    }

    public static SphereData GetSphereData(Sphere sphere)
    {
        SphereData sphereData = new SphereData();
        sphereData.position = sphere.transform.position;
        return sphereData;
    }

    [Serializable]
    public class SphereData
    {
        public Vector3 position;
    }

    [Serializable]
    public class CubeData
    {
        public Vector3 position;
        public Vector3 scale;
    }

    [Serializable]
    public class SceneData
    {
        public SphereData[] spheres;
        public CubeData[] cubes;
    }
}
