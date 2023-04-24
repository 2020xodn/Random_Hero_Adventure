using DataInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour {
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }


    #region Content
    public static GameData GameData;

    Joystick _joystick;
    PrototypeHero _player;
    public static Joystick Joystick { get { if (Instance._joystick == null) FindJoyStick(); return Instance._joystick; } }
    public static PrototypeHero Player { get { if (Instance._player == null) FindPlayer(); return Instance._player; }}
    #endregion

    #region Core
    MiniGameManager _minigame = new MiniGameManager();
    DataManager _data = new DataManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    UIManager _ui = new UIManager();

    // TODO 0 -> signal, 1 -> Avoid, in game mode = 0;
    public static MiniGameManager Mini { get { return Instance._minigame; } }

    public static DataManager Data { get { return Instance._data; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static UIManager UI { get { return Instance._ui; } }
    #endregion


    void Start()
    {
        Init();
    }

    static void Init() {
        if (s_instance == null) {
            GameObject go = GameObject.Find("@Managers");
            if (go == null) {
                go = new GameObject { name = "@Managers"};
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            Pool.Init();
            Mini.Init();

            // TODO


            // Save Data
            if (GameData == null) {
                GameData = Data.Load();
                GameData.gameProgress = 5; // For Test
                GameData.goldAmount = 50;
                GameData.item1 = false;
            }
        }
        
    }

    public static void Clear() {
        // Each Manager Clear
        Pool.Clear();
        Scene.Clear();
    }


    // TODO Move to UI Manager
    public static void FindJoyStick() {
        GameObject js = GameObject.FindWithTag("JoyStick");
        if (js != null) {
            Instance._joystick = GameObject.FindWithTag("JoyStick").GetComponent<Joystick>();
        }
        
        if (Instance._joystick == null) { 
            GameObject go = Managers.Resource.Instantiate("UI/UI_Joystick");
            go.name = "UI_Joystick";
            go.GetComponent<Canvas>().worldCamera = Camera.main;
            go.transform.SetParent(Managers.UI.Root.transform);

            Instance._joystick = go.GetComponentInChildren<Joystick>();
        }
    }

    public static void FindPlayer() { 
        if (Instance._player == null)
            Instance._player = GameObject.FindWithTag("Player").GetComponent<PrototypeHero>();
    }
}
