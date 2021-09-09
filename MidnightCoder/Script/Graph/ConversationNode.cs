using System.Collections;
using System.Collections.Generic;
using System;
public enum ConversationBelong
{
    Player,
    Npc
}
[Serializable]
public class ConversationNode : GraphNode<string>
{
    public ConversationBelong Belong;
    public ConversationBelong NextBelong;
    public List<string>  NextId;
   [NonSerialized] private List<ConversationNode> NextConversationList;

    public ConversationNode()
    {

    }
  
    public ConversationNode(ConversationBelong _belong, string _id, string _value, params string[] _nextIds) : base(_id, _value)
    {
        Belong = _belong;
        if (_nextIds.Length > 0)
        {
            NextId = new List<string>();
            foreach (string str in _nextIds)
            {
                NextId.Add(str);
            }
        }
    }

    public void AddNextNode(ConversationNode _n)
    {
        if (NextConversationList == null)
        {
            NextConversationList = new List<ConversationNode>();
        }
        NextConversationList.Add(_n);
    }

    public bool HaveNextNode()
    {
        return NextConversationList.Count > 0;
    }

    public List<ConversationNode> GetNextConversationList()
    {
        return NextConversationList;
    }
}
