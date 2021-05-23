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

    public bool doEkspedycji;
    public bool doMiasta;
    public bool sklep;
    public bool paczka;
    public bool zakonczMiesiac;
    public bool misjeZZiemi;
}

[System.Serializable]
public class Dialogue
{
    public Sprite dialogueSprite;
    [TextArea(1,4)]
    public string dialogueMsg;
}
