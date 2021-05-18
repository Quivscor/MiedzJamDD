using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpeditionMap
{
    public class InputManager : MonoBehaviour
    {
        public ExpeditionMapManager mapManager = null;

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int mouseX = -1000;
            int mouseZ = -1000;

            if (Physics.Raycast(ray, out hit))
            {
                mouseX = (int)hit.point.x;
                mouseZ = (int)hit.point.z;
            }

            Debug.Log(hit.point);
            mapManager?.SetActiveField(mouseX, mouseZ);
        }
    }
}


