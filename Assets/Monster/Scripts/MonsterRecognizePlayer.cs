using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRecognizePlayer : MonoBehaviour
{
    MonsterBase monster;

    
    private void Start() {
        monster = transform.parent.GetComponent<MonsterBase>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            monster.traceTarget = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Player") {
            monster.isTracing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            monster.traceTarget = null;
            monster.isTracing = false;
        }
    }
}
