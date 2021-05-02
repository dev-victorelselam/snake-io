using System;
using GameActors;
using UnityEngine;

namespace Context
{
    [Serializable]
    public class BlockModel
    {
        public BlockType BlockType;
        public GameObject BlockPrefab;
    }
}