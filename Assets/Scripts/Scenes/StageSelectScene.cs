using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataInfo;

public class StageSelectScene : BaseScene {
    protected override void Init() {
        base.Init();

        SceneType = Define.Scene.StageSelect;
    }

    public override void Clear() {
        // TODO
    }
}