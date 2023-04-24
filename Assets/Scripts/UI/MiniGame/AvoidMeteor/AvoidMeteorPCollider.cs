using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidMeteorPCollider : MonoBehaviour
{
    List<GameObject> collMeteors;
    MiniGameAvoidMeteor am;

    private void Start() {
        collMeteors = new List<GameObject>();
        am = GameObject.Find("MiniGame_AvoidMeteor").GetComponent<MiniGameAvoidMeteor>();
    }

    void Update()
    {
        if (am.isRunning) {
            Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, transform.localScale * 0.8f, 1);

            collMeteors.RemoveAll(p => p == null);
            for (int idx = 0; idx < colls.Length; idx++) {
                if (colls[idx].tag == "Meteor") {
                    if (collMeteors.Contains(colls[idx].gameObject)) {
                        Debug.Log("Ãæµ¹ : " + colls[idx].name);

                        colls[idx].GetComponent<Animator>().SetBool("IsExploding", true);
                        colls[idx].GetComponent<AudioSource>().Play();

                        am.GameOver();
                    }
                    else {
                        collMeteors.Add(colls[idx].gameObject);
                    }
                    break;
                }
            }
        }
    }
}
