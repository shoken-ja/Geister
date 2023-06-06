using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Light : MonoBehaviour
{
    [SerializeField] Sprite lightImg;
    public bool IsLighting = false; 
    public void Changelight()
    {
        this.GetComponent<Image>().sprite = lightImg;
        IsLighting = true;
    }
}
