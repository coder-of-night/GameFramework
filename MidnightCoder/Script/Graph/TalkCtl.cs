using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using MidnightCoder.Game;

public class TalkCtl : MonoBehaviour
{
    public ConversationManager talkManager;

    public TalkLevelEntity talk;

    void Awake()
    {
        talk = JsonMapper.ToObject<TalkLevelEntity>(Resources.Load<TextAsset>("Config/Talk").text);
        BuildTalkRelative();
    }

    void Start()
    {
        //测试
        //Debug.LogError(FindNodeById(1, "P_4").Value);
    }

    /// <summary>
    /// 反序列化得到数据后执行，构建对话之间的联系
    /// </summary>
    private void BuildTalkRelative()
    {
        talkManager = new ConversationManager();
        for (int m = 0; m < talk.Level.Count; m++)
        {
            List<ConversationNode> allConversationNodeList = talk.Level[m].Talk;
            for (int i = 0; i < allConversationNodeList.Count; i++)
            {
                ConversationNode cn = allConversationNodeList[i];
                if (cn.NextId?.Count > 0)
                {
                    for (int j = 0; j < cn.NextId.Count; j++)
                    {
                        ConversationNode find = FindNodeById(m, cn.NextId[j]);
                        if (find != null)
                        {
                            cn.AddNextNode(find);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 根据Id查找对话信息
    /// </summary>
    /// <param name="lv">关卡</param>
    /// <param name="id">对话Id</param>
    /// <returns></returns>
    public ConversationNode FindNodeById(int lv, string id)
    {
        List<ConversationNode> allConversationNodeList = talk.Level[lv].Talk;
        for (int i = 0; i < allConversationNodeList.Count; i++)
        {
            ConversationNode cn = allConversationNodeList[i];
            if (cn.Id == id)
            {
                return cn;
            }
        }
        return null;
    }
}

[Serializable]
public class TalkLevelEntity
{
    public List<TalkEntity> Level;
}
[Serializable]
public class TalkEntity
{
    public string Name;
    public List<ConversationNode> Talk;
}