using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public static class PlayerPropertiesExtensions
{
    public static void UpdatePlayerProperty<T>(string key, T value)
    {
        ExitGames.Client.Photon.Hashtable Hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
        object temp = null;
        if (Hashtable.TryGetValue(key, out temp))
        {
            Hashtable[key] = value;
        }
        else
        {
            Hashtable.Add(key, value);
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(Hashtable);
    }

    public static void UpdateEnemyProperty<T>(string key, T value)
    {
        ExitGames.Client.Photon.Hashtable Hashtable = PhotonNetwork.PlayerListOthers[0].CustomProperties;
        object temp = null;
        if (Hashtable.TryGetValue(key, out temp))
        {
            Hashtable[key] = value;
        }
        else
        {
            Hashtable.Add(key, value);
        }
        PhotonNetwork.PlayerListOthers[0].SetCustomProperties(Hashtable);
    }
}
