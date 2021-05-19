using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpeditionMap
{
    public class Expedition
    {
        public Field destinationField = null;

        public Expedition(Field field)
        {
            this.destinationField = field;
        }

        public void OnExpeditionFinished()
        {
            Debug.Log("Expedition finished! Better implementation required! :(");
        }
    }
}
