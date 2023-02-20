using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYENavAI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 敵人
/// </summary>
public class Enemy : AYENavAI<狀態>
{
    #region 欄位
    /// <summary>
    /// 狩獵偵測範圍
    /// </summary>
    [SerializeField] float 視野範圍 = 0f;
    /// <summary>
    /// 玩家的圖層 要追蹤的物件圖層
    /// </summary>
    [SerializeField] LayerMask 玩家圖層;
    /// <summary>
    /// 當前的追蹤目標
    /// </summary>
    [SerializeField] Transform target = null;
    /// <summary>
    /// 可以使玩家受傷的距離
    /// </summary>
    [SerializeField] float 攻擊距離 = 1.5f;
    [SerializeField] float 巡邏移動速度 = 2f;
    [SerializeField] float 狩獵移動速度 = 4f;
    [SerializeField] Animator 動畫 = null;
    #endregion

    void Awake()
    {
        //登記狀態
        AddStatus(狀態.未初始化, 未初始化, 未初始化中, 離開未初始化);
        AddStatus(狀態.待機, 待機, 待機中, 離開待機);
        AddStatus(狀態.巡邏, 巡邏, 巡邏中, 離開巡邏);
        AddStatus(狀態.狩獵, 狩獵, 狩獵中, 離開狩獵);
        AddStatus(狀態.攻擊, 攻擊, 攻擊中, 離開攻擊);
        AddStatus(狀態.受傷, 受傷, 受傷中, 離開受傷);
        AddStatus(狀態.死掉, 死掉, 死掉中, 離開死掉);
        AddStatus(狀態.逃跑, 逃跑, 逃跑中, 離開逃跑);
    }

    #region 未初始化
    void 未初始化()
    {

    }

    void 未初始化中()
    {
        //如果我是房長 就使敵人進待機狀態
        if (PhotonNetwork.IsMasterClient == true)
        {
            status = 狀態.待機;
        }
    }

    void 離開未初始化()
    {

    }
    #endregion

    #region 待機
    void 待機()
    {

    }

    void 待機中()
    {
        //在此狀態一段時間就巡邏
        if (IsTime(Random.Range(2f, 5f)))
        {
            status = 狀態.巡邏;
        }
    }

    void 離開待機()
    {

    }
    #endregion

    #region 巡邏
    void 巡邏()
    {
        IsMove(true);                                                   //啟動移動動畫
        IsWalkOrRun(0);                                                 //設定走路跑步動畫
        speed = 巡邏移動速度;                                            //巡邏的速度
        Vector3 隨機抽出附近一個可以站的點 = GetRandomXZPos(5f, 3f);      //取得範圍內的隨機點
        Goto(隨機抽出附近一個可以站的點, 到目標位置);                      //走到(目的地, 結束時要做什麼)
    }

    /// <summary>
    /// 到達位置時取消走路動畫
    /// </summary>
    void 到目標位置()
    {
        IsMove(false);
    }

    void 巡邏中()
    {
        //進入狀態一段時間進入待機
        if (IsTime(3f))
        {
            status = 狀態.待機;
        }
    }

    void 離開巡邏()
    {
        IsMove(false);                  //取消走路動畫
        StopGoto();                     //停止移動
    }
    #endregion

    #region 狩獵
    void 狩獵()
    {
        IsMove(true);                   //啟動走路動畫
        IsWalkOrRun(1);                 //設定走路跑步動畫
        speed = 狩獵移動速度;
    }

    void 狩獵中()
    {
        Goto(target.position, 到目標位置);       //移動至(目的地, 到了之後要做的事);
        //追蹤一段時間後待機
        if (IsTime(2f))
        {
            status = 狀態.待機;
        }

        //我與目標的距離在範圍內就啟動攻擊
        float 我與目標的距離 = Vector3.Distance(this.transform.position, target.position);
        if (我與目標的距離 < 攻擊距離)
        {
            status = 狀態.攻擊;
        }
    }

    void 離開狩獵()
    {
        IsMove(false);
        StopGoto();
    }
    #endregion

    #region 攻擊
    void 攻擊()
    {
        Attack(true);               //啟動攻擊動畫
    }

    void 攻擊中()
    {
        //進入狀態一段時間就待機
        if (IsTime(2.5f))
        {
            status = 狀態.待機;
        }
    }

    void 離開攻擊()
    {
        Attack(false);
    }
    #endregion

    #region 受傷
    void 受傷()
    {

    }

    void 受傷中()
    {

    }

    void 離開受傷()
    {

    }
    #endregion

    #region 死掉
    void 死掉()
    {

    }

    void 死掉中()
    {

    }

    void 離開死掉()
    {

    }
    #endregion

    #region 逃跑
    void 逃跑()
    {

    }

    void 逃跑中()
    {

    }

    void 離開逃跑()
    {

    }
    #endregion

    #region 動畫處理
    /// <summary>
    /// 使用移動動畫
    /// </summary>
    /// <param name="v"></param>
    [PunRPC]
    void IsMove(bool v)
    {
        動畫.SetBool("是否移動", v);
        // 如果我是房長 同步其他敵人鏡像的動畫
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("IsMove", RpcTarget.Others, v);
        }
    }

    /// <summary>
    /// 走路跑步動畫
    /// </summary>
    /// <param name="v"></param>
    [PunRPC]
    void IsWalkOrRun(float v)
    {
        動畫.SetFloat("走路或跑步", v);
        // 如果我是房長 同步其他敵人鏡像的動畫
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("IsWalkOrRun", RpcTarget.Others, v);
        }
    }

    /// <summary>
    /// 攻擊動畫
    /// </summary>
    /// <param name="v"></param>
    [PunRPC]
    void Attack(bool v)
    {
        if (v == true)
        {
            動畫.SetTrigger("普通攻擊");
        }
        else
        {
            動畫.ResetTrigger("普通攻擊");
        }
        
        // 如果我是房長 同步其他敵人鏡像的動畫
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("Attack", RpcTarget.Others, v);
        }
    }
    #endregion

    #region 視線檢定
    protected override void FixedUpdate30()
    {
        base.FixedUpdate30();

        //當在待機或巡邏狀態時 尋找範圍內是"玩家圖層"的物件
        if (status == 狀態.待機 || status == 狀態.巡邏)
        {
            Collider[] 所見所聞 = Physics.OverlapSphere(this.transform.position, 視野範圍, 玩家圖層);

            //如果發現東西
            if (所見所聞 != null)
            {
                if (所見所聞.Length > 0)
                {
                    target = 所見所聞[0].transform;     //將移動目標設為發現的東西陣列的第一個 (發現的第一個玩家)
                    status = 狀態.狩獵;                 //進入狩獵
                }
            }
        }
    }
    #endregion
}

/// <summary>
/// AI的狀態機
/// </summary>
public enum 狀態
{
    未初始化,
    待機,
    巡邏,
    狩獵,
    攻擊,
    受傷,
    死掉,
    逃跑,
}
