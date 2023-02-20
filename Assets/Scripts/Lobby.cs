using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// �j�U
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
    [SerializeField] GameObject �}�l�̽� = null;

    public GameObject mainPlayer = null;
    public Player mainPlayerScript = null;
    List<Player> ���a�C�� = new List<Player>();
    int �ǳƦn���H�� = 0;
    #endregion

    #region �ƥ�
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;        //��Ъ��P�@���Ъ����a�|�۰ʸ�Ъ��@�_������

        PlayerInfoManager.instance.Load();                  //�s���a�s�ɨt��Ū�����a���

        �s�u���A.text = "�s�u��...";

        PhotonNetwork.ConnectUsingSettings();               //�Q�ιw�]�ȳs�������

        �[�J�ж����s.localScale = Vector3.zero;              //�s�u���\�e���������a�ϥΫ��s
        ���}�ж����s.localScale = Vector3.zero;
    }

    /// <summary>
    /// �s�u���Ц��\�ɷ|����
    /// </summary>
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        �s�u���A.text = "�s�u��: " + PhotonNetwork.CloudRegion + " Ping: " + PhotonNetwork.GetPing();  //�s�u��: ���A����a    Ping:�s�u�t��
        �[�J�ж����s.localScale = Vector3.one;                //�s���Ц��\ �i�H�[�J�ж��F
        ���}�ж����s.localScale = Vector3.zero;
    }
    #endregion

    #region ��k
    /// <summary>
    /// �۰ʴM��ж�
    /// </summary>
    public void �۰ʥ[�J()
    {
        �s�u���A.text = "�t�襤...";

        PhotonNetwork.JoinRandomRoom();                      //�[�J�H���ж�
    }

    /// <summary>
    /// �[�J����ж����\
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        �s�u���A.text = "�i�J�F: " + PhotonNetwork.CurrentRoom.Name;       //��e�ж����s��

        �[�J�ж����s.localScale = Vector3.zero;               //�i�J�ж����\ �i�H���}�ж��F
        ���}�ж����s.localScale = Vector3.one;

        //���ͦۤv������
        mainPlayer = PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)), Quaternion.identity);
        mainPlayerScript = mainPlayer.GetComponent<Player>();       //��ۤv���⪺�}��
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
        PhotonNetwork.CreateRoom(Random.Range(100000, 999999).ToString(), roomOptions);         //createRoom �ݭn���@�өЦWstring
    }

    /// <summary>
    /// �V���A���o�X���}�ж��ШD
    /// </summary>
    public void ���}�ж�()
    {
        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// �V����n�D�󴫳y��
    /// </summary>
    public void �󴫥~�[()
    {
        mainPlayerScript.ChangeSkin();
    }

    /// <summary>
    /// ���a�i�J�ж��� �s�W�ܪ��a�C��è�s�ƦC
    /// </summary>
    /// <param name="playerIn"></param>
    public void �n�O�J��(Player playerIn)
    {
        ���a�C��.Add(playerIn);
        �ƦC���a����();
    }

    /// <summary>
    /// ���a���}�ж��� �����۪��a�C��è�s�ƦC
    /// </summary>
    /// <param name="playerOut"></param>
    public void �n�O����(Player playerOut)
    {
        ���a�C��.Remove(playerOut);
        �ƦC���a����();
    }

    /// <summary>
    /// �N�����b���w��m�W
    /// </summary>
    void �ƦC���a����()
    {
        for (int i = 0; i < ���a�C��.Count; i++) 
        {
            ���a�C��[i].transform.position = new Vector3(i * 2, 0f, 0f);
        }
    }

    /// <summary>
    /// �Ϩ���i�J�ǳƪ��A�ýT�{�O�_�����ǳ�
    /// </summary>
    public void �ǳƦn�F()
    {
        mainPlayerScript.isReady = true;

        �T�{�O�_�����ǳƦn();
    }

    /// <summary>
    /// �p�G�ڬO�ХD�N�T�{�Ҧ��H���ǳƪ��A
    /// </summary>
    public void �T�{�O�_�����ǳƦn()
    {
        //�p�G�ڬO�ХD
        if (PhotonNetwork.IsMasterClient)
        {
            int �ǳƦn���H�� = 0;
            for (int i = 0; i < ���a�C��.Count; i++)
            {
                if (���a�C��[i].isReady == true)
                {
                    �ǳƦn���H��++;
                }
            }

            //���ǳƦn�F�N�}�l�C��
            if (�ǳƦn���H�� >= ���a�C��.Count)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;                   //�����ж�

                photonView.RPC("RPCStartGame", RpcTarget.AllBuffered);      //�q���Ҧ��蹳�}�l�C��
            }
        }
    }

    /// <summary>
    /// �}�l�C��
    /// </summary>
    [PunRPC]
    public void RPCStartGame()
    {
        �}�l�̽�.SetActive(true);                   //�}�ҫ̽�
        Invoke("ToGameplay", 2f);                   //�����������
    }

    /// <summary>
    /// ��������
    /// </summary>
    void ToGameplay()
    {
        //�p�G�ڬO�ХD�N��������
        if (PhotonNetwork.IsMasterClient == true) 
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
    #endregion
}
