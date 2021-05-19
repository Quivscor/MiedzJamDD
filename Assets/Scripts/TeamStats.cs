using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamStats : MonoBehaviour
{
    public static TeamStats Instance = null;

    private void Awake()
    {
        if (TeamStats.Instance == null)
            TeamStats.Instance = this;
        else
            Destroy(this);
    }

    public int AvailableDistance = 1;
    public int MaximumLoad = 1;
}
