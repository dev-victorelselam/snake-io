using System;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Context
{
    public class SpawnPointsList : MonoBehaviour
    {
        public SpawnPoints[] Spawns;
        
        [Serializable]
        public class SpawnPoints
        {
            public List<SpawnPoint> Points => Random.Range(0, 100) % 2 == 0 ? 
                    new List<SpawnPoint>{Spawn1, Spawn2} : 
                    new List<SpawnPoint>{Spawn2, Spawn1};

            public SpawnPoint Spawn1;
            public SpawnPoint Spawn2;
        }
    }
}