using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstPositionSetCorrect : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject FirstPositionSetPanel;

    public void Onclick()
    {
        PlayerPropertiesExtensions.UpdatePlayerProperty<bool>("Set", true);
        MyGhosts.instance.IsFirstPositionSet();
        MyGhosts.instance.TouchGhost(false);
        FirstPositionSetPanel.SetActive(false);
    }
}
