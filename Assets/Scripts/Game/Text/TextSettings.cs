using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR
{
    [CreateAssetMenu(menuName = "PMR/Data/Text Settings", fileName = "TextSettings", order = 0)]
    public class TextSettings : ScriptableObject
    {
        [Header("Wave")] 
        public float waveLength = 1;
        public float waveHeight = 1;
        public Material waveMaterial;

    }
}
