using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "GameEvent", menuName = "Game Event")]
public class GameEventData : ScriptableObject
{
    public string eventTitle;
    [TextArea(4,20)]
    public string eventText;

    //need at least 1
    public List<GameEventChoice> eventChoices;
}

[System.Serializable]
public class GameEventChoice
{
    public string choiceButtonText;
    public string choiceTitle;
    [TextArea(4, 20)]
    public string choiceText;

    public int choiceCost;
    public CategoriesProgressController.ScienceCategory category;
    public int scienceCategoryScorePenalty;
    public int currencyReward;
    public PackageData packageReward;
}