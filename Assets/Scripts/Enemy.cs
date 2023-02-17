using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYENavAI;
using Photon.Pun;
using Photon.Realtime;

public class Enemy : AYENavAI<狀態>
{
    #region 欄位
    [SerializeField] float 視野範圍 = 0f;
    [SerializeField] LayerMask 玩家圖層;
    [SerializeField] Transform target = null;
    [SerializeField] float 攻擊距離 = 1.5f;
    [SerializeField] float 巡邏移動速度 = 2f;
    [SerializeField] float 狩獵移動速度 = 4f;
    [SerializeField] Animator 動畫 = null;
    #endregion

    void Awake()
    {
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
        IsMove(true);
        IsWalkOrRun(0);
        speed = 巡邏移動速度;
        Vector3 隨機抽出附近一個可以站的點 = GetRandomXZPos(5f, 3f);
        Goto(隨機抽出附近一個可以站的點, 到目標位置);
    }

    void 到目標位置()
    {
        IsMove(false);
    }

    void 巡邏中()
    {
        if (IsTime(3f))
        {
            status = 狀態.待機;
        }
    }

    void 離開巡邏()
    {
        IsMove(false);
        StopGoto();
    }
    #endregion

    #region 狩獵
    void 狩獵()
    {
        IsMove(true);
        IsWalkOrRun(1);
        speed = 狩獵移動速度;
    }

    void 狩獵中()
    {
        Goto(target.position, 到目標位置);
        if (IsTime(2f))
        {
            status = 狀態.待機;
        }
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
        Attack(true);
    }

    void 攻擊中()
    {
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
    [PunRPC]
    void IsMove(bool v)
    {
        動畫.SetBool("是否移動", v);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("IsMove", RpcTarget.Others, v);
        }
    }

    [PunRPC]
    void IsWalkOrRun(float v)
    {
        動畫.SetFloat("走路或跑步", v);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("IsWalkOrRun", RpcTarget.Others, v);
        }
    }

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

        if (status == 狀態.待機 || status == 狀態.巡邏)
        {
            Collider[] 所見所聞 = Physics.OverlapSphere(this.transform.position, 視野範圍, 玩家圖層);
            if (所見所聞 != null)
            {
                if (所見所聞.Length > 0)
                {
                    target = 所見所聞[0].transform;
                    status = 狀態.狩獵;
                }
            }
        }
    }
    #endregion
}

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
