using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    Vector3 playerPosition;
    public float cameraSpeed = 2.0f;

    void Update(){
        if (player != null) {
            playerPosition.Set(player.transform.position.x, player.transform.position.y + 1.0f, this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, playerPosition, cameraSpeed * Time.deltaTime);
        }
    }
}
