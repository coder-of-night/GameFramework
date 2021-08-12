using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

[RequireComponent(typeof(CanvasGroup))]
/// <summary>
/// 所有面板的公共父类(面板基类)
/// </summary>
public class BasePanel : MonoBehaviour
{
    public UIPanelType panelType;
    public bool isHide = false;
    public bool isBan = false;

    #region 可重写的方法
    /// <summary>
    /// 界面显示
    /// </summary>
    protected virtual void OnShow()
    {

    }
    /// <summary>
    /// 界面显示动画，没有可不重写
    /// </summary>
    /// <param name="awaiter">动画播放完成后调用awaiter.Complete();</param>
    protected virtual void OnShowAnim(CustomAwaiter awaiter)
    {
        awaiter.Complete();
    }

    /// <summary>
    /// 界面暂停(禁用交互)
    /// </summary>
    protected virtual void OnPause()
    {

    }
    /// <summary>
    /// 界面继续(开启交互)
    /// </summary>
    protected virtual void OnResume()
    {

    }
    /// <summary>
    /// 界面关闭
    /// </summary>
    protected virtual void OnClose()
    {

    }
    /// <summary>
    /// 界面关闭动画，没有可不重写
    /// </summary>
    /// <param name="awaiter">动画播放完成后调用awaiter.Complete();</param>
    protected virtual void OnCloseAnim(CustomAwaiter awaiter)
    {
        awaiter.Complete();
    }
    #endregion

    protected CanvasGroup canvasGroup;
    public async void Show()
    {
        SetShow();
        OnShow();
        await CustomAwaiter.WaitForAction(OnShowAnim);
        SetInput();
    }

    public void Pause()
    {
        SetInput(false);
        OnPause();
    }
    public void Resume()
    {
        SetInput(true);
        OnResume();
    }
    public async void Close()
    {
        SetInput(false);
        await CustomAwaiter.WaitForAction(OnCloseAnim);
        SetShow(false);
        OnClose();
    }
    protected void SetShow(bool flag = true)
    {
        isHide = !flag;
        CheckCanvasGroup();
        this.canvasGroup.alpha = flag ? 1 : 0;
    }
    protected void SetInput(bool flag = true)
    {
        isBan = !flag;
        CheckCanvasGroup();
        this.canvasGroup.interactable = flag;
        this.canvasGroup.blocksRaycasts = flag;
    }
    private void CheckCanvasGroup()
    {
        if (null == this.canvasGroup)
        {
            this.canvasGroup = gameObject.GetComponent<CanvasGroup>();
        }
        if (null == this.canvasGroup)
        {
            this.canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    protected void CloseThis()
    {
        UIManager.S.ClosePanel(this.panelType);
    }

}
