using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject GameContainer;
    [SerializeField]
    private Transform PlayersContainer;
    [SerializeField]
    private Transform CoinsContainer;

    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    private GameObject CoinPrefab;

    public GameState State;
    private Dictionary<string, Transform> PlayersToRender;
    private Dictionary<string, Transform> BulletsToRender;
    private string current_User_USername;
    internal void StartGame(GameState state)
    {
        PlayersToRender = new Dictionary<string, Transform>();
        BulletsToRender = new Dictionary<string, Transform>();

        GameObject.Find("PanelConnect").SetActive(false);
        GameContainer.SetActive(true);


        foreach (Player player in state.Players)
        {
            InstantiatePlayer(player);
        }

        var Socket = NetworkController._Instance.Socket;

        InputController._Instance.onAxisChange += (axis) => { Socket.Emit("move", axis); };
        InputController._Instance.onPlayerShoot += (shoot) => { Socket.Emit("Shoot", shoot); };

        State = state;
        Socket.On("updateState", UpdateState);
    }

    private void InstantiatePlayer(Player player)
    {
        GameObject playerGameObject = Instantiate(PlayerPrefab, PlayersContainer);
        if (player.Username == NetworkController._Instance.Username) InputController._Instance.set_Player(playerGameObject);
        playerGameObject.transform.position = new Vector2(player.x, player.y);
        playerGameObject.GetComponent<GamePlayer>().Id = player.Id;
        playerGameObject.GetComponent<GamePlayer>().Username = player.Username;
        playerGameObject.GetComponent<GamePlayer>().set_TXT();
        playerGameObject.GetComponent<GamePlayer>().Set_up(player.Skin);
        Debug.Log(player.Skin);
        Player_Selection_Manager.instance.set_up_new_player(player);
        PlayersToRender[player.Id] = playerGameObject.transform;
    }

    private void UpdateState(string json)
    {
        GameStateData jsonData = JsonUtility.FromJson<GameStateData>(json);
        State = jsonData.State;

    }

    internal void NewPlayer(string id, string username)
    {
        InstantiatePlayer(new Player { Id = id, Username = username });

    }

    void Update()
    {
        if (State != null)
        {
            foreach (Player player in State.Players)
            {

                if (PlayersToRender.ContainsKey(player.Id))
                {
                    if (player.Dead) 
                    {
                        PlayersToRender[player.Id].gameObject.SetActive(false);
                    }
                    else 
                    {
                        PlayersToRender[player.Id].gameObject.SetActive(true);
                        PlayersToRender[player.Id].position = new Vector2(player.x, player.y);
                        PlayersToRender[player.Id].GetComponent<GamePlayer>().anim.SetFloat("X", player.Dir_X);
                        Player_Selection_Manager.instance.update_Score(player);
                    }
                }
                else
                {
                    InstantiatePlayer(player);
                }

            }
            var plarersToDelete = PlayersToRender.Where(item => !State.Players.Any(player => player.Id == item.Key)).ToList();
            foreach (var playerItem in plarersToDelete)
            {
                Destroy(playerItem.Value.gameObject);
                PlayersToRender.Remove(playerItem.Key);
            }

            foreach (bullet bul in State.Bullets)
            {
                if (BulletsToRender.ContainsKey(bul.id))
                {
                    BulletsToRender[bul.id].position = new Vector2(bul.x, bul.y);
                }
                else
                {
                    InstantiateCoin(bul);
                }
            }

            if (BulletsToRender.Count > State.Bullets.Length)
            {
                if (State.Bullets.Length == 0)
                {
                    foreach (string trns in BulletsToRender.Keys)
                    {
                        GameObject g = BulletsToRender[trns].gameObject;
                        BulletsToRender.Remove(trns);
                        Destroy(g);

                    }
                }
                else
                {

                    foreach (string trns in BulletsToRender.Keys)
                    {
                        bullet b = (bullet)State.Bullets.Where(x => x.id == trns);

                        if (b == null)
                        {
                            GameObject g = BulletsToRender[trns].gameObject;
                            BulletsToRender.Remove(trns);
                            Destroy(g);
                        }
                    }
                }
            }


        }
    }
    private void InstantiateCoin(bullet bullet)
    {
        GameObject coinGameObject = Instantiate(CoinPrefab, CoinsContainer);
        coinGameObject.transform.position = new Vector2(bullet.x, bullet.y);
        coinGameObject.GetComponent<GameCoin>().Id = bullet.id;
        
        BulletsToRender[bullet.id] = coinGameObject.transform;
    }
}

[Serializable]
public class GameStateData
{
    public GameState State;
}
