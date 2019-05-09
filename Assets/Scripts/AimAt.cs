using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class AimAt : MonoBehaviour
    {
        public Transform Target;
        public int maxDist = 10;
        public int minDist = 2;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(Target);
            transform.Rotate(0, 180, 0);
        }
    }
}
