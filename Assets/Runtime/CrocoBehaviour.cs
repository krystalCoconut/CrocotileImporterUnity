using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocoBehaviour : MonoBehaviour
{
    
    // Store the prefabs custom data, key is prefab name, inner dictionary is uuid keyed 
    public static Dictionary<string,Dictionary<string,CrocotileData.CustomData>> uuidTocustomData;
    // Store the local instances custom data overrides
    public Dictionary<string,CrocotileData.CustomData> nameTocustomData;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    public void Init(string prefabName,List<CrocotileData.CustomData> propertiesCustom, GameObject go)
    {
        // Create the Custom Data Dictionary 
        // Add the component this is to the object
        if (uuidTocustomData[prefabName].Count > 0)
        {
            if (uuidTocustomData[prefabName]["componentName"] != null)
            {
                string componentName = uuidTocustomData[prefabName]["componentName"].value;
                Debug.Log($"Add Component: {componentName} to instance of {prefabName}");
                Type componentType = Type.GetType("Runtime."+componentName);
                if (componentType != null && componentType.BaseType == typeof(MonoBehaviour))
                {
                    Component component = go.AddComponent(componentType);    
                }
                else
                {
                    Debug.Log($"Runtime.{componentName} does not exist. Please make the class before trying to add it");
                }
                
                
            }
                
            
        }
            
        
        // Check if there is a componentName attached to this 

    }

    public static void InitDictionary()
    {
        uuidTocustomData = new Dictionary<string, Dictionary<string, CrocotileData.CustomData>>();
    }
}
