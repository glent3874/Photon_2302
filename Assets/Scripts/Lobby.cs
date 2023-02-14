using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 1.�s�u�����
/// 2.��ܳs����� ��ܳt��
/// </summary>
public class Lobby : MonoBehaviourPunCallbacks
{
    #region ��ҼҦ�
    public static Lobby instance = null;
    private void Awake()
    {
        instance = this;    
    }
    #endregion

    #region ���
    [SerializeField] Text �s�u���A;
    [SerializeField] Transform �[�J�ж����s = null;
    [SerializeField] Transform ���}�ж����s = null;

    public GameObject mainPlayer = null;
    public Player mainPlayerScript = null;
    List<Player> ���a�C�� = new List<Player>();
    #endregion

    #region �ƥ�
    private void Start()
    {
        �s�u���A.text = "�s�u��...";

        //�Q�ιw�]�ȳs�������
        PhotonNetwork.ConnectUsingSettings();

        �[�J�ж����s.localScale = Vector3.zero;
        ���}�ж����s.localScale = Vector3.zero;
    }

    /// <summary>
    /// �s�u���Ц��\�ɷ|����
    /// </summary>
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        �s�u���A.text = "�s�u��: " + PhotonNetwork.CloudRegion + " Ping: " + PhotonNetwork.GetPing();
        �[�J�ж����s.localScale = Vector3.one;
        ���}�ж����s.localScale = Vector3.zero;
    }
    #endregion

    #region ��k
    public void �۰ʥ[�J()
    {
        �s�u���A.text = "�t�襤...";
        //�[�J�H���ж�
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// �[�J����ж����\
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        �s�u���A.text = "�i�J�F: " + PhotonNetwork.CurrentRoom.Name;

        �[�J�ж����s.localScale = Vector3.zero;
        ���}�ж����s.localScale = Vector3.one;

        //�إߦۤv������
        mainPlayer = PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)), Quaternion.identity);
        mainPlayerScript = mainPlayer.GetComponent<Player>();
    }

    /// <summary>
    /// ���}�ж����\��
    /// </summary>
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        �s�u���A.text = "";
        �[�J�ж����s.localScale = Vector3.one;
        ���}�ж����s.localScale = Vector3.zero;
    }

    /// <summary>
    /// �[�J�H���ж�����
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        �s�u���A.text = "�䤣���H���ж�!";
        //�۰ʳЫؤ@�өж������a�[�J
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        //createRoom �ݭn���@�өЦWstring
        PhotonNetwork.CreateRoom(Random.Range(100000, 999999).ToString(), roomOptions); ;
    }

    /// <summary>
    /// �V���A���o�X���}�ж��ШD
    /// </summary>
    public void ���}�ж�()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void �󴫥~�[()
    {
        mainPlayerScript.ChangeSkin();
    }

    public void �n�O�J��(Player playerIn)
    {
        ���a�C��.Add(playerIn);
        �ƦC���a����();
    }

    public void �n�O����(Player playerOut)
    {
        ���a�C��.Remove(playerOut);
        �ƦC���a����();
    }

    void �ƦC���a����()
    {
        for (int i = 0; i < ���a�C��.Count; i++) 
        {
            ���a�C��[i].transform.position = new Vector3(i * 2, 0f, 0f);
        }
    }
    #endregion
}
