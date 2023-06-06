using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaces : MonoBehaviour
{
    public space[] space;
    public int Num;
    // Start is called before the first frame update
    void Start()
    {
        space = GetComponentsInChildren<space>();
        for (int i = 0; i < 6; i++)
        {
            space[i].x = i;
            space[i].y = Num;
            UpdateSpace(i, false);
        }
    }

    public void UpdateSpace(int k, bool a)
    {
        space[k].gameObject.SetActive(a);
    }

    public void ResetYourGhost()
    {
        foreach (var space in space)
        {
            space.yourGhost = -1;
        }
    }

}
