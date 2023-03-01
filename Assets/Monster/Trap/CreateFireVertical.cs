using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFireVertical : MonoBehaviour {
    public GameObject Areas;
    Transform[] VerticalAreas;
    bool isRunning = false;
    public float delayTime = 1.0f;

    Transform playerTransform;

    GameObject fireObj;
    public float fireSpeed = 20.0f;

    Coroutine coroutine;

    private void Start() {
        VerticalAreas = new Transform[Areas.transform.childCount];

        for (int idx = 0; idx < Areas.transform.childCount; idx++) {
            VerticalAreas[idx] = Areas.transform.GetChild(idx);
            //Debug.Log(VerticalAreas[idx].name);
        }

        fireObj = Resources.Load<GameObject>("Trap/Fire Vertical");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (!isRunning) {
                isRunning = true;
                playerTransform = collision.transform;
                coroutine = StartCoroutine(SummonHorizontalFire());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (isRunning) {
                isRunning = false;
                StopCoroutine(coroutine);
            }
        }


        if (collision.tag == "MonsterAttack") {
            Destroy(collision.gameObject);
        }
    }

    IEnumerator SummonHorizontalFire() {
        while (isRunning) {
            GameObject fire = Instantiate(fireObj, getRandomArea(), fireObj.transform.rotation);
            //fire.GetComponent<Rigidbody2D>().AddForce(Vector2.left * fireSpeed * Time.deltaTime * 1000, ForceMode2D.Force);
            fire.GetComponent<Rigidbody2D>().AddForce(Vector2.down * fireSpeed, ForceMode2D.Force);

            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(delayTime);

        }
    }

    Vector3 getRandomArea() {
        Vector3 area = Vector3.zero;
        area = VerticalAreas[Random.Range(0, VerticalAreas.Length)].position;

        area = new Vector3(area.x, area.y, 0);
        return area;
    }
}
