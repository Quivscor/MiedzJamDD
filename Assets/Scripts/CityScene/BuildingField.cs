using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingField : MonoBehaviour, IPointerClickHandler
{
    protected bool m_isEmpty = true;
    public bool IsEmpty => m_isEmpty;

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
