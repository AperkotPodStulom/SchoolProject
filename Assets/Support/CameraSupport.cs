using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Support
{
    public class CameraSupport : MonoBehaviour
    {
        [SerializeField] private float size;

        public float Size
        {
            get { return size; }
        }
    }
}
