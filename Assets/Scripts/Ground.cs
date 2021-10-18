using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(GameManager.Instance.isActiveGame)
        {
            GameManager.Instance.Fail();
        }
    }
}
