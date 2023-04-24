using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    // UI_Scene _sceneUI = null;

    public GameObject Root {
        get {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }

    // TODO
    // MakeSubItem

    // TODO
    public void ShowUI(string path) {
        GameObject go = Managers.Resource.Instantiate($"UI/{path}");
        if (go == null) return;

        string name = path;

        int index = name.LastIndexOf("/");
        if (index >= 0)
            name = name.Substring(index + 1);

        go.name = name;
        go.GetComponent<Canvas>().worldCamera = Camera.main;
        go.transform.SetParent(Root.transform);
    }

    public void Clear() {
        // TODO

    }
}
