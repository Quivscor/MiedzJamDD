using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ExpeditionMap
{
    public class FieldUI : MonoBehaviour
    {
        [Header("References:")]
        public GameObject ui = null;
        public TextMeshProUGUI uiInfoText = null;

        public void ShowFieldInfo(Field field)
        {
            if (uiInfoText && ui)
            {
                uiInfoText.text = field.ToString();
                ui.SetActive(true);
            }
        }

        public void HideFieldInfo()
        {
            ui?.SetActive(false);
        }
    }

}

