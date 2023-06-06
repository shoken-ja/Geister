using System;
using UnityEngine;

[Serializable]
public class GhostMine
{
    public enum Type
    {
        Red,
        Blue
    }

    public Type type;
    public GameObject ghostObj;

    public GhostMine(Type type, GameObject ghostObj)
    {
        this.type = type;
        this.ghostObj = ghostObj;
    }
}
