using System;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace PMR.GraphEditor.Utilities
{
    [Serializable]
    public class PMRVector2
    {
        public float X { get; set; }
        public float Y { get; set; }
        
        [JsonConstructor]
        public PMRVector2 (float x, float y)
        {
            X = x;
            Y = y;
        }
        public PMRVector2(Vector2 fromVector)
        {
            X = fromVector.x;
            Y = fromVector.y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
    }
}
