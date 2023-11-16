using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Manager : MonoBehaviour
{
    public static Bullet_Manager instance;
    void Start()
    {
        instance = this;
    }

    public GameObject[] Get_Active_Bullets() 
    {
        GameObject[] ret = new GameObject[transform.childCount];

        for(int i = 0; i < ret.Length; i++) 
        {
            ret[i] = transform.GetChild(i).gameObject;
        }

        return ret;
    }
}
