using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostGenerater : MonoBehaviour
{
    [SerializeField] MyGhostListEntity ghostListEntity;

    // どこでも実行できるやつ
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
        //itemListの中からtypeと一致したら同じitemを生成して渡す
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
