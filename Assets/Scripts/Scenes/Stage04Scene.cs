using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage04Scene : BaseScene
{
    protected override void Init() {
        SceneType = Define.Scene.Stage5;

        base.Init();

        // MiniGame Instantiate
        Managers.Mini.Init();

        // UI »ý¼º TODO
        GameObject gameover = GameObject.Find("UI_GameOver");
        if (gameover == null) {
            Managers.UI.ShowUI("UI_GameOver");
        }

        GameObject status = GameObject.Find("UI_Status");
        if (status == null) {
            Managers.UI.ShowUI("UI_Status");
        }
    }

    public override void Clear() {
    }
}
