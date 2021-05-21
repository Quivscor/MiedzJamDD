using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialSequence : ScriptableObject
{
    public List<Dialogue> dialogues;
}

[System.Serializable]
public class Dialogue
{
    public Sprite dialogueSprite;
    [TextArea(1,4)]
    public string dialogueMsg;
}
