using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InputController : MonoBehaviour
{

    public static InputController _Instance { get; set; }
    public event Action<Axis> onAxisChange;
    public event Action<Shoot> onPlayerShoot;
    public GameObject player;
    public float timer = 1;
    private float timer_timer;

    private static Axis axis = new Axis { Horizontal = 0, Vertical =0};
    Axis LastAxis = new Axis { Horizontal = 0, Vertical =0};

    void Start()
    {
        _Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");

        axis.Vertical = Mathf.RoundToInt(verticalInput);
        axis.Horizontal = Mathf.RoundToInt(horizontalInput);

        if(timer_timer <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 V2T = Vector2.zero;
                Vector3 mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                V2T = (mouse_Pos - player.transform.position);
                timer_timer = timer;

                onPlayerShoot?.Invoke(new Shoot {x = (int)V2T.x, y= (int) V2T.y, username = player.GetComponent<GamePlayer>().Username });
            }
        }
        else 
        {
            timer_timer -= Time.deltaTime;
        }
        
    }

    /*private Vector3 conver_vector(Vector3 vec) 
    {
        Vector3 vector = new Vector3();

        if(vec.x > 0.5)

        return vector;
    }*/

    public void set_Player(GameObject pl)
    {
        player = pl;
    }

    private void LateUpdate()
    {
        if (AxisChange())
        {
            LastAxis = new Axis { Horizontal = axis.Horizontal, Vertical = axis.Vertical };
            onAxisChange?.Invoke(axis);
        }
    }
 

    private bool AxisChange()
    {
        return (axis.Vertical != LastAxis.Vertical || axis.Horizontal !=LastAxis.Horizontal);
    }
}

public class Axis
{
    public int Horizontal;
    public int Vertical;
}

public class Shoot 
{
    public int x; public int y;
    public string username;
}


