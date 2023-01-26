using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

//https://docs.unity3d.com/Manual/ScriptedImporters.html
[ScriptedImporter(1, "crocotile")]
public class CrocotileImporter : ScriptedImporter
{
    public CrocotileData data;
    public string texString;
    public Material mainMaterial;
    public override void OnImportAsset(AssetImportContext ctx)
    {
        // Parse Filename
        string name = ctx.assetPath;
        // TODO actually parse it

        // Load Icon from file
        Texture2D crocodileIcon =
            AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/CrocotileImporter/funnyicon.png");

        data = JsonConvert.DeserializeObject<CrocotileData>(File.ReadAllText(ctx.assetPath));

        
        // Data
        
        GameObject parent = new GameObject(name);
        ctx.AddObjectToAsset("main obj",parent);
        ctx.SetMainObject(parent);
        
        Material mat = new Material(Shader.Find("Unlit/Transparent Cutout"));
        mat.name = "Main Material";
        ctx.AddObjectToAsset("material",mat);
        MeshFilter mf;
        mf = parent.AddComponent<MeshFilter>();
        
        ctx.AddObjectToAsset("meshFilter",mf);
        MeshRenderer mr;
        mr = parent.AddComponent<MeshRenderer>();
        ctx.AddObjectToAsset("renderer",mr);
        MeshCollider mc;
        mc = parent.AddComponent<MeshCollider>();
        ctx.AddObjectToAsset("collider",mc);
        
        Texture2D spriteSheet = new Texture2D(1, 1);
        // Read tex
        string strippedB64Image = data.model[0].texture.Replace("data:image/png;base64,", "");
        byte[] imageData = System.Convert.FromBase64String(strippedB64Image);
        spriteSheet.filterMode = FilterMode.Point;
        spriteSheet.LoadImage(imageData);
        ctx.AddObjectToAsset("Texture",spriteSheet);
        //ctx.AddObjectToAsset("Renderer",mr);
        //ctx.AddObjectToAsset("Object",gMesh);



        //DestroyImmediate(GameObject.Find("parent"));
        //GameObject parent = new GameObject("parent");
        
        // Create Mesh
        
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        
        Mesh mesh = new Mesh();
        int i = 0;
        foreach (CrocotileData.Model model in data.model)
            foreach (CrocotileData.Object obj in model.@object)
            {
                

            Vector3 pos = new Vector3((float)obj.position.x, (float)obj.position.y, -(float)obj.position.z);
            
            // VERTICIES
            vertices.Add( new Vector3(obj.vertices[0].x, obj.vertices[0].y,-obj.vertices[0].z) + pos );
            vertices.Add( new Vector3(obj.vertices[1].x, obj.vertices[1].y,-obj.vertices[1].z) + pos );
            vertices.Add( new Vector3(obj.vertices[2].x, obj.vertices[2].y,-obj.vertices[2].z) + pos );
            vertices.Add( new Vector3(obj.vertices[3].x, obj.vertices[3].y,-obj.vertices[3].z) + pos );

            // Create the tris  
            triangles.Add(obj.faces[0][2]+ i * 4);
            triangles.Add(obj.faces[0][1]+ i * 4);
            triangles.Add(obj.faces[0][0]+ i * 4);
            triangles.Add(obj.faces[1][2]+ i * 4);
            triangles.Add(obj.faces[1][1]+ i * 4);
            triangles.Add(obj.faces[1][0]+ i * 4);
            
            
            
            // Create the UVs
            uvs.Add(new Vector2(obj.uvs[0][0].x, obj.uvs[0][0].y));
            uvs.Add(new Vector2(obj.uvs[0][2].x, obj.uvs[0][2].y));
            uvs.Add(new Vector2(obj.uvs[0][1].x, obj.uvs[0][1].y));
            uvs.Add(new Vector2(obj.uvs[1][1].x, obj.uvs[1][1].y));
            
            i++;
            }
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

            
        mesh.RecalculateNormals();
            
        // Make my mesh Game Object
        // Give it a mesh
        
        mf.mesh = mesh;
        mc.sharedMesh = mesh;
        mesh.name = "Main Mesh";
        ctx.AddObjectToAsset("mesh",mesh);
        
        // Save to 
        // Give it a renderer
//        ctx.AddObjectToAsset("Mesh",mf);
        
        //g_Mesh.transform.SetParent(parent.transform);
        // Give it a material
        mr.sharedMaterial = mat;
        mr.sharedMaterial.SetTexture("_MainTex",spriteSheet);
        
        
        
        
        List<Mesh> meshes = new List<Mesh>();
        

        //Create Prefabs 


        Mesh currentMesh;


        CrocoBehaviour.InitDictionary();
         
         int j = 0;
         foreach (CrocotileData.Prefab prefab in data.prefabs)
         {
             if (prefab.@object == null) continue;
             Debug.Log($"prefabs {prefab.name} has {prefab.@object.Count} objects");

             Dictionary<string, CrocotileData.CustomData> prefabCustomData =
                 new Dictionary<string, CrocotileData.CustomData>();
             
             // Custom Data
             foreach (var customData in prefab.properties.custom)
             {                  
                 prefabCustomData.Add(customData.name,customData);      
             }
             CrocoBehaviour.uuidTocustomData.Add(prefab.name,prefabCustomData);
             
             // Mesh Cleanup 
             currentMesh = new Mesh();
             currentMesh.Clear();
             vertices.Clear();
             triangles.Clear();
             uvs.Clear();
             i = 0;
             foreach (var obj in prefab.@object)
             {
                 
                 Debug.Log($"\tBuilding mesh for object {i}");
                 Vector3 pos = new Vector3((float)obj.position.x, (float)obj.position.y, -(float)obj.position.z);
        
                 // VERTICIES
                 vertices.Add( new Vector3(obj.vertices[0].x, obj.vertices[0].y,-obj.vertices[0].z) + pos );
                 vertices.Add( new Vector3(obj.vertices[1].x, obj.vertices[1].y,-obj.vertices[1].z) + pos );
                 vertices.Add( new Vector3(obj.vertices[2].x, obj.vertices[2].y,-obj.vertices[2].z) + pos );
                 vertices.Add( new Vector3(obj.vertices[3].x, obj.vertices[3].y,-obj.vertices[3].z) + pos );
        
                 // Create the tris  
                 triangles.Add(obj.faces[0][2]+ i * 4);
                 triangles.Add(obj.faces[0][1]+ i * 4);
                 triangles.Add(obj.faces[0][0]+ i * 4);
                 triangles.Add(obj.faces[1][2]+ i * 4);
                 triangles.Add(obj.faces[1][1]+ i * 4);
                 triangles.Add(obj.faces[1][0]+ i * 4);
        
                 // Create the UVs
                 uvs.Add(new Vector2(obj.uvs[0][0].x, obj.uvs[0][0].y));
                 uvs.Add(new Vector2(obj.uvs[0][2].x, obj.uvs[0][2].y));
                 uvs.Add(new Vector2(obj.uvs[0][1].x, obj.uvs[0][1].y));
                 uvs.Add(new Vector2(obj.uvs[1][1].x, obj.uvs[1][1].y));

                 i++;
             }
        
             currentMesh.vertices = vertices.ToArray();
             currentMesh.triangles = triangles.ToArray();
             currentMesh.uv = uvs.ToArray();
             currentMesh.name = prefab.name;
        
             currentMesh.RecalculateNormals();
             Debug.Log($"Add object: {prefab.name} to asset");

             // Make my mesh Game Object
             GameObject g_Mesh = new GameObject(prefab.name);
             
             // Give it a mesh
             mf = g_Mesh.AddComponent<MeshFilter>();
             
             mf.sharedMesh = currentMesh;
             // Give it a renderer
             
             mr = g_Mesh.AddComponent<MeshRenderer>();
             
             g_Mesh.transform.SetParent(parent.transform);
             // Give it a material
             Material m = new Material(Shader.Find("Unlit/Transparent Cutout"));
             mr.sharedMaterial = m;
             
             mr.sharedMaterial.SetTexture("_MainTex", spriteSheet);
             ctx.AddObjectToAsset("childMaterial"+i,m);
             Debug.Log($"Prefab has {prefab.instances.Count} instances");
             foreach (var instance in prefab.instances)
             {
                 //Debug.Log($"Prefab has {prefabs.instances.Count} instances");
                 GameObject tmp = Instantiate(g_Mesh);
                 tmp.name = g_Mesh.name;
                 tmp.transform.position = new Vector3((float)instance.position.x, (float)instance.position.y, -(float)instance.position.z);
                 tmp.transform.eulerAngles = new Vector3(instance.rotation._x, instance.rotation._y, instance.rotation._z);
                 tmp.transform.localScale = new Vector3(instance.scale.x, instance.scale.y, instance.scale.z);
                 CrocoBehaviour crocoData = tmp.AddComponent<CrocoBehaviour>();
                 crocoData.Init(prefab.name,instance.properties.custom,tmp);
                 tmp.transform.SetParent(parent.transform);
             }
             DestroyImmediate(g_Mesh);
             meshes.Add(currentMesh);

             if (prefab.type == "object")
             {
                 ctx.AddObjectToAsset(prefab.num,meshes[j]);
                 j++;
             }
             Debug.Log($"Contents of Asset Import Context: ");
             printContents(ctx);

         }
         
    }
    private void printContents(AssetImportContext aic)
    {
        List<UnityEngine.Object> test = new List<UnityEngine.Object>();
        aic.GetObjects(test);
        foreach (var obj in test)
        {
            Debug.Log(obj.ToString());
        }
    }

}





[CustomEditor(typeof(CrocotileImporter))]
public class CrocotileEditor: ScriptedImporterEditor
{
   
    private SerializedProperty _data;
    private SerializedProperty _MainMaterial;

    public enum ColliderType
    {
        BOX,MESH
    }

    private ColliderType _colliderType = ColliderType.MESH;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Crocotile Importer");
        
        _data = serializedObject.FindProperty("texString");
        _MainMaterial = serializedObject.FindProperty("mainMaterial");
        string strippedB64Image = _data.stringValue.Replace("data:image/png;base64,", "");
        byte[] imageData = System.Convert.FromBase64String(strippedB64Image);
        
        Texture2D spriteSheet = new Texture2D(1, 1);
        spriteSheet.LoadImage(imageData);
        GUILayout.Box(spriteSheet);
        var config = new GUIContent("data");
        EditorGUILayout.ObjectField(_MainMaterial.objectReferenceValue, typeof(Material));
        
        // if (((CrocotileImporter) serializedObject.targetObject).data != null)
        //     ;
        //
        // EditorGUILayout.PropertyField(prop, colorShift);
        //EditorGUILayout.PropertyField(prop, config);
        base.ApplyRevertGUI();
    }
}