using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject target;
    private Vector3 offset = new Vector3(0, 0, -8);
 
    void LateUpdate()
    {
        target = GameObject.Find("mixamorig:HeadTop_End");
        if (GameManager.Instance.isActiveGame)
        transform.position = target.transform.position + offset;
    }
}
