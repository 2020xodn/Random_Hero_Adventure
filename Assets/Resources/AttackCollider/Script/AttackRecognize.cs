using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRecognize : MonoBehaviour
{
    public float damage;
    

    private Animator animator;
    private void Start() {
        gameObject.name = gameObject.name.Replace("(Clone)", "");

        animator = null;
        if (gameObject.name.Contains("Slam")) {
            damage = 6.0f;

            animator = GameObject.FindWithTag("Player").GetComponentInChildren<Animator>();
        }
        else if (gameObject.name.Contains("AirNormal")) {
            damage = 4.0f;
        }
        else {
            damage = 3.0f;
        }


        damage += GameObject.FindWithTag("Player").GetComponent<PrototypeHero>().damageWeight;
        Debug.Log("damage : " + damage);



    }

    private void Update() {
        if (animator && (
            animator.GetBool("Grounded") ||
            animator.GetBool("Attack1") ||
            animator.GetBool("Attack2") ||
            animator.GetBool("UpAttack") ||
            animator.GetBool("AirAttack") ||
            animator.GetBool("AirAttackUp")
            )) {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Monster") {
            if (collision.name.Contains("Fire"))
                collision.GetComponent<FireInfo>().onAttack();
            else
                collision.GetComponent<MonsterBase>().onAttack(gameObject, damage);

            if (gameObject.name.Contains("Slam")) {
                GameObject.FindWithTag("Player").GetComponent<PrototypeHero>().invisibleDelaySlam();
            }
        }
        else if (collision.tag == "Boss") {
            collision.GetComponent<Abomination>().onAttack(gameObject, damage);
        }



        else if (collision.tag == "Switch") {
            Debug.Log("½ºÀ§Ä¡");
            SwitchOnStage sw = collision.GetComponent<SwitchOnStage>();
            sw.TriggerSwitch();

            // Time.timeScale = 0;

        }
    }
}
