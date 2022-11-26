using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

//https://docs.unity3d.com/Manual/ScriptedImporters.html
[ScriptedImporter(1, "crocy")]
public class CrocyImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        // Parse Filename
        string name = ctx.assetPath;
        // TODO actually parse it

        // Load Icon from file
        Texture2D crocodileIcon =
            AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/CrocotileImporter/funnyicon.png");

        
        // Convert to Prefab

        GameObject crocyFab = new GameObject(name);
        ctx.AddObjectToAsset("crocy", crocyFab,crocodileIcon);
        ctx.SetMainObject(crocyFab);
        
        
        CrocyData crocy = JsonConvert.DeserializeObject<CrocyData>(File.ReadAllText(ctx.assetPath));

        // Create Prefabs 
        if (crocy != null)
            foreach (CrocyData.Object _object in crocy.objects)
            {
                GameObject fab =
                    AssetDatabase.LoadAssetAtPath<GameObject>("Assets/scenedata/" + _object.name + ".prefab");
                if (fab == null)
                {
                    GameObject originalPrefab = new GameObject(_object.name);
                    CrocoBehaviour cBehaviour = originalPrefab.AddComponent<CrocoBehaviour>();
                    cBehaviour.customData = _object.custom;
                    MeshFilter mesh = originalPrefab.AddComponent<MeshFilter>();
                    MeshRenderer mr = originalPrefab.AddComponent<MeshRenderer>();
                    mesh.sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Objects/" + _object.name + ".obj");
                    mr.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Crocotile.mat");
                    
                    fab = PrefabUtility.SaveAsPrefabAsset(originalPrefab,"Assets/scenedata/" + originalPrefab.name + ".prefab");
                }
                
                foreach (CrocyData.Instance _instance in _object.instances)
                {
                    if (fab != null)
                    {
                        GameObject child = (GameObject) PrefabUtility.InstantiatePrefab(fab, crocyFab.transform);
                        // Crocotile is Pos X is Left - Unity is Pos X is right
                        child.transform.position = new Vector3(-_instance.pos.x,_instance.pos.y,_instance.pos.z);
                        child.transform.eulerAngles = new Vector3(_instance.rot.x,_instance.rot.y,_instance.rot.z);
                        child.transform.localScale = new Vector3(_instance.sca.x,_instance.sca.y,_instance.sca.z);
                        
                    }
                }
            }
        else
            Debug.LogError("BAD CROCOFILE!");
    }
}

[Serializable]
public class CrocyData
{
    [SerializeField]
    public Object[] objects;
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    [Serializable]
    public class Billboard
    {
        public bool enabled { get; set; }
        public bool y { get; set; }
    }
    [Serializable]
    public class Instance
    {
        
        public string uuid { get; set; }
        public object parent { get; set; }
        
        public Pos pos { get; set; }
         public Rot rot { get; set; }
         public Sca sca { get; set; }
        public List<object> custom { get; set; }
        
        
    }

    
    [Serializable]
    public class Object
    {
        public string name { get; set; }
        public List<Point> points { get; set; }
        public Billboard billboard { get; set; }
        public List<object> custom { get; set; }
        public List<Instance> instances { get; set; }
    }
    [Serializable]
    public class Point
    {
        public string name { get; set; }
        public Pos pos { get; set; }
    }
    [Serializable]
    public class Pos
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }
    [Serializable]
    public class Root
    {
        public List<Object> objects { get; set; }
        // unused
        public List<object> lights { get; set; }
        public List<object> cameras { get; set; }
    }
    [Serializable]
    public class Rot
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public string order { get; set; }
    }
    [Serializable]
    public class Sca
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }


}

[CustomEditor(typeof(CrocyImporter))]
public class CrocyEditor: ScriptedImporterEditor
{
    public override void OnInspectorGUI()
    {
        // var colorShift = new GUIContent("Color Shift");
        // var prop = serializedObject.FindProperty("m_ColorShift");
        // EditorGUILayout.PropertyField(prop, colorShift);
        base.ApplyRevertGUI();
    }
}