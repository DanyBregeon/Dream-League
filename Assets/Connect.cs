using Photon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using ExitGames.Client.Photon;

public class Connect : PunBehaviour
{
    public InputField InputField;
    public string UserId;

    string previousRoomPlayerPrefKey = "PUN:Demo:RPS:PreviousRoom";
    public string previousRoom;

    private const string MainSceneName = "DemoRPS-Scene";

    const string NickNamePlayerPrefsKey = "NickName";


    void Start()
    {
        ApplyUserIdAndConnect();
        //PhotonNetwork.automaticallySyncScene = true;
    }

    public void ApplyUserIdAndConnect()
    {
        //if (string.IsNullOrEmpty(UserId))
        //{
        //    this.UserId = nickName + "ID";
        //}

        PhotonNetwork.ConnectUsingSettings("0.5");

        // this way we can force timeouts by pausing the client (in editor)
        PhotonHandler.StopFallbackSendAckThread();
    }
    //test
    /*public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel(null);
    }*/


    public override void OnConnectedToMaster()
    {
        // after connect 
        this.UserId = PhotonNetwork.player.UserId;
        ////Debug.Log("UserID " + this.UserId);

        // after timeout: re-join "old" room (if one is known)
        if (!string.IsNullOrEmpty(this.previousRoom))
        {
            Debug.Log("ReJoining previous room: " + this.previousRoom);
            PhotonNetwork.ReJoinRoom(this.previousRoom);
            this.previousRoom = null;       // we only will try to re-join once. if this fails, we will get into a random/new room
        }
        else
        {
            // else: join a random room
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinedLobby()
    {
        OnConnectedToMaster(); // this way, it does not matter if we join a lobby or not
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("OnPhotonRandomJoinFailed");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2, PlayerTtl = 20000 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.room.Name);
        this.previousRoom = PhotonNetwork.room.Name;

    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("OnPhotonJoinRoomFailed");
        this.previousRoom = null;
    }

    public override void OnConnectionFail(DisconnectCause cause)
    {
        Debug.Log("Disconnected due to: " + cause + ". this.previousRoom: " + this.previousRoom);
    }

    public override void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
    {
        Debug.Log("OnPhotonPlayerActivityChanged() for NickName IsInactive: " + otherPlayer.IsInactive);
    }

}
