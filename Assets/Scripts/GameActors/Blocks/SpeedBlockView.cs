using System;
using UnityEngine;

namespace GameActors.Blocks
{
    public class SpeedBlockView : BlockView
    {
        enum SpeedModifierType
        {
            Sum,
            Multiply,
            Power
        }
        
        [SerializeField] [Range(0, 20)] float _boostValue;
        [SerializeField] private SpeedModifierType _type;

        public float Apply(float speed)
        {
            switch (_type)
            {
                case SpeedModifierType.Sum:
                    return speed + _boostValue;
                case SpeedModifierType.Multiply:
                    return speed * _boostValue;
                case SpeedModifierType.Power:
                    return Mathf.Pow(speed, _boostValue);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        } 
    }
}