using System;
using System.Collections;
using Photon;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

using ExitGames.Client.Photon;

// the Photon server assigns a ActorNumber (player.ID) to each player, beginning at 1
// for this game, we don't mind the actual number
// this game uses player 0 and 1, so clients need to figure out their number somehow
public class Core : PunBehaviour, IPunTurnManagerCallbacks
{
    [SerializeField]
    private RectTransform TimerFillImage;

    [SerializeField]
    private Text TurnText;

    [SerializeField]
    private Text TimeText;

    [SerializeField]
    private Text RemotePlayerText;

    [SerializeField]
    private RectTransform DisconnectedPanel;

    private PunTurnManager turnManager;

    //!!!
    [SerializeField]
    private GameObject Personnage1Prefab;
    [SerializeField]
    private GameObject Personnage2Prefab;
    [SerializeField]
    private GameObject Personnage3Prefab;
    [SerializeField]
    private GameObject Personnage4Prefab;

    private GameObject Personnage1;
    private GameObject Personnage2;
    private GameObject Personnage3;
    private GameObject Personnage4;

    private bool initialiserPartie;

    public PunTurnManager TurnManager
    {
        get
        {
            return turnManager;
        }

        set
        {
            turnManager = value;
        }
    }

    public void Start()
    {
        this.TurnManager = this.gameObject.AddComponent<PunTurnManager>();
        this.TurnManager.TurnManagerListener = this;
        this.TurnManager.TurnDuration = 10f;
        
        RefreshUIViews();
    }

    public void Update()
    {
        if (!initialiserPartie && GameObject.FindGameObjectWithTag("Personnage") != null)
        {
            /*Personnage1 = Instantiate(Personnage1Prefab, Personnage1Prefab.transform.position, Quaternion.identity);
            Personnage2 = Instantiate(Personnage2Prefab, Personnage2Prefab.transform.position, Quaternion.identity);
            Personnage3 = Instantiate(Personnage3Prefab, Personnage3Prefab.transform.position, Quaternion.identity);
            Personnage4 = Instantiate(Personnage4Prefab, Personnage4Prefab.transform.position, Quaternion.identity);*/
            GameObject.Find("Plateau").AddComponent<Partie>();
            GameObject.Find("Plateau").GetComponent<Partie>().enabled = true;

            Transform[] allChildren = GameObject.Find("Plateau").GetComponentsInChildren<Transform>();
            foreach (Transform c in allChildren)
            {
                if(c.gameObject.name.Length >= 7 && c.gameObject.name.Substring(0,7) == "CaseSol")
                {
                    c.gameObject.AddComponent<Case>();
                }
            }

            initialiserPartie = true;
        }

        // Check if we are out of context, which means we likely got back to the demo hub.
        if (this.DisconnectedPanel == null)
        {
            Destroy(this.gameObject);
        }

        // for debugging, it's useful to have a few actions tied to keys:
        if (Input.GetKeyUp(KeyCode.L))
        {
            PhotonNetwork.LeaveRoom();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            PhotonNetwork.ConnectUsingSettings(null);
            PhotonHandler.StopFallbackSendAckThread();
        }


        if (!PhotonNetwork.inRoom)
        {
            return;
        }
        
        // disable the "reconnect panel" if PUN is connected or connecting
        if (PhotonNetwork.connected && this.DisconnectedPanel.gameObject.GetActive())
        {
            this.DisconnectedPanel.gameObject.SetActive(false);
        }
        if (!PhotonNetwork.connected && !PhotonNetwork.connecting && !this.DisconnectedPanel.gameObject.GetActive())
        {
            this.DisconnectedPanel.gameObject.SetActive(true);
        }


        if (PhotonNetwork.room.PlayerCount > 1)
        {
            if (this.TurnManager.IsOver)
            {
                return;
            }

            /*
			// check if we ran out of time, in which case we loose
			if (turnEnd<0f && !IsShowingResults)
			{
					Debug.Log("Calling OnTurnCompleted with turnEnd ="+turnEnd);
					OnTurnCompleted(-1);
					return;
			}
		*/

            if (this.TurnText != null)
            {
                this.TurnText.text = this.TurnManager.Turn.ToString();
            }

            if (this.TurnManager.Turn > 0 && this.TimeText != null)
            {

                this.TimeText.text = this.TurnManager.RemainingSecondsInTurn.ToString("F1") + " SECONDES";

                TimerFillImage.anchorMax = new Vector2(1f - this.TurnManager.RemainingSecondsInTurn / this.TurnManager.TurnDuration, 1f);
            }


        }

        this.UpdatePlayerTexts();

        /*if (!this.turnManager.IsCompletedByAll)
        {
            if (PhotonNetwork.room.PlayerCount < 2)
            {

            }

            // if the turn is not completed by all, we use a random image for the remote hand
            else if (this.turnManager.Turn > 0 && !this.turnManager.IsCompletedByAll)
            {
                // alpha of the remote hand is used as indicator if the remote player "is active" and "made a turn"
                PhotonPlayer remote = PhotonNetwork.player.GetNext();
            }
        }*/

    }

    #region TurnManager Callbacks

    /// <summary>Called when a turn begins (Master Client set a new Turn number).</summary>
    public void OnTurnBegins(int turn)
    {
        Debug.Log("OnTurnBegins() turn: " + turn);

    }


    public void OnTurnCompleted(int obj)
    {
        Debug.Log("OnTurnCompleted: " + obj);

        this.UpdateScores();
        this.OnEndTurn();
    }


    // when a player moved (but did not finish the turn)
    public void OnPlayerMove(PhotonPlayer photonPlayer, int turn, object move)
    {
        Debug.Log("OnPlayerMove: " + photonPlayer + " turn: " + turn + " action: " + move);
        throw new NotImplementedException();
    }


    // when a player made the last/final move in a turn
    public void OnPlayerFinished(PhotonPlayer photonPlayer, int turn, object move)
    {
        Debug.Log("OnTurnFinished: " + photonPlayer + " turn: " + turn + " action: " + move);
    }



    public void OnTurnTimeEnds(int obj)
    {
        Debug.Log("OnTurnTimeEnds: Calling OnTurnCompleted");

        if (PhotonNetwork.isMasterClient)
        {
            GameObject.Find("Plateau").GetComponent<Partie>().View.RPC("FinTour", PhotonTargets.All);
            this.TurnManager.BeginTurn();
        }

    }

    private void UpdateScores()
    {

    }

    #endregion

    #region Core Gameplay Methods


    /// <summary>Call to start the turn (only the Master Client will send this).</summary>
    public void StartTurn()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Personnage1 = PhotonNetwork.Instantiate("Prefabs/" + Personnage1Prefab.name, Personnage1Prefab.transform.position, Quaternion.identity, 0);
            Personnage2 = PhotonNetwork.Instantiate("Prefabs/" + Personnage2Prefab.name, Personnage2Prefab.transform.position, Quaternion.identity, 0);
            Personnage3 = PhotonNetwork.Instantiate("Prefabs/" + Personnage3Prefab.name, Personnage3Prefab.transform.position, Quaternion.identity, 0);
            Personnage4 = PhotonNetwork.Instantiate("Prefabs/" + Personnage4Prefab.name, Personnage4Prefab.transform.position, Quaternion.identity, 0);
            this.TurnManager.BeginTurn();
            //!!!
            
        }
    }

    public void MakeTurn()
    {
        
    }

    public void OnEndTurn()
    {

    }




    public void EndGame()
    {
        Debug.Log("EndGame");
    }

    private void UpdatePlayerTexts()
    {
        PhotonPlayer remote = PhotonNetwork.player.GetNext();
        PhotonPlayer local = PhotonNetwork.player;

        if (remote == null)
        {
            TimerFillImage.anchorMax = new Vector2(0f, 1f);
            this.TimeText.text = "";
            this.RemotePlayerText.text = "waiting for another player";
        }
    }


    #endregion


    #region Handling Of Buttons

    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings(null);
        PhotonHandler.StopFallbackSendAckThread();  // this is used in the demo to timeout in background!
    }

    public void OnClickReConnectAndRejoin()
    {
        PhotonNetwork.ReconnectAndRejoin();
        PhotonHandler.StopFallbackSendAckThread();  // this is used in the demo to timeout in background!
    }

    #endregion

    void RefreshUIViews()
    {
        TimerFillImage.anchorMax = new Vector2(0f, 1f);
    }


    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom()");
        RefreshUIViews();
    }

    public override void OnJoinedRoom()
    {
        RefreshUIViews();

        if (PhotonNetwork.room.PlayerCount == 2)
        {
            if (this.TurnManager.Turn == 0)
            {
                // when the room has two players, start the first turn (later on, joining players won't trigger a turn)
                //Partie.personnageTour = PhotonNetwork.Instantiate("Prefabs/" + Personnage1.name, Personnage1.transform.position, Quaternion.identity, 0).GetComponent<Mage>();
                this.StartTurn();
            }
        }
        else
        {
            Debug.Log("Waiting for another player");
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("Other player arrived");

        if (PhotonNetwork.room.PlayerCount == 2)
        {
            if (this.TurnManager.Turn == 0)
            {
                // when the room has two players, start the first turn (later on, joining players won't trigger a turn)
                this.StartTurn();
                this.RemotePlayerText.text = "";
            }
        }
    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("Other player disconnected! " + otherPlayer.ToStringFull());
    }


    public override void OnConnectionFail(DisconnectCause cause)
    {
        this.DisconnectedPanel.gameObject.SetActive(true);
    }

}
