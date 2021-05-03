using UnityEngine;

namespace Context
{
    /// <summary>
    /// Main asset for game setup of params
    /// </summary>
    [CreateAssetMenu(fileName = "Game Setup", menuName = "Snake/GameSetup")]
    public class GameSetup : ScriptableObject
    {
        [Header("General Configs")]
        public CharacterSettings[] Characters;
        public KeyCode[] AvailableKeys = {
            KeyCode.Alpha0,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,

            KeyCode.Q,
            KeyCode.W,
            KeyCode.E,
            KeyCode.R,
            KeyCode.T,
            KeyCode.Y,
            KeyCode.U,
            KeyCode.I,
            KeyCode.O,
            KeyCode.P,

            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
            KeyCode.F,
            KeyCode.G,
            KeyCode.H,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,

            KeyCode.Z,
            KeyCode.X,
            KeyCode.C,
            KeyCode.V,
            KeyCode.B,
            KeyCode.N,
            KeyCode.M,
        };
        
        [Header("Snake Gameplay Configs")]
        [Space(10)]
        public GameObject SnakePrefab;
        [Range(0, 10)] public float BaseSpeed;
        [Range(0, 1)] public float LoadedSpeedDecay;
        public BlockModel[] Blocks;
    }
}