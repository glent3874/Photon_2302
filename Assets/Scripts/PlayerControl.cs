using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 在遊戲內的玩家控制
/// </summary>
public class PlayerControl : MonoBehaviourPunCallbacks
{
    #region 欄位
    [SerializeField] Rigidbody rig = null;
    [SerializeField] [Tooltip("跑速設定")] float speed = 5f;
    [SerializeField] Transform rotateRoot = null;
    [SerializeField] SetPlayerInfo 衣服系統 = null;
    [SerializeField] float 鏡像動畫緩衝 = 0f;
    [SerializeField] LayerMask 滑鼠可以點擊的圖層;
    [SerializeField] GameObject 魔法砲彈 = null;

    [Tooltip("攝影機追蹤目標")]
    GameObject 自拍棒 = null;
    Vector3 lastPos = Vector3.zero;
    [Tooltip("實際上的跑速")]
    float moveSpeed = 0f;
    Quaternion moveQuaternion = Quaternion.identity;
    Vector3 bufferAB = Vector3.zero;
    #endregion

    #region 事件
    void Start()
    {   
        //如果我是本尊 把攝影機追蹤的目標"CameraPos" 做成自拍棒 並作為我的子物件
        if (photonView.IsMine)
        {
            自拍棒 = GameObject.Find("CameraPos");
            自拍棒.transform.SetParent(this.transform);
            自拍棒.transform.localPosition = Vector3.zero;
        }
    }

    private void Update()
    {
        //如果我是本尊 
        if (photonView.IsMine)
        {
            Move();                                             //移動
        }
        Rotate();                                               //旋轉
        AniUpdate();                                            //動畫更新
    }

    private void FixedUpdate()
    {
        Vector3 pos = rig.position;                             //當前位置
        Vector3 a = lastPos;                                    //上一偵的位置
        Vector3 b = pos;
        a.y = b.y;                                              //同步高度
        Vector3 ab = b - a;
        moveSpeed = Vector3.Distance(a, b);

        moveSpeed = moveSpeed / Time.fixedDeltaTime;            //計算每一偵的移動量

        //當有移動就刷新旋轉
        if (moveSpeed > 0.01f)
        {
            bufferAB = Vector3.Lerp(bufferAB, ab, Time.fixedDeltaTime * 10f);           //漸變
            moveQuaternion = Quaternion.LookRotation(bufferAB);                         //將ab移動值改變成旋轉量
        }
        lastPos = rig.position;
    }
    #endregion

    #region 方法
    /// <summary>
    /// 物理系統移動與發射子彈
    /// </summary>
    void Move()
    {
        //物理系統移動
        float ws = Input.GetAxisRaw("Vertical");                                                        //取得垂直輸入值
        float ad = Input.GetAxisRaw("Horizontal");                                                      //取得水平輸入值

        Vector3 move = new Vector3(ad * speed, rig.velocity.y, ws * speed);                             //賦予x z軸的向量
        move = Vector3.ClampMagnitude(move, speed);                                                     //斜方向的移動距離會超出speed的設定 所以要限制
        move.y = rig.velocity.y;                                                                        //可能會遇到斜坡問題 必須將y軸回復
        Vector3 修正後的move = CameraRotate.instance.transform.TransformDirection(move);                 //以camerarotate的面向做參考 改變角色的前進方向

        rig.velocity = 修正後的move;

        //滑鼠左鍵點擊發射子彈
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                                //射出一條雷射 起點是main camera 終點是滑鼠點擊的地方
            bool 是否有點到地板 = Physics.Raycast(ray, out raycastHit, 999, 滑鼠可以點擊的圖層);           //如果打到滑鼠可以點擊的圖層就true
            Vector3 目標位置 = raycastHit.point;                                                         //子彈的飛行目標為雷射打擊到的有效點

            //命令所有鏡像(包括自己)使用shoot方法發射子彈 傳入 子彈起點與終點(滑鼠點下的位置)
            photonView.RPC("Shoot", RpcTarget.All, this.transform.position + (Vector3.up * 2f), 目標位置);
        }
    }

    /// <summary>
    /// 射出子彈
    /// </summary>
    /// <param name="startPos">子彈起點</param>
    /// <param name="targetPos">目標點</param>
    /// <param name="info">伺服器上被傳輸的信息的基本資料</param>
    [PunRPC]
    void Shoot(Vector3 startPos,Vector3 targetPos,PhotonMessageInfo info)
    {
        GameObject 砲彈 = Instantiate(魔法砲彈, startPos, Quaternion.identity);       //生成砲彈

        double lag多久 = PhotonNetwork.Time - info.SentServerTime;                   //我當前伺服器的時間 - 訊息送出的時間 = lag時長

        BulletControl 砲彈的程式 = 砲彈.GetComponent<BulletControl>();

        //同步延遲的子彈
        砲彈的程式.設定目標(targetPos);
        砲彈的程式.補償時間((float)lag多久);
    }

    /// <summary>
    /// 角色旋轉
    /// </summary>
    void Rotate()
    {
        rotateRoot.rotation = Quaternion.Lerp(rotateRoot.rotation, moveQuaternion, Time.deltaTime * 20f);
    }

    /// <summary>
    /// 動畫更新
    /// </summary>
    void AniUpdate()
    {
        float 現在的速度與上限的百分比 = moveSpeed / speed;

        //如果我不是本尊 就使用緩衝過的動畫數值
        if (photonView.IsMine == false)
        {
            鏡像動畫緩衝 = Mathf.Lerp(鏡像動畫緩衝, 現在的速度與上限的百分比, Time.deltaTime * 5f);
            if (衣服系統.動畫系統 != null)
            {
                衣服系統.動畫系統.SetFloat("Speed", 鏡像動畫緩衝);
            }
        }
        else
        {
            if (衣服系統.動畫系統 != null)
            {
                衣服系統.動畫系統.SetFloat("Speed", 現在的速度與上限的百分比);
            }
        }

        
    }
    #endregion
}
