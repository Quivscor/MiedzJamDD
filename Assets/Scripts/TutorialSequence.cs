using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class TutorialSequence
{
    public string name;
    public List<Dialogue> dialogues;
    public bool hasFired = false;
    public int chainSequenceID = -1;

    public bool doEkspedycji = true;
    public bool doMiasta = true;
    public bool sklep = true;
    public bool paczka = true;
    public bool zakonczMiesiac = true;
    public bool misjeZZiemi = true;
}

[System.Serializable]
public class Dialogue
{
    public Sprite dialogueSprite;
    [TextArea(1,4)]
    public string dialogueMsg;
}
