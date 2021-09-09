using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager
{

    public ConversationNode CreateConversationNode(ConversationBelong belong, string id, string text)
    {
        ConversationNode cnode = new ConversationNode(belong, id, text);
        return cnode;
    }

}
