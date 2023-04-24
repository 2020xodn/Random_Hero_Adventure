using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBoom : MonoBehaviour
{
    Tilemap tilemap;
    BoxCollider2D boxCollider2D;
    public GameObject[] ColliderToActivate;
    public GameObject ground;
    void Start()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Boom() {
        int sizeX = (int)boxCollider2D.size.x / 2;
        int sizeY = (int)boxCollider2D.size.y / 2;

        Vector3Int cellPosition = new Vector3Int();
        for (int i = -sizeX; i < sizeX; i++) {
            for (int j = -sizeY; j < sizeY; j++) {
                cellPosition = tilemap.WorldToCell(transform.position + new Vector3(i, j, 0));
                tilemap.SetTile(cellPosition, null);
            }
        }

        for (int i = 0; i < ColliderToActivate.Length; i++) {
            ColliderToActivate[i].SetActive(true);
        }

        Destroy(ground);
        Destroy(gameObject);
    }
}
