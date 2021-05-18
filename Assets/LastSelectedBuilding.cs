using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastSelectedBuilding : MonoBehaviour
{
    [SerializeField] private Building m_selectedBuilding;
    public Building SelectedBuilding => m_selectedBuilding;

    public void SetBuilding(Building b)
    {
        if (b.IsPlaced)
            return;
        m_selectedBuilding = b;
    }

    public void Deselect()
    {
        m_selectedBuilding = null;
    }
}
