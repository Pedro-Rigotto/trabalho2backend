using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class SaveManager : MonoBehaviour
{
    public GameObject cube1;
    public GameObject cube2;
    public GameObject sphere1;
    public GameObject sphere2;

    string path;

    void Start()
    {
        path = Application.persistentDataPath + "/save.json";
        Debug.Log("Save Path: " + path);
    }

    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        } else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            Save();
        } else if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine("LoadOnline");
        } else if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Reset();
        }
    }

    void Save()
    {
        Cube[] cubes = FindObjectsByType<Cube>(FindObjectsSortMode.None);
        Sphere[] spheres = FindObjectsByType<Sphere>(FindObjectsSortMode.None);
        List<Adapter.CubeData> cubeDataList = new List<Adapter.CubeData>();
        List<Adapter.SphereData> sphereDataList = new List<Adapter.SphereData>();
        foreach (var cube in cubes)
        {
            var cubeData = Adapter.GetCubeData(cube);
            cubeDataList.Add(cubeData);
        }
        foreach (var sphere in spheres)
        {
            var sphereData = Adapter.GetSphereData(sphere);
            sphereDataList.Add(sphereData);
        }
        Adapter.SceneData sceneData = new Adapter.SceneData();
        sceneData.cubes = cubeDataList.ToArray();
        sceneData.spheres = sphereDataList.ToArray();
        string json = JsonUtility.ToJson(sceneData);
        System.IO.File.WriteAllText(path, json);
    }

    void Load()
    {
        if (!System.IO.File.Exists(path))
        {
            Debug.LogWarning("Save file not found!");
            return;
        }

        string json = System.IO.File.ReadAllText(path);
        Adapter.SceneData sceneData = JsonUtility.FromJson<Adapter.SceneData>(json);
        cube1.transform.position = sceneData.cubes[0].position;
        cube1.transform.localScale = sceneData.cubes[0].scale;
        cube2.transform.position = sceneData.cubes[1].position;
        cube2.transform.localScale = sceneData.cubes[1].scale;
        sphere1.transform.position = sceneData.spheres[0].position;
        sphere2.transform.position = sceneData.spheres[1].position;
    }

    void Reset()
    {
        cube1.transform.position = new Vector3(0, 0, 0);
        cube1.transform.localScale = new Vector3(0, 0, 0);
        cube2.transform.position = new Vector3(0, 0, 0);
        cube2.transform.localScale = new Vector3(0, 0, 0);
        sphere1.transform.position = new Vector3(0.095f, 0, 0.155f);
        sphere2.transform.position = new Vector3(0, 0, 0.148f);
    }

    System.Collections.IEnumerator LoadOnline()
    {
        string url = "https://raw.githubusercontent.com/Pedro-Rigotto/trabalho2backend/refs/heads/main/save.json";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Adapter.SceneData sceneData = JsonUtility.FromJson<Adapter.SceneData>(json);
            cube1.transform.position = sceneData.cubes[0].position;
            cube1.transform.localScale = sceneData.cubes[0].scale;
            cube2.transform.position = sceneData.cubes[1].position;
            cube2.transform.localScale = sceneData.cubes[1].scale;
            sphere1.transform.position = sceneData.spheres[0].position;
            sphere2.transform.position = sceneData.spheres[1].position;
            Debug.Log("Online data loaded successfully from url:\n" + url);
        }
        else
        {
            Debug.LogError("Failed to load online data: " + request.error);
        }
    }
}