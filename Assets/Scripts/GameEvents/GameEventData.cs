using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "GameEvent", menuName = "Game Event")]
public class GameEventData : ScriptableObject
{
    [TextArea(4,10)]
    public string eventText;

    public bool hasOptions;
    public List<GameEventChoice> eventChoices;
}

[System.Serializable]
public class GameEventChoice
{
    [TextArea(4, 10)]
    public string choiceText;

    public int choiceCost;
    public int currencyReward;
    public PackageData packageReward;
}