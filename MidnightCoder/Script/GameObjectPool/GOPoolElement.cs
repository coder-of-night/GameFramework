using System;
using UnityEngine;

namespace MidnightCoder.Game
{
    public class GOPoolElement
    {
        //
        // Fields
        //
        public bool isUsed;

        public GameObject go;

        //
        // Constructors
        //
        public GOPoolElement(GameObject _go)
        {
            this.go = _go;
        }
    }
}
