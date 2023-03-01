using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameAvoidMeteor : MiniGameBase
{
    // Player
    RectTransform player;
    SpriteRenderer p_SR;


    // meteor
    Transform[] meteorPrefabs;

    List<(GameObject, float)> meteors;

    float speedbyLevel;

    Vector3 centerPos;

    

    void Start()
    {
        player = gameObject.transform.Find("Player").GetComponent<RectTransform>();
        p_SR = GetComponentInChildren<SpriteRenderer>();
        p_SR.flipX = false;

        txtResult = gameObject.transform.Find("Timer Left").Find("Text Timer").GetComponent<TextMeshProUGUI>();


        meteors = new List<(GameObject, float)>();
        meteorPrefabs = new Transform[3];
        meteorPrefabs[0] = Resources.Load<Transform>("MiniGame/Avoid Meteor/Meteor1");
        meteorPrefabs[1] = Resources.Load<Transform>("MiniGame/Avoid Meteor/Meteor2");
        meteorPrefabs[2] = Resources.Load<Transform>("MiniGame/Avoid Meteor/Meteor3");

        Init();
    }

    public override void Init() {
        base.Init();

        // 레벨 조정
        // timeFull = 8.0f + (INITIAL_LEVEL + levelAdjust) * 3;
        timeFull = time + (INITIAL_LEVEL + levelAdjust) * 3;
        speedbyLevel = (INITIAL_LEVEL + levelAdjust - 3) * 0.01f;
        setTimer();

        ClearEverything();

        centerPos = gameObject.transform.GetChild(0).position;
    }

    protected override void SetJoyStick() {
        base.SetJoyStick();

        joystick_Stick.GetComponent<RectTransform>().anchoredPosition = new Vector3(-800, -200, 0);
    }

    void Update(){
        if (isRunning) {
            // 타이머
            timeElapsed += Time.unscaledDeltaTime;
            setTimer();

            /*Debug.Log(Time.timeScale);
            Time.timeScale = 0.01f;*/

            //Physics.autoSyncTransforms = true;
            //Debug.Log(player.transform.position);

            //Debug.Log(timeElapsed);
            // 플레이어 이동
            Vector2 inputKeyboard = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            inputKeyboard = joystick.Direction;

            inputKeyboard *= 8; // 속도 가중치
            if (inputKeyboard.x > 0 && p_SR.flipX) {
                p_SR.flipX = false;
            }
            else if (inputKeyboard.x < 0 && !p_SR.flipX) {
                p_SR.flipX = true;
            }
            if (player.anchoredPosition.x + inputKeyboard.x > -850 &&
                player.anchoredPosition.x + inputKeyboard.x <  850 &&
                player.anchoredPosition.y + inputKeyboard.y > -350 &&
                player.anchoredPosition.y + inputKeyboard.y <  450 
                ){
                player.anchoredPosition = player.anchoredPosition + inputKeyboard;
            }
                

            

            // 운석 생성
            int maxMeteor = (int)((timeElapsed - 5.0) % 1.5) + 15;

            if (meteors.Count < maxMeteor) {
                int random = Random.Range(0, meteorPrefabs.Length);
                float angle = Random.Range(0f, 360f);
                //Vector3 targetPos = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up * 11f;
                //Vector3 targetPos = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up * 11f + centerPos;
                Vector3 targetPos = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up * 11f;
                targetPos = new Vector3(targetPos.x + centerPos.x,
                                        targetPos.y + centerPos.y,
                                        targetPos.z);
                /*Debug.Log("centerPos : " + centerPos);
                Debug.Log("targetPos : " + targetPos);*/
                GameObject obj = Instantiate(meteorPrefabs[random], targetPos, Quaternion.identity).gameObject;
                //obj.transform.position = targetPos;
                obj.transform.SetParent(gameObject.transform);

                float randomSize = Random.Range(50.0f, 80.0f);
                randomSize = 50.0f;
                obj.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
                //obj.position = new Vector3(obj.position.x, obj.position.y, 0);
                
                obj.transform.LookAt(player);

                Vector3 recoilAmount = new Vector3(0, 0, 0);
                recoilAmount.x = Random.Range(-2.0f, 2.0f);
                recoilAmount.y = Random.Range(-2.0f, 2.0f);
                obj.transform.eulerAngles += recoilAmount;

                float speed = 0.0f;
                /*if (SceneManager.GetActiveScene().buildIndex == 0) {
                    speed = Random.Range(0.05f, 0.15f) + speedbyLevel;
                }
                else {
                    speed = Random.Range(0.03f, 0.07f) + speedbyLevel;
                }*/

                // 모바일에서는 느려서 수정함
                if (SceneManager.GetActiveScene().buildIndex == 0) {
                    speed = Random.Range(0.2f, 0.4f) + speedbyLevel;
                }
                else {
                    speed = Random.Range(0.15f, 0.35f) + speedbyLevel;
                }


                //Destroy(obj.gameObject);
                // Debug.Log("파괴!!");

                // 0.1 기준 7초
                // 0.1
                //obj.name = obj.name;
                meteors.Add((obj, speed));
            }

            //Physics.SyncTransforms();

            meteors.RemoveAll(p => p.Item1 == null);
            // Cycle through each asteroid and move it forwards
            foreach ((GameObject, float) meteor in meteors) {
                meteor.Item1.transform.Translate(meteor.Item1.transform.forward * meteor.Item2);

                //meteor.Item1.localPosition = new Vector3(meteor.Item1.localPosition.x, meteor.Item1.localPosition.y, 0);
                //meteor
            }
        }
    }
    public override void GameStart() {
        base.GameStart();

        ClearEverything();
        txtResult.text = "남은 시간";
    }

    public override void ClearEverything() {
        base.ClearEverything();

        foreach ((GameObject, float) meteor in meteors) {
            Destroy(meteor.Item1);
        }
        meteors.RemoveRange(0, meteors.Count);
        player.anchoredPosition = new Vector3(-150, 0, 0);

    }
}
