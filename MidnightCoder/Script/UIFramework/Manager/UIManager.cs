using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidnightCoder.Game;
using System;
public class UIManager : TMonoSingleton<UIManager>
{

    /// <summary>
    /// 存储所有面板的prefab路径
    /// </summary>
    private Dictionary<UIPanelType, string> panelPathDict;
    /// <summary>
    /// 存储所有实例化出的面板的BasePanel组件(方便取)
    /// </summary>
    private Dictionary<UIPanelType, BasePanel> panelObjectDict;
    /// <summary>
    /// 
    /// </summary>
    // private Stack<BasePanel> panelStack;
    private LinkedList<BasePanel> panelList;
    private Dictionary<UIPanelType, List<UIPanelType>> panelRelativeDict;

    //JsonUtility只能转换为对象,因此写个内部类
    [Serializable]
    class UIPanelTypeJson
    {
        public List<UIPanelInfo> panelInfoList = null;
    }

    void Awake()
    {
        this.panelPathDict = new Dictionary<UIPanelType, string>();
        panelObjectDict = new Dictionary<UIPanelType, BasePanel>();
        panelList = new LinkedList<BasePanel>();
        panelRelativeDict = new Dictionary<UIPanelType, List<UIPanelType>>();
        panelRelativeDict.Add(UIPanelType.MainPanel, new List<UIPanelType>() { UIPanelType.ReadyPopup });
        this.ParseUIPanelTypeJson();
    }
    /*----------------------------------分割线--------------------------------------*/

    /// <summary>
    /// 把json文本内信息转化为json对象
    /// </summary>
    private void ParseUIPanelTypeJson()
    {
        //从Resources中加载UIPanelType文件中所有文本信息
        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");

        //将加载的文本信息转换为json对象,存入实体类,构造json对象列表

        //JsonUtility只能转换为对象
        UIPanelTypeJson panelTypeJson = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);
        //将json对象列表映射入字典中
        foreach (UIPanelInfo item in panelTypeJson.panelInfoList)
        {
            this.panelPathDict.Add(item.panelType, item.path);
        }
    }
    /// <summary>
    /// 实例化指定类型的面板存入字典并返回其BasePanel组件
    /// </summary>
    /// <returns></returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {
        BasePanel panel = panelObjectDict.TryGet(panelType);
        if (null == panel)
        {
            string path = this.panelPathDict.TryGet(panelType);
            GameObject obj = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            obj.transform.SetParent(this.transform, false);
            BasePanel bp = obj.GetComponent<BasePanel>();
            bp.panelType = panelType;
            this.panelObjectDict.Add(panelType, bp);
            return bp;
        }
        else
        {
            return panel;
        }
    }

    public void OpenPanel(UIPanelType panelType, Action callback = null)
    {
        BasePanel panel = this.GetPanel(panelType);
        if (this.panelList.Contains(panel))
        {
            Debug.LogError($"重复开启页面{panelType}");
            return;
        }
        //入栈前栈顶页面不可交互
        if (this.panelList.Count > 0)
        {
            BasePanel topPanel = this.panelList.Last.Value;
            topPanel.Pause();
        }

        this.panelList.AddLast(panel);
        panel.Show();
        callback?.Invoke();
    }
    /// <summary>
    /// 打开页面并建立其上属关联页面
    /// </summary>
    public void OpenRelativePanel(UIPanelType panelType, UIPanelType relativePanel, Action callback = null)
    {
        BasePanel panel = this.GetPanel(panelType);
        if (this.panelList.Contains(panel))
        {
            Debug.LogError($"重复开启页面{panelType}");
            return;
        }
        //入栈前栈顶页面不可交互
        if (this.panelList.Count > 0)
        {
            BasePanel topPanel = this.panelList.Last.Value;
            topPanel.Pause();
        }
        this.panelList.AddLast(panel);
        panel.Show();
        callback?.Invoke();
    }
    public void OpenPopup(UIPanelType panelType, Action callback = null)
    {
        BasePanel panel = this.GetPanel(panelType);
        if (this.panelList.Contains(panel))
        {
            Debug.LogError($"重复开启页面{panelType}");
            return;
        }
        this.panelList.AddLast(panel);
        panel.Show();
        callback?.Invoke();
    }

    /// <summary>
    /// 关闭顶部页面
    /// </summary>
    public void ClosePanel(Action callback = null)
    {
        if (this.panelList.Count <= 0)
        {
            return;
        }
        BasePanel panel = this.panelList.Last.Value;
        UIPanelType panelType = panel.panelType;
        Stack<BasePanel> closeStack = new Stack<BasePanel>();
        panelList.Remove(panel);
        closeStack.Push(panel);
        if (panelRelativeDict.ContainsKey(panelType))
        {
            List<UIPanelType> rpList = panelRelativeDict.TryGet(panelType);
            foreach (UIPanelType item in rpList)
            {
                if (panelObjectDict.ContainsKey(item))
                {
                    BasePanel rp = GetPanel(item);
                    if (panelList.Contains(rp))
                    {
                        panelList.Remove(rp);
                        closeStack.Push(rp);
                    }
                }
            }
        }
        foreach (BasePanel p in closeStack)
        {
            p.Close();
        }
        callback?.Invoke();
        if (this.panelList.Count <= 0)
        {
            return;
        }
        BasePanel topPanel2 = this.panelList.Last.Value;
        topPanel2.Resume();
    }
    /// <summary>
    /// 关闭指定页面（及其下属关联界面）
    /// </summary>
    public void ClosePanel(UIPanelType panelType, Action callback = null)
    {
        if (this.panelList.Count <= 0)
        {
            return;
        }
        BasePanel panel = GetUIPanel(panelType);
        Stack<BasePanel> closeStack = new Stack<BasePanel>();
        if (panel != null && panelList.Contains(panel))
        {
            panelList.Remove(panel);
            closeStack.Push(panel);
        }
        else
        {
            Debug.LogError($"不存在页面{panelType}");
            return;
        }
        if (panelRelativeDict.ContainsKey(panelType))
        {
            List<UIPanelType> rpList = panelRelativeDict.TryGet(panelType);
            foreach (UIPanelType item in rpList)
            {
                if (panelObjectDict.ContainsKey(item))
                {
                    BasePanel rp = GetPanel(item);
                    if (panelList.Contains(rp))
                    {
                        panelList.Remove(rp);
                        closeStack.Push(rp);
                    }
                }
            }
        }
        foreach (BasePanel p in closeStack)
        {
            p.Close();
        }

        callback?.Invoke();
        if (this.panelList.Count <= 0)
        {
            return;
        }
        BasePanel topPanel2 = this.panelList.Last.Value;
        topPanel2.Resume();
    }


    public BasePanel GetUIPanel(UIPanelType panelType)
    {
        BasePanel panel = panelObjectDict.TryGet(panelType);
        return panel;
    }


}
