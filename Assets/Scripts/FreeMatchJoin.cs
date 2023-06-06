using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class FreeMatchJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject FirstPanel;
    [SerializeField] GameObject LoadingPanel;

    public void OnClick()
    {
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        //ランダムなルームに参加する
        PhotonNetwork.JoinRandomRoom();
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    // すでにルームがあれば参加する
    public override void OnJoinedRoom()
    {
        FirstPanel.SetActive(false);
        LoadingPanel.SetActive(true);

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {

            SetOrder();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            SetOrder();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        foreach (var prop in changedProps)
        {
            if ((string)prop.Key == "IsFirst")
            {
                if (targetPlayer != PhotonNetwork.LocalPlayer)
                {
                    if (!(bool)prop.Value)
                    {
                        ChangeOrder(true);
                    }
                    else
                    {
                        ChangeOrder(false);
                    }
                    SceneManager.LoadScene("Game");
                }
            }
        }
    }

    // ルームがなければ作る
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ルームの参加人数を2人に設定する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public void SetOrder()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            int randam = Random.Range(0, 100);

            if (randam % 2 == 0)
            {
                ChangeOrder(true);
            }
            else
            {
                ChangeOrder(false);
            }
            SceneManager.LoadScene("Game");
        }
    }

    private void ChangeOrder(bool k)
    {
        PlayerPropertiesExtensions.UpdatePlayerProperty<bool>("IsFirst", k);

    }
}
