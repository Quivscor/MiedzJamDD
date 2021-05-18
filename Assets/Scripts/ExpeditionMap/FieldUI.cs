using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ExpeditionMap
{
    public class FieldUI : MonoBehaviour
    {
        [Header("References:")]
        public GameObject fieldInfoUi = null;
        public GameObject expeditionSendingUI = null;
        public TextMeshProUGUI uiInfoText = null;

        public void ShowFieldInfo(Field field)
        {
            if (uiInfoText && fieldInfoUi)
            {
                uiInfoText.text = field.ToString();
                fieldInfoUi.SetActive(true);
            }
        }

        public void HideFieldInfo()
        {
            fieldInfoUi?.SetActive(false);
        }

        public void ShowExpeditionInfo(bool value)
        {
            expeditionSendingUI?.SetActive(value);
        }
    }

}

