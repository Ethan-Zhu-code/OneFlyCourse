using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class IDGenerate : MonoBehaviour {
    public EventController[] targetObjects;

    private int numberPool = 0;
    //物品ID__string=Name+Number

    public void SignObjectst()
    {
        ClearArray(targetObjects);
        targetObjects = GetAllEventControllerObjectsInScene();
        //print("All " + Resources.FindObjectsOfTypeAll<EventController>().Length);

    }

    public void RegisterObjectsID()
    {
        SignObjectst();
        CaulateID(targetObjects);
    }

    public void RegisterObjectsIDClearly()
    {
        SignObjectst();
        numberPool = 0;
        CaulateID(targetObjects);
    }



    public void ClearArray(EventController[] gameObjects)
    {
        gameObjects = new EventController[0];
    }
    public EventController[] GetAllEventControllerObjectsInScene()
    {
        List<EventController> objectsInScene = new List<EventController>();
        EventController[] objects;
        foreach (EventController go in Resources.FindObjectsOfTypeAll<EventController>())
        {

            if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
            { continue; }


            if (EditorUtility.IsPersistent(go.transform.root.gameObject))
            { continue; }


            objectsInScene.Add(go);

        }
        objects = new EventController[objectsInScene.Count];
        objectsInScene.CopyTo(objects);
        return objects;
    }

    public void CaulateID(EventController[] eventControllers)
    {
        for (int i = 0; i < eventControllers.Length; i++)
        {
            if (eventControllers[i].objectID == "")
            {
                eventControllers[i].objectID = eventControllers[i].name + numberPool.ToString("d8");
                print(numberPool.ToString("d8"));
                numberPool++;
            }

        }

    }


}
