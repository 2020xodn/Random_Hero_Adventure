using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

    public GameObject gameOverWindowchunk;
    [HideInInspector]
    public bool isWindowZero = false;

    [HideInInspector]
    public Button btnToMain;
    [HideInInspector]
    public Button btnReStartByMoney;
    TextMeshProUGUI txtHaveMoney;

    [HideInInspector]
    public Button btnReStartByAdv;

    // SpawnPoint
    public GameObject spawnPoint;

    // Other Script
    PrototypeHero player;
    GameManager gameManager;

    public LavaCaveBossManager lavaManager = null;

    void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<PrototypeHero>();
        gameManager = GameObject.Find("MainManager").GetComponent<GameManager>();

        btnToMain = gameOverWindowchunk.transform.GetChild(2).GetComponent<Button>();
        btnReStartByMoney = gameOverWindowchunk.transform.GetChild(3).GetComponent<Button>();
        btnReStartByAdv = gameOverWindowchunk.transform.GetChild(4).GetComponent<Button>();
        txtHaveMoney = gameOverWindowchunk.transform.GetChild(5).GetComponent<TextMeshProUGUI>();

        btnToMain.onClick.AddListener(() => clickToMain());
        btnReStartByMoney.onClick.AddListener(() => clickReStartByMoney());
        btnReStartByAdv.onClick.AddListener(() => clickReStartByAdv());

        txtHaveMoney.text = GameManager.gameData.goldAmount + "";
    }

    public void MoveWindowToCenter() {
        isWindowZero = true;
        gameOverWindowchunk.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

    }

    void MoveWindowSomewehre() {
        isWindowZero = false;
        gameOverWindowchunk.GetComponent<RectTransform>().anchoredPosition = new Vector3(-2000, -5000, 0);
    }

    void clickToMain() {
        Debug.Log(SceneManager.GetActiveScene().buildIndex +    "메인으로 이동");
        SceneManager.LoadScene(0);
        
    }

    void clickReStartByMoney() {
        if (GameManager.gameData.goldAmount >= 20) {
            GameManager.gameData.goldAmount -= 20;
            player.RespawnHero();


            MoveWindowSomewehre();
            isWindowZero = false;

            if (lavaManager != null) {
                lavaManager.backWall();
            }
        }
        else {
            Debug.Log(SceneManager.GetActiveScene().buildIndex + "메인으로 이동");
            SceneManager.LoadScene(0);
        }

    }

    void clickReStartByAdv() {
        // 광고 ON

        player.RespawnHero();
        MoveWindowSomewehre();
        isWindowZero = false;
    }
}
