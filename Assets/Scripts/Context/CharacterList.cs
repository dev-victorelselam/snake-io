using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Context
{
    /// <summary>
    /// Asset to register all characters (it adds all characters in project automatically)
    /// </summary>
    [CreateAssetMenu(fileName = "Character List", menuName = "Snake/CharacterList")]
    public class CharacterList : ScriptableObject
    {
        public CharacterSettings[] Characters;

        #if UNITY_EDITOR
        public void OnValidate()
        {
            var assets = AssetDatabase.FindAssets($"t:{nameof(CharacterSettings)}");
            Characters = assets
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<CharacterSettings>)
                .ToArray();
        }
        #endif
    }
}