using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

// MonoBehaviourPunCallbacks���p�����āAPUN�̃R�[���o�b�N���󂯎���悤�ɂ���
public class FreeMatchJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject FirstPanel;
    [SerializeField] GameObject LoadingPanel;

    public void OnClick()
    {
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        //�����_���ȃ��[���ɎQ������
        PhotonNetwork.JoinRandomRoom();
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    // ���łɃ��[��������ΎQ������
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

    // ���[�����Ȃ���΍��
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���[���̎Q���l����2�l�ɐݒ肷��
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
