using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameSignalConnect : MiniGameBase
{
    GameObject[] Lights; // R G B Y
    int[] answerNum;
    GameObject[] userAnswer;
    GameObject[] answer;
    int progress = 0;

    
    private void Start() {
        Init();
    }

    public override void Init() {
        base.Init();
        breakAnswer();

        Lights = new GameObject[4];
        answerNum = new int[6];
        userAnswer = new GameObject[6];
        answer = new GameObject[6];
        
        Lights[0] = Managers.Resource.Load<GameObject>("Prefabs/MiniGame/Signal Connect/Red Light");
        Lights[1] = Managers.Resource.Load<GameObject>("Prefabs/MiniGame/Signal Connect/Green Light");
        Lights[2] = Managers.Resource.Load<GameObject>("Prefabs/MiniGame/Signal Connect/Blue Light");
        Lights[3] = Managers.Resource.Load<GameObject>("Prefabs/MiniGame/Signal Connect/Yellow Light");
    }


    public override void GameStart() {
        base.GameStart();

        breakAnswer();
        CreateQuestion();
    }

    GameObject CreateLight(int num, Vector3 pos) {
        GameObject L = Instantiate(Lights[num], pos, Lights[num].transform.rotation);
        L.transform.SetParent(transform);
        L.transform.localScale = new Vector3(2f, 2f, 2f);
        L.transform.localPosition = pos;
        
        return L;
    }

    void CreateQuestion() {
        progress = 0;
        for (int idx = 0; idx < 6; idx++) {
            int r = Random.Range(0, Lights.Length);
            answerNum[idx] = r;
            answer[idx] = CreateLight(r, new Vector2(-500f + 200 * idx, 220f));
        }
    }

    public void clickSignalRedBtn() {
        CreateUserAnswer(0);
    }

    public void clickSignalGreenBtn() {
        CreateUserAnswer(1);
    }

    public void clickSignalBlueBtn() {
        CreateUserAnswer(2);
    }

    public void clickSignalYellowBtn() {
        CreateUserAnswer(3);
    }

    public void CreateUserAnswer(int signalNumber) {
        if (isRunning) {
            Managers.Mini.isMiniGameRunning = false;
            if (answerNum[progress] == signalNumber) {
                userAnswer[progress] = CreateLight(signalNumber, new Vector2(-500f + 200 * progress, 0));
                progress++;
                checkGameClear();
            }
            else
                breakUserAnswer();

        }
    }

    public void breakUserAnswer() {
        for (int idx = 0; idx < progress; idx++) {
            Destroy(userAnswer[idx]);
        }
        progress = 0;
    }

    public void breakAnswer() {
        if (answerNum != null) {
            for (int idx = 0; idx < 6; idx++) {
                Destroy(answer[idx]);
                answerNum[idx] = -1;
            }
        }
        breakUserAnswer();
    }

    public void checkGameClear() {
        if (progress == answerNum.Length) {
            GameClear();
            
        }
    }

    public override void ClearEverything() {
        base.ClearEverything();
        breakAnswer();
    }


}
