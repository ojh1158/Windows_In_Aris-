using System;
using System.Collections.Generic;

namespace Script.Data
{
    public enum DialogueType
    {
        Normal,
        PickUp,
        Fight,
        Other,
        Relation,
        MemoReal,
        Anniversary
    }
    [Serializable]
    public class DialogueData
    {
        public DialogueType dialogueType;
        public List<string> text;
    }
}
