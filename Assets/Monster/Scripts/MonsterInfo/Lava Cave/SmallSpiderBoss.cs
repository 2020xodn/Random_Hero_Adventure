using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSpiderBoss : MonsterBase {
    Animator animator;

    // Move
    string direction = "Left";
    IEnumerator toggleCoroutine;


    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if (currentHP > 0)
            MoveToPlayer();
    }

    public override void MoveToPlayer() {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";

        if (isTracing && toggleCoroutine != null) {
            StopCoroutine(toggleCoroutine);
            Vector3 playerPos = traceTarget.transform.position;

            if (playerPos.x < transform.position.x) {
                dist = "Left";
            }
            else if (playerPos.x > transform.position.x) {
                dist = "Right";
            }
        }
        else {
            if (toggleCoroutine == null) {
                toggleCoroutine = toggleMoveIdleDirection();
                StartCoroutine(toggleCoroutine);
            }

            dist = direction;
        }

        if (dist == "Left") {
            moveVelocity = Vector3.left;
            sp.flipX = false;
        }
        else if (dist == "Right") {
            moveVelocity = Vector3.right;
            sp.flipX = true;
        }

        transform.position += moveVelocity * patrolSpeed * Time.deltaTime;
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("플레이어 인");
        }
    }

    IEnumerator toggleMoveIdleDirection() {
        yield return new WaitForSeconds(2.0f);

        if (direction == "Left")
            direction = "Right";
        else
            direction = "Left";
    }
}
