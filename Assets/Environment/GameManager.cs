using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DataInfo;

public class GameManager : MonoBehaviour
{
    SelectCharacterMove charMove;

    int firstStageSceneWeight = 1;

    // 스테이지 변경
    public TextMeshProUGUI currentStageNameTMP;
    Coroutine coroutineChangeImage;

    // 스테이지 별 배경 이미지
    public SpriteRenderer[] backgroundSprites;
    SpriteRenderer currentBackround;

    // 메뉴 - 설정
    public RectTransform menuChunkSetting;
    public RectTransform menuChunkShop;
    public RectTransform menuChunkGamePractice;

    // 메뉴 - 상점
    public ShopManager shopManager;


    public static GameData gameData;

    string[] stageNameString = {
        "튜토리얼",
        "눈산",
        "사막",
        "동굴",
        "보스동굴",
        "보스(임시)"
    };
    
    private void Awake() {
    }

    void Start(){
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            charMove = GameObject.FindWithTag("Player").GetComponent<SelectCharacterMove>();

            charMove = GameObject.FindWithTag("Player").GetComponent<SelectCharacterMove>();

            ChangeStageNameText();
            ClearAllBackground();

            if (gameData == null) {
                gameData = GetComponent<DataManager>().Load();
                Debug.Log("세이브 데이터 생성");
                Debug.Log("gameProgess : " + gameData.gameProgress);
                Debug.Log("goldAmount : " + gameData.goldAmount);
            }
        }

        //if ()

            
    }

    public void MoveToEachStage(int stageNumber) {
        //Debug.Log(gameObject.name[0]);
        if (stageNumber == 0 || gameData.gameProgress >= 1) { // 이전 스테이지를 클리어 해야 이동 가능

            Debug.Log("stageNumber : " + stageNumber);
            //StopCoroutine(ChangeBackgroundTransparency());
            if (coroutineChangeImage != null)
                StopCoroutine(coroutineChangeImage);
            ClearAllBackground();

            bool hastoChange = charMove.currentSelected != stageNumber;
            charMove.currentSelected = stageNumber;

            if (hastoChange) {
                ChangeStageNameText();
                coroutineChangeImage = StartCoroutine(ChangeBackgroundTransparency());
            }
        }
        else {
            Debug.Log("이전 스테이지 클리어 필요");
        }
        
    }

    void StartStage() {
        int stageNumber = charMove.currentSelected + firstStageSceneWeight;

        
        Debug.Log("씬 로드 : " + stageNumber);
        if (stageNumber == 1) {
            SceneManager.LoadScene(1);

        }
        if (stageNumber == 5) {
            SceneManager.LoadScene(2);
        }

    }

    void ClearAllBackground() {
        Color c;
        // 이미지 하나 빼고 전부 투명 처리
        for (int idx = 0; idx < backgroundSprites.Length; idx++) {
            if (idx == charMove.currentSelected)
                continue;
            c = backgroundSprites[idx].color;
            c.a = 0;
            backgroundSprites[idx].color = c;
        }

        c = backgroundSprites[charMove.currentSelected].color;
        c.a = 1;
        backgroundSprites[charMove.currentSelected].color = c;
        currentBackround = backgroundSprites[charMove.currentSelected];
    }

    void ChangeStageNameText() {
        currentStageNameTMP.text = stageNameString[charMove.currentSelected];
    }

    IEnumerator ChangeBackgroundTransparency() {
        SpriteRenderer newBackground = backgroundSprites[charMove.currentSelected];
        Color newBackgroundColor = newBackground.color;
        Color oldBackgroundColor = currentBackround.color;

        yield return new WaitForSeconds(0.20f);
        for (int count = 0; count < 2; count++) {
            for (int i = 0; i < 50; i++) {
                newBackgroundColor.a += 0.01f;
                newBackground.color = newBackgroundColor;

                oldBackgroundColor.a -= 0.01f;
                currentBackround.color = oldBackgroundColor;
                yield return new WaitForSeconds(0.01f);
            }
        }

        newBackgroundColor.a = 1;
        newBackground.color = newBackgroundColor;

        oldBackgroundColor.a = 0;
        currentBackround.color = oldBackgroundColor;

        currentBackround = newBackground;
    }

    // UI Button
    public void clickSettingButton() {
        menuChunkSetting.position = Vector3.zero;
    }

    public void clickSettingQuitButton() {
        Debug.Log("종료버튼");
        Application.Quit();
    }

    public void clickSettingDeleteFileButton() {
        GetComponent<DataManager>().DeleteData();
        MoveToEachStage(0);
    }

    public void clickSettingBackButton() {
        menuChunkSetting.position = new Vector3(2500, 0, 0);
    }

    public void clickShopButton() {
        menuChunkShop.gameObject.GetComponent<ShopManager>().updateCoin();

        menuChunkShop.position = Vector3.zero;
        
    }

    public void clickShopBackButton() {
        menuChunkShop.position = new Vector3(2500, -1000, 0);
        shopManager.buyItemConfirmChunk.anchoredPosition = new Vector3(2500, -1000, 0);
        shopManager.buyItemAlreadyBuyConfirmChunk.anchoredPosition = new Vector3(2500, -1000, 0);
    }

    public void clickGamePracticeButton() {
        menuChunkGamePractice.position = Vector3.zero;
    }

    public void clickGamePracticeBackButton() {
        menuChunkGamePractice.position = new Vector3(4500, 1000, 0);
    }

    
}
