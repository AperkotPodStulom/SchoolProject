using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Support
{
    public class Vectors
    {
        public static void Normalize(ref Vector3 vectorToNormalize)
        {
            float endVectorLength = Mathf.Sqrt((vectorToNormalize.x * vectorToNormalize.x) + (vectorToNormalize.y * vectorToNormalize.y));
            vectorToNormalize = new Vector3((vectorToNormalize.x / endVectorLength), (vectorToNormalize.y / endVectorLength));
        }
    }
}
