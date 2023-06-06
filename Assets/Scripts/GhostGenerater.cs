using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostGenerater : MonoBehaviour
{
    [SerializeField] MyGhostListEntity ghostListEntity;

    // ‚Ç‚±‚Å‚àÀs‚Å‚«‚é‚â‚Â
    public static GhostGenerater instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GhostMine Spawn(GhostMine.Type type)
    {
        //itemList‚Ì’†‚©‚çtype‚Æˆê’v‚µ‚½‚ç“¯‚¶item‚ğ¶¬‚µ‚Ä“n‚·
        foreach (GhostMine ghost in ghostListEntity.ghostList)
        {
            if (ghost.type == type)
            {
                return new GhostMine(ghost.type, ghost.ghostObj);
            }
        }
        return null;
    }

    public List<GameObject> GetGhost()
    {
        List<GameObject> ghostObjList = new List<GameObject>();

        foreach (GhostMine ghost in ghostListEntity.ghostList)
        {
            for (int i = 0; i < 4; i++)
            {
                ghostObjList.Add(ghost.ghostObj);
            }
            
        }
        return ghostObjList;
    }
}
