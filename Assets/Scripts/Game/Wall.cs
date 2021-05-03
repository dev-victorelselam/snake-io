using System;
using GameActors;
using UnityEngine;

namespace Game
{
    public class Wall : MonoBehaviour, IHittable
    {
        public Type Type => GetType();
    }
}
