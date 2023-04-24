using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameAvoidMeteor : MiniGameBase
{
    enum RectTransforms{ 
        Player
    }

    [SerializeField]
    RectTransform _player;
    SpriteRenderer _pSR;

    float playerSpeed = 3.0f;

    // meteor
    [SerializeField]
    GameObject[] meteorPrefabs;

    List<(GameObject, float)> meteors;

    float speedbyLevel;
    Vector3 centerPos;


    // Joystick


    public override void Init() {
        base.Init();

        Bind<RectTransform>(typeof(RectTransforms));
        _player = Get<RectTransform>((int)RectTransforms.Player);


        _pSR = _player.GetComponentInChildren<SpriteRenderer>();
        _pSR.flipX = false;

        meteors = new List<(GameObject, float)>();

        meteorPrefabs = new GameObject[3];
        meteorPrefabs[0] = Managers.Resource.Load<GameObject>("Prefabs/MiniGame/Avoid Meteor/Meteor1");
        meteorPrefabs[1] = Managers.Resource.Load<GameObject>("Prefabs/MiniGame/Avoid Meteor/Meteor2");
        meteorPrefabs[2] = Managers.Resource.Load<GameObject>("Prefabs/MiniGame/Avoid Meteor/Meteor3");

        // 레벨 조정
        // timeFull = 8.0f + (INITIAL_LEVEL + levelAdjust) * 3;
        timeFull = time + (INITIAL_LEVEL + levelAdjust) * 3;
        speedbyLevel = (INITIAL_LEVEL + levelAdjust - 3) * 0.01f;

        ClearEverything();

        centerPos = gameObject.transform.GetChild(0).position;
    }

    protected override void SetJoyStick() {
        base.SetJoyStick();

        joystick_Stick.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500, -200, 0);

        // Sorting Layer
        if (Managers.Scene.CurrentScene.SceneType != Define.Scene.StageSelect)
            joystick.transform.parent.parent.GetComponent<Canvas>().sortingOrder = 4;

    }

    void Update(){
        if (isRunning) {
            // 타이머
            timeElapsed += Time.unscaledDeltaTime;
            setTimer();

            // 플레이어 이동
            Vector2 inputKeyboard = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            inputKeyboard = joystick.Direction;
            inputKeyboard *= playerSpeed; // 속도 가중치
            if (inputKeyboard.x > 0 && _pSR.flipX) {
                _pSR.flipX = false;
            }
            else if (inputKeyboard.x < 0 && !_pSR.flipX) {
                _pSR.flipX = true;
            }
            if (_player.anchoredPosition.x + inputKeyboard.x > -850 &&
                _player.anchoredPosition.x + inputKeyboard.x <  850 &&
                _player.anchoredPosition.y + inputKeyboard.y > -350 &&
                _player.anchoredPosition.y + inputKeyboard.y <  450 
                ){
                _player.anchoredPosition = _player.anchoredPosition + inputKeyboard;
            }

            // 운석 생성
            int maxMeteor = (int)((timeElapsed - 5.0) % 1.5) + 15;
            if (meteors.Count < maxMeteor) {
                int random = Random.Range(0, meteorPrefabs.Length);
                float angle = Random.Range(0f, 360f);
                Vector3 targetPos = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up * 10f;

                targetPos = new Vector3(targetPos.x + centerPos.x,
                                        targetPos.y + centerPos.y,
                                        targetPos.z);
                GameObject obj = Managers.Resource.Instantiate(meteorPrefabs[random], targetPos, Quaternion.identity);

                obj.transform.SetParent(gameObject.transform);

                float randomSize = Random.Range(50.0f, 80.0f);
                randomSize = 50.0f;
                obj.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
                
                obj.transform.LookAt(_player);

                // 방향 벡터 조절
                Vector3 recoilAmount = new Vector3(0, 0, 0);
                recoilAmount.x = Random.Range(-2.0f, 2.0f);
                recoilAmount.y = Random.Range(-2.0f, 2.0f);
                obj.transform.eulerAngles += recoilAmount;

                float speed = 0.0f;
                if (Managers.Scene.CurrentScene.SceneType == Define.Scene.StageSelect) {
                    speed = Random.Range(0.2f, 0.4f) + speedbyLevel;
                }
                else {
                    speed = Random.Range(0.15f, 0.35f) + speedbyLevel;
                }

                speed *= 0.2f;

                meteors.Add((obj, speed));
            }

            meteors.RemoveAll(p => p.Item1 == null);

            foreach ((GameObject, float) meteor in meteors) {
                meteor.Item1.transform.Translate(meteor.Item1.transform.forward * meteor.Item2);
            }
        }
    }
    public override void GameStart() {
        base.GameStart();

        ClearEverything();

        SetJoyStick();
    }

    public override void ClearEverything() {
        base.ClearEverything();

        if (meteors != null) {
            foreach ((GameObject, float) meteor in meteors) {
                Destroy(meteor.Item1);
            }
            meteors.RemoveRange(0, meteors.Count);
        }
        
        if (_player != null)
            _player.anchoredPosition = new Vector3(-150, 0, 0);
        centerPos = gameObject.transform.GetChild(0).position;

    }
}
