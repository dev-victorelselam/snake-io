using System;
using UnityEngine;

namespace GameActors.Blocks
{
    public class SpeedBlockView : BlockView
    {
        public float SpeedValue => _boostValue;
        [SerializeField] [Range(0, 20)] float _boostValue;
    }
}