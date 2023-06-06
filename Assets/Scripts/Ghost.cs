using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class Ghost : MonoBehaviourPunCallbacks
{
    [SerializeField] Material[] materials = new Material[2];
    public bool selected = false;
    public int ghostNum = -1;
    public int[] position = new int[2];


    public void Start()
    {
        GetComponent<MeshRenderer>().material = materials[0];
    }

    public void Move(int x, int y)
    {
        position[0] = x;
        position[1] = y;
        MyGhosts.instance.UpdatePositionProperty(ghostNum, x, y);
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
        selected = true;
        transform.position += new Vector3(0, 1.5f, 0);
    }

    public void Deselected()
    {
        Move(position[0], position[1]);
    }
}