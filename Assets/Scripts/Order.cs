using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    [SerializeField] GameObject yourOrder;
    [SerializeField] Sprite first;
    [SerializeField] Sprite second;
    public void ChangeOrder()
    {
        this.GetComponent<Image>().sprite = second;
        yourOrder.GetComponent<Image>().sprite = first;
    }
}
