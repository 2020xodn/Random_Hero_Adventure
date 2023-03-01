using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCaveBossManager : MonoBehaviour
{
    public bool didStart = false;
    public GameObject boss;

    public GameObject entranceWallCollider;
    public GameObject entranceRock;

    public GameObject endPortal;

    public GameObject respawnPoint;
    

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && !didStart) {
            didStart = true;
            boss.SetActive(true);
            boss.GetComponent<Abomination>().isAttacking = false;
            boss.GetComponent<Abomination>().canAttack= true;
            boss.GetComponent<Abomination>().nowMoveDelay = null;
            
            entranceWallCollider.transform.position = new Vector3(
                entranceWallCollider.transform.position.x,
                entranceWallCollider.transform.position.y - 2,
                entranceWallCollider.transform.position.z);

            entranceRock.transform.position = new Vector3(41.75f, -3f, 0);

            boss.transform.Find("Sound").Find("Generated").GetComponent<AudioSource>().Play();

            collision.GetComponent<PrototypeHero>().respawnObject = respawnPoint;
        }
    }

    public void clearStageSetPortal() {
        endPortal.SetActive(true);
    }

    public void backWall() {
        didStart = false;
        boss.SetActive(false);

        entranceWallCollider.transform.position = new Vector3(
        entranceWallCollider.transform.position.x,
        entranceWallCollider.transform.position.y + 2,
        entranceWallCollider.transform.position.z);

        entranceRock.transform.position = new Vector3(41.75f, 10f, 0);
    }
}
