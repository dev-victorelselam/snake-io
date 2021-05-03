using System;
using GameActors;
using GameActors.Blocks;
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