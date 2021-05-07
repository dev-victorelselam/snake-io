using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Context
{
    public class SpawnPoints : MonoBehaviour
    {
        public SpawnPoint[] Spawns;
        
        [Serializable]
        public class SpawnPoint
        {
            public List<Transform> Points => Random.Range(0, 100) % 2 == 0 ? 
                    new List<Transform>{Spawn1, Spawn2} : 
                    new List<Transform>{Spawn2, Spawn1};

            public Transform Spawn1;
            public Transform Spawn2;
        }
    }
}