using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DevelopEngine;

public class UIName
{ 
    public const string UISceneBegin = "UIScene_Begin";
    public const string UISceneHint = "UIScene_Hint";
    public const string UIScenePurpose = "UIScene_Purpose";
}
public class UIManager : MonoSingleton<UIManager>
{

    private Dictionary<string, UIScene> mUIScene = new Dictionary<string, UIScene>();
    //private Dictionary<UIAnchor.Side, GameObject> mUIAnchor = new Dictionary<UIAnchor.Side, GameObject>();
    private List<UIScene> lists = new List<UIScene>();
    public void InitializeUIs()
    {
        
        mUIScene.Clear();
        UIScene[] scenes = FindObjectsOfType<UIScene>();
        lists.AddRange(scenes);
        Object[] uis = FindObjectsOfType(typeof(UIScene));
        if (uis != null)
        {
            foreach (Object obj in uis)
            {
                UIScene ui = obj as UIScene;
                ui.SetVisible(false);
                mUIScene.Add(ui.gameObject.name, ui);
            }
        }
    }

    public void SetVisible(string name, bool visible)
    {
        if (visible && !IsVisible(name))
        {
            OpenScene(name);
        }
        else if (!visible && IsVisible(name))
        {
            CloseScene(name);
        }
    }

    public bool IsVisible(string name)
    {
        UIScene ui = GetUI(name);
        if (ui != null)
            return ui.IsVisible();
        return false;
    }
    private UIScene GetUI(string name)
    {
        UIScene ui;
        return mUIScene.TryGetValue(name, out ui) ? ui : null;
    }

    public T GetUI<T>(string name) where T : UIScene
    {
        return GetUI(name) as T;
    }

    private bool isLoaded(string name)
    {
        if (mUIScene.ContainsKey(name))
        {
            return true;
        }
        return false;
    }

    private void OpenScene(string name)
    {
        if (isLoaded(name))
        {
            mUIScene[name].SetVisible(true);
        }
    }
    private void CloseScene(string name)
    {
        if (isLoaded(name))
        {
            mUIScene[name].SetVisible(false);
        }
    }

    public List<UIScene> GetActiveScenes()
    {
        var scenes = lists.FindAll(p => p.gameObject.activeSelf);
        return scenes;
    }

    /// <summary>
    /// 获取当前UI的激活状态
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool GetVisibleByName(string name)
    {
        UIScene ui = GetUI(name);
        return ui.gameObject.activeSelf;
    }

    public void SetInitVisible(bool visible)
    {
        SetVisible(UIName.UISceneBegin, visible);
    }

}
