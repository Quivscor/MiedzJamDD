using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deselect : MonoBehaviour, IPointerClickHandler, ISelectHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        CityDirector.Instance.DeselectCurrentBuilding();
    }
}
