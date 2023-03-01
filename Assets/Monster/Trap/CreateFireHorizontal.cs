using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFireHorizontal : MonoBehaviour
{
    public GameObject Areas;
    Transform[] HorizontalAreas;
    bool isRunning = false;
    public float delayTime = 1.0f;

    Transform playerTransform;

    GameObject fireObj;
    public float fireSpeed = 20.0f;

    Coroutine coroutine;

    private void Start() {
        HorizontalAreas = new Transform[Areas.transform.childCount];

        for (int idx = 0; idx < Areas.transform.childCount; idx++) {
            HorizontalAreas[idx] = Areas.transform.GetChild(idx);
            //Debug.Log(HorizontalAreas[idx].name);
        }

        fireObj = Resources.Load<GameObject>("Trap/Fire Horizontal");
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
            //Debug.Log("º“»Ø");
            //GameObject fire = Instantiate(fireObj, getRandomArea());
            GameObject fire = Instantiate(fireObj, getRandomArea(), fireObj.transform.rotation);
            //fire.GetComponent<Rigidbody2D>().AddForce(Vector2.left * fireSpeed * Time.deltaTime * 1000, ForceMode2D.Force);
            fire.GetComponent<Rigidbody2D>().AddForce(Vector2.left * fireSpeed, ForceMode2D.Force);
            GetComponent<AudioSource>().Play();
            //Debug.Log(Vector2.left * fireSpeed * Time.deltaTime);
            //Debug.Log(fire);
            yield return new WaitForSeconds(delayTime);
            
        }
    }

    Vector3 getRandomArea() {
        Vector3 area = Vector3.zero;
        area = HorizontalAreas[Random.Range(0, HorizontalAreas.Length)].position;

        area = new Vector3(area.x, area.y, 0);
        return area;
    }
}
