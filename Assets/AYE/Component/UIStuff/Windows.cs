using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Windows<T> : SingletonMonoBehaviour<T> where T : class
{
    [SerializeField] CanvasGroup canvasGroup = null;
    /// <summary>淡入淡出速度</summary>
    [HideInInspector] public float openSpeed = 10f;
    private void Reset()
    {
        if (canvasGroup == null)
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }
    protected override void Awake()
    {
        base.Awake();
        if (canvasGroup == null)
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }
    string ogName = "";
    virtual protected void Start()
    {
        ogName = this.gameObject.name;
        Close();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }
    /// <summary>啟動介面</summary>
    virtual public void Open()
    {
        targetAlpha = 1f;
#if UNITY_EDITOR
        this.gameObject.name = ">>>>>>>" + ogName + "<<<<<<<";
#endif
    }
    /// <summary>關閉介面</summary>
    virtual public void Close()
    {
        targetAlpha = 0f;
        canvasGroup.blocksRaycasts = false;
#if UNITY_EDITOR
        this.gameObject.name = ogName;
#endif
    }
    float targetAlpha = 0f;
    public bool isOpen = false;
    virtual protected void Update()
    {
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.unscaledDeltaTime * openSpeed);
        if (canvasGroup.alpha > 0.9f && isOpen == false)
        {
            isOpen = true;
            canvasGroup.blocksRaycasts = true;
            OnOpen();
        }
        if (canvasGroup.alpha < 0.1f && isOpen == true)
        {
            isOpen = false;
            OnClose();
        }
    }
    /// <summary>充分完成開啟介面</summary>
    virtual public void OnOpen() { }
    /// <summary>充分完成關閉介面</summary>
    virtual public void OnClose() { }
    /// <summary>透明度</summary>
    public float alpha
    {
        get
        {
            if (canvasGroup != null)
                return canvasGroup.alpha;
            else
                return 0f;
        }
    }
}
