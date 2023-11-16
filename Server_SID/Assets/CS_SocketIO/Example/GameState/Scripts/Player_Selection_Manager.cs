using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Selection_Manager : MonoBehaviour
{

    public Sprite[] characters;
    public Image image;
    public NetworkController contection;
    private int index;
    private Dictionary<string,int> keyValuePairs = new Dictionary<string,int>();
    public int players_in_game;
    public data_structure_UI[] games_panles;
    public static Player_Selection_Manager instance;

    private void Start()
    {
        instance = this;
        index = 1;
        contection.Skin = index;
        players_in_game = 0;
        Change_Character(0);
    }

    public void Change_Character(int i)
    {
        index = Mathf.Clamp(index + i, 1, 4);
        image.sprite = characters[index - 1];

        contection.Skin = index;
    }

    public void set_up_new_player(Player pl) 
    {
        if (keyValuePairs.ContainsKey(pl.Id)) return;
        keyValuePairs.Add(pl.Id, players_in_game);
        games_panles[players_in_game].parent.SetActive(true);
        players_in_game++;
    }

    public void update_Score(Player pl) 
    {
        games_panles[keyValuePairs[pl.Id]].image.sprite = characters[pl.Skin - 1];
        games_panles[keyValuePairs[pl.Id]].username.text = pl.Username;
        games_panles[keyValuePairs[pl.Id]].score.text = pl.Score.ToString();
    }
}

[Serializable]
public class data_structure_UI 
{
    public GameObject parent;
    public Image image;
    public TextMeshProUGUI username;
    public TextMeshProUGUI score;
}
