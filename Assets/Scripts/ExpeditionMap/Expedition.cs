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
    }
}
