using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostOfYours : MonoBehaviour
{
    public int ghostNum = -1;
    public int[] position = new int[2];
    public bool CanTouch = false;


    public void Start()
    {
        
    }

    public void Move(int x, int y)
    {
        position[0] = x;
        position[1] = y;
        if (x == -1 && y == -1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = MyGhosts.instance.position[x, y].position;
        }
    }

    public void OnClick()
    {
        if (CanTouch)
        {
            MyGhosts.instance.Attacked(ghostNum);
        }
    }
}
