using UnityEngine;
using System.Collections;

public class Sensor_Prototype : MonoBehaviour {

    private int m_ColCount = 0;

    private float m_DisableTimer;

    private void OnEnable(){
        m_ColCount = 0;
    }

    public bool State(){
        /*Debug.Log("=========");
        Debug.Log(m_DisableTimer);
        Debug.Log(m_ColCount);
        Debug.Log("=========");*/
        if (m_DisableTimer > 0)
            return false;
        return m_ColCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "MapCollider")
            m_ColCount++;
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.tag == "MapCollider")
            m_ColCount--;
    }

    void Update(){
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration){
        m_DisableTimer = duration;
    }
}
