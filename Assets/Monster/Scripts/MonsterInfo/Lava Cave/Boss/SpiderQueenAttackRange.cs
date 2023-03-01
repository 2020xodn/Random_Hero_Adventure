using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderQueenAttackRange : MonoBehaviour
{
    Abomination boss;
    void Start(){
        boss = transform.parent.GetComponent<Abomination>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Player" && boss.canAttack && !boss.isDead) {
            //Debug.Log("공격가능!");
            boss.AttackForward();
        }
    }
}
