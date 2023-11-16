using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    public string Id;
    public string Username;

    public TextMeshPro txt;

    public Animator anim;

    public void set_TXT() 
    {
        txt.text = Username;
    }

    public void Set_up(int i) 
    {
        transform.GetChild(i).gameObject.SetActive(true);
        anim = transform.GetChild(i).GetComponent<Animator>();
    }
}
