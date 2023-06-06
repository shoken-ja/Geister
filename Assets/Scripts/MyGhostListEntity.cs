using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MyGhostListEntity : ScriptableObject
{
    public List<GhostMine> ghostList = new List<GhostMine>();
}
