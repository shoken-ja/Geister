using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class WinLosePanel : MonoBehaviour
{
    [SerializeField] GameObject Image;
    [SerializeField] Sprite LoseImg;

    public void ChangeLose()
    {
        Image.GetComponent<Image>().sprite = LoseImg;
    }

    public void BackToMenu()
    {
        List<object> keys = new List<object>();
        foreach (var key in PhotonNetwork.LocalPlayer.CustomProperties.Keys)
        {
            keys.Add(key);
        }
        foreach (var key in keys)
        {
            PlayerPropertiesExtensions.UpdatePlayerProperty<object>((string)key, null);
        }
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Title");
    }
}
