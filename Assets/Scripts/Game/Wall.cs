using System;
using GameActors;
using UnityEngine;

public class Wall : MonoBehaviour, IHittable
{
    public Type Type => GetType();
}
