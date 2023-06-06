using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;

public class MyGhosts : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks
{
    [SerializeField] Transform objParent;
    GameObject ghostObj;
    List<GameObject> GhostList;
    [SerializeField] Ghost selectedGhost = null;

    [SerializeField] Transform defaltPosition;
    [SerializeField] Spaces[] spacesList; 
    public Transform[,] position = new Transform[6, 6];
    int[,] initialPosition = new int[8, 0];

    Ghost[] ghostsOfMine;
    GhostOfYours[] ghostOfYours;
    [SerializeField] Order order;
    bool isFirstPositionSet = false;
    private PunTurnManager turnManager;
    [SerializeField] GameObject enemysGhost; 
    [SerializeField] PhysicsRaycaster physicsRaycaster;

    [SerializeField] GameObject MyBlueLights;
    [SerializeField] GameObject MyRedLights;
    [SerializeField] GameObject YourBlueLights;
    [SerializeField] GameObject YourRedLights;

    Light[] myBlueLight;
    Light[] myRedLight;
    Light[] yourBlueLight;
    Light[] yourRedLight;

    [SerializeField] GameObject exitButton;
    [SerializeField] WinLosePanel winLosePanel;
    [SerializeField] TurnPanel turnPanel;



    // Ç«Ç±Ç≈Ç‡é¿çsÇ≈Ç´ÇÈÇ‚Ç¬
    public static MyGhosts instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        InitialPositionSet();
        spacesList = GetComponentsInChildren<Spaces>();
        for (int i = 0; i < 6; i++)
        {
            spacesList[i].Num = i;
        }
        this.turnManager = this.gameObject.AddComponent<PunTurnManager>();
        this.turnManager.TurnManagerListener = this;
        this.turnManager.TurnDuration = 600f;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetOrder();
        MakeDefaltPositions();
        GhostInitialize();
        LightsInitialize();
        SetTrigger();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetSpace()
    {
        foreach (var spaces in spacesList)
        {
            for (int i = 0; i < 6; i++)
            {
                spaces.UpdateSpace(i, false);
                spaces.ResetYourGhost();
            }
        }

        foreach (var yourghost in ghostOfYours)
        {
            yourghost.CanTouch = false;
        }
    }

    private void MakeDefaltPositions()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                position[j, i] = defaltPosition.GetChild(i).GetChild(j);
            }
        }
    }

    private void InitialPositionSet()
    {
        int[,] k = new int[8, 2];
        for (int i = 0; i < 8; i++)
        {
            if (i < 4)
            {
                for (int j = 0; j < 4; j++)
                {
                    k[i, 0] = i + 1;
                    k[i, 1] = 0;
                }
            }
            if (i>= 4)
            {
                for (int j = 0; j < 4; j++)
                {
                    k[i, 0] = i - 3;
                    k[i, 1] = 1;
                }
            }
            
        }
        initialPosition = k;
    }

    private void GhostInitialize()
    {
        GhostList = GhostGenerater.instance.GetGhost();
        int myGhostCount = 0;

        for (int i = 0; i < 8; i++)
        {
            ghostObj = Instantiate(GhostList[i], objParent);
            ghostObj.AddComponent<EventTrigger>();
            ghostObj.name = "MyGhost " + myGhostCount;
            myGhostCount++;
        }
        ghostsOfMine = GetComponentsInChildren<Ghost>();
        for (int i = 0; i < 8; i++)
        {
            ghostsOfMine[i].ghostNum = i;
            ghostsOfMine[i].Move(initialPosition[i, 0], initialPosition[i, 1]);
        }
    }

    private void LightsInitialize()
    {
        myBlueLight = MyBlueLights.GetComponentsInChildren<Light>();
        myRedLight = MyRedLights.GetComponentsInChildren<Light>();
        yourBlueLight = YourBlueLights.GetComponentsInChildren<Light>();
        yourRedLight = YourRedLights.GetComponentsInChildren<Light>();
    }

    private void SetTrigger()
    {
        for (int i = 0; i < 8; i++)
        {
            int Num = ghostsOfMine[i].ghostNum;
            EventTrigger.Entry entry = new EventTrigger.Entry();

            EventTrigger trigger = ghostsOfMine[i].GetComponent<EventTrigger>();
            entry.eventID = EventTriggerType.PointerClick;

            entry.callback.AddListener((eventDate) => { OnSelectedGhost(Num); });

            trigger.triggers.Add(entry);

        }
        Debug.Log("SetTrigger");
    }

    public void UpdatePositionProperty(int k, int x, int y)
    {
        PlayerPropertiesExtensions.UpdatePlayerProperty<int>(k + "x", x);
        PlayerPropertiesExtensions.UpdatePlayerProperty<int>(k + "y", y);
    }

    public void OnSelectedGhost(int Num)
    {
        if (selectedGhost == null)
        {
            if (CanGoal(Num))
            {
                exitButton.SetActive(true);
                return;
            }
            selectedGhost = ghostsOfMine[Num];
            selectedGhost.OnClick();
            Debug.Log("GHOST" + selectedGhost.ghostNum + ";" + selectedGhost.position[0] + "," + selectedGhost.position[1]);
            if (isFirstPositionSet)
            {
                ShowCanMove(selectedGhost);
            }
        }
        else if(selectedGhost != null)
        {
            if (isFirstPositionSet)
            {
                selectedGhost.Deselected();
                ResetSpace();
                selectedGhost = null;
            }
            else
            {
                FirstPositionSet(Num);
            }
        }
    }

    private bool CanGoal(int Num)
    {
        if (Num < 4)
        {
            if (ghostsOfMine[Num].position[0] == 0 && ghostsOfMine[Num].position[1] == 5)
            {
                return true;
            }else if(ghostsOfMine[Num].position[0] == 5 && ghostsOfMine[Num].position[1] == 5)
            {
                return true;
            }
        }
        return false;
    }

    public void ShowCanMove(Ghost ghost)
    {
        int[] position = ghost.position;
        Left(position);
        Right(position);
        Forward(position);
        Back(position);
    }

    public void OnClickExit()
    {
        PlayerPropertiesExtensions.UpdatePlayerProperty<bool>("IsWin", true);
        TouchGhost(false);
        turnManager.SendMove(null, true);
        winLosePanel.gameObject.SetActive(true);
        //èüóò
    }

    public void Left(int[] pos)
    {
        if (pos[0] != 0)
        {
            foreach (var myghost in ghostsOfMine)
            {
                if (myghost.position[0] == pos[0] - 1 && myghost.position[1] == pos[1])
                {
                    return;
                }
            }
            foreach (var yourghost in ghostOfYours)
            {
                if (yourghost.position[0] == pos[0] - 1 && yourghost.position[1] == pos[1])
                {
                    yourghost.CanTouch = true;
                    spacesList[pos[1]].space[pos[0] - 1].yourGhost = yourghost.ghostNum;
                }
            }
            spacesList[pos[1]].UpdateSpace(pos[0] - 1, true);
        }
    }

    public void Right(int[] pos)
    {
        if (pos[0] != 5)
        {
            foreach (var myghost in ghostsOfMine)
            {
                if (myghost.position[0] == pos[0] + 1 && myghost.position[1] == pos[1])
                {
                    return;
                }
            }
            foreach (var yourghost in ghostOfYours)
            {
                if (yourghost.position[0] == pos[0] + 1 && yourghost.position[1] == pos[1])
                {
                    yourghost.CanTouch = true;

                    spacesList[pos[1]].space[pos[0] + 1].yourGhost = yourghost.ghostNum;
                }
            }
            spacesList[pos[1]].UpdateSpace(pos[0] + 1, true);
        }
    }

    public void Forward(int[] pos)
    {
        if (pos[1] != 5)
        {
            foreach (var myghost in ghostsOfMine)
            {
                if (myghost.position[1] == pos[1] + 1 && myghost.position[0] == pos[0])
                {
                    return;
                }
            }
            foreach (var yourghost in ghostOfYours)
            {
                if (yourghost.position[1] == pos[1] + 1 && yourghost.position[0] == pos[0])
                {
                    yourghost.CanTouch = true;

                    spacesList[pos[1] + 1].space[pos[0]].yourGhost = yourghost.ghostNum;
                }
            }
            spacesList[pos[1] + 1].UpdateSpace(pos[0], true);
        }
    }

    public void Back(int[] pos)
    {
        if (pos[1] != 0)
        {
            foreach (var myghost in ghostsOfMine)
            {
                if (myghost.position[1] == pos[1] - 1 && myghost.position[0] == pos[0])
                {
                    return;
                }
            }
            foreach (var yourghost in ghostOfYours)
            {
                if (yourghost.position[1] == pos[1] - 1 && yourghost.position[0] == pos[0])
                {
                    yourghost.CanTouch = true;
                    spacesList[pos[1] - 1].space[pos[0]].yourGhost = yourghost.ghostNum;
                }
            }
            spacesList[pos[1] - 1].UpdateSpace(pos[0], true);
        }
    }

    public void OnClickSpace(int x, int y)
    {
        selectedGhost.Move(x, y);
        ResetSpace();
        selectedGhost = null;
        TouchGhost(false);
        turnManager.SendMove(null, true);
        turnPanel.myTurn.SetActive(false);
        turnPanel.enemyTurn.SetActive(true);
    }

    public void Attacked(int Num)
    {
        if (Num < 4)
        {
            Lighting(myBlueLight);
            if (myBlueLight[3].IsLighting)
            {
                //èüóò
                PlayerPropertiesExtensions.UpdatePlayerProperty<bool>("IsWin", true);
                TouchGhost(false);
                turnManager.SendMove(null, true);
                winLosePanel.gameObject.SetActive(true);
            }
        }
        else if (Num >= 4)
        {
            Lighting(myRedLight);
            if (myRedLight[3].IsLighting)
            {
                //îsñk
                PlayerPropertiesExtensions.UpdatePlayerProperty<bool>("IsWin", false);
                TouchGhost(false);
                turnManager.SendMove(null, true);
                winLosePanel.gameObject.SetActive(true);
                winLosePanel.ChangeLose();
            }
        }
        selectedGhost.Move(ghostOfYours[Num].position[0], ghostOfYours[Num].position[1]);
        ResetSpace();
        selectedGhost = null;
        TouchGhost(false);
        PlayerPropertiesExtensions.UpdateEnemyProperty<int>(Num + "x", -1);
        PlayerPropertiesExtensions.UpdateEnemyProperty<int>(Num + "y", -1);
        ghostOfYours[Num].Move(-1, -1);
        turnManager.SendMove(null, true);
        turnPanel.myTurn.SetActive(false);
        turnPanel.enemyTurn.SetActive(true);
    }

    public void Lighting(Light[] lights)
    {
        foreach (var light in lights)
        {
            if (!light.IsLighting)
            {
                light.Changelight();
                light.IsLighting = true;
                return;
            }
        }
    }

    public void SetOrder()
    {
        if (!(bool)PhotonNetwork.LocalPlayer.CustomProperties["IsFirst"])
        {
            order.ChangeOrder();
        }
    }

    public void FirstPositionSet(int k)
    {
        int selectedPositionX = ghostsOfMine[k].position[0];
        int selectedPositionY = ghostsOfMine[k].position[1];

        ghostsOfMine[k].Move(selectedGhost.position[0], selectedGhost.position[1]);
        selectedGhost.Move(selectedPositionX, selectedPositionY);
        selectedGhost = null;
    }

    public void IsFirstPositionSet()
    {
        isFirstPositionSet = true;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        foreach (var prop in changedProps)
        {
            if ((string)prop.Key == "Set")
            {
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    if (player.CustomProperties["Set"] == null)
                    {
                        return;
                    }
                }
                PlayerPropertiesExtensions.UpdatePlayerProperty<Object>("Set", null);
                //ÉQÅ[ÉÄäJén
                Debug.Log("Game Start!");
                SpawnEnemysGhost();
                turnManager.BeginTurn();
            }
        }   
    }

    public void SpawnEnemysGhost()
    {
        int GhostCount = 0;

        for (int i = 0; i < 8; i++)
        {
            ghostObj = Instantiate(enemysGhost, objParent);
            ghostObj.name = "YourGhost " + GhostCount;
            GhostCount++;
        }
        ghostOfYours = GetComponentsInChildren<GhostOfYours>();
        for (int i = 0; i < 8; i++)
        {
            ghostOfYours[i].ghostNum = i;
            ghostOfYours[i].Move(GetEnemysGhostPosition(i,"x"), GetEnemysGhostPosition(i, "y"));
        }
    }

    public int GetMyGhostPosition(int k, string xy)
    {
        return (int)PhotonNetwork.LocalPlayer.CustomProperties[k + xy];
    }

    public int GetEnemysGhostPosition(int k, string xy)
    {
        int position = (int)PhotonNetwork.PlayerListOthers[0].CustomProperties[k + xy];
        if (position == -1)
        {
            return -1;
        }
        return 5 - position;
    }

    public void TouchGhost(bool k)
    {
        physicsRaycaster.enabled = k;
    }

    public void BeginMyTurn()
    {
        TouchGhost(true);
        turnPanel.gameObject.SetActive(true);
        turnPanel.myTurn.SetActive(true);
        turnPanel.enemyTurn.SetActive(false);
    }

    void IPunTurnManagerCallbacks.OnTurnBegins(int turn)
    {
        if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["IsFirst"])
        {
            BeginMyTurn();
        }
        else
        {
            turnPanel.gameObject.SetActive(true);
            turnPanel.enemyTurn.SetActive(true);
        }
    }

    void IPunTurnManagerCallbacks.OnPlayerMove(Photon.Realtime.Player player, int turn, object move)
    {
        for (int i = 0; i < 8; i++)
        {
            ghostOfYours[i].Move(GetEnemysGhostPosition(i, "x"), GetEnemysGhostPosition(i, "y"));
        }
    }

    void IPunTurnManagerCallbacks.OnPlayerFinished(Player player, int turn, object move)
    {
        for (int i = 0; i < 8; i++)
        {
            ghostOfYours[i].Move(GetEnemysGhostPosition(i, "x"), GetEnemysGhostPosition(i, "y"));
        }
        int BlueNum = 0;
        int RedNum = 0;

        for (int i = 0; i < 8; i++)
        {
            ghostsOfMine[i].Move(GetMyGhostPosition(i, "x"), GetMyGhostPosition(i, "y"));
            if (ghostsOfMine[i].position[0] == -1 && ghostsOfMine[i].position[1] == -1)
            {
                if (i < 4)
                {
                    BlueNum++;
                }
                else if (i >= 4)
                {
                    RedNum++;
                }
            }
        }

        if (BlueNum != 0)
        {
            yourBlueLight[BlueNum - 1].Changelight();
        }

        if (RedNum != 0)
        {
            yourRedLight[RedNum - 1].Changelight();
        }

        if (!(bool)PhotonNetwork.LocalPlayer.CustomProperties["IsFirst"] && PhotonNetwork.LocalPlayer != player)
        {
            BeginMyTurn();
        }

        if (PhotonNetwork.PlayerListOthers[0].CustomProperties["IsWin"] != null)
        {
            if ((bool)PhotonNetwork.PlayerListOthers[0].CustomProperties["IsWin"])
            {
                TouchGhost(false);
                winLosePanel.gameObject.SetActive(true);
                winLosePanel.ChangeLose();
                //îsñk
            }else if (!(bool)PhotonNetwork.PlayerListOthers[0].CustomProperties["IsWin"])
            {
                TouchGhost(false);
                winLosePanel.gameObject.SetActive(true);
                //èüóò
            }
        }
    }

    void IPunTurnManagerCallbacks.OnTurnCompleted(int turn)
    {
        turnManager.BeginTurn();
    }

    void IPunTurnManagerCallbacks.OnTurnTimeEnds(int turn)
    {

    }

}
