using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterController : MonoBehaviour
{
    private Animator m_animator;
    private SpriteRenderer m_SR;

    Transform[] stagePosition;
    Transform cornerPos;

    [HideInInspector]
    public int currentSelected = 0;
    [HideInInspector]
    public int previousSelected = 0;

    void Start(){
        m_animator = GetComponentInChildren<Animator>();
        m_animator.SetBool("Grounded", true);

        m_SR = GetComponentInChildren<SpriteRenderer>();
        m_SR.flipX = false;

        GameObject go = GameObject.Find("Button Stage");
        stagePosition = new Transform[go.transform.childCount - 1];
        int count = 0;
        for (int i = 0; i < go.transform.childCount; i++) {
            Transform child = go.transform.GetChild(i);
            if (child.name == "Corner")
                cornerPos = child;
            else {
                stagePosition[count++] = child;
            }
        }
        currentSelected = 0;
    }

    void Update(){
        if (currentSelected > previousSelected) {
            m_SR.flipX = false;
        }
        else if (currentSelected < previousSelected) {
            m_SR.flipX = true;
        }

        if ((currentSelected <= 1 && previousSelected >= 2) || (currentSelected >= 2 && previousSelected <= 1)) {
            ViaCornerPos();
        }
        else {
            MoveToSelectPos();
        }
    }

    void MoveToSelectPos() {
        previousSelected = currentSelected;


        Vector3 targetPos = new Vector3(stagePosition[currentSelected].position.x,
                                        stagePosition[currentSelected].position.y,
                                        transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 1.8f);

        if (isReachedTargetPos(targetPos)) {
            m_animator.SetInteger("AnimState", 0);
        }
        else {
            m_animator.SetInteger("AnimState", 1);

        }
    }

    void ViaCornerPos() {
        m_animator.SetInteger("AnimState", 1);

        Vector3 targetPos = new Vector3(cornerPos.position.x,
                                        cornerPos.position.y,
                                        transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 1.8f);

        if (isReachedTargetPos(targetPos)) {
            previousSelected = currentSelected;
        }
    }

    bool isReachedTargetPos(Vector3 targetPos) {
        if (Mathf.Floor(transform.position.x) == Mathf.Floor(targetPos.x) &&
            Mathf.Floor(transform.position.y) == Mathf.Floor(targetPos.y) &&
            Mathf.Floor(transform.position.z) == Mathf.Floor(targetPos.z)
        ) return true;
        else return false;
    }
}
