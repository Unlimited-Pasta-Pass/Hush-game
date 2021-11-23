using System;
using UnityEngine;

namespace Common.Models
{
    [Serializable]
    public class SerializableRotation
    {
        private float X { get; }
        private float Y { get; }
        private float Z { get; }

        public SerializableRotation(Quaternion rotation)
        {
            X = rotation.eulerAngles.x;
            Y = rotation.eulerAngles.y;
            Z = rotation.eulerAngles.z;
        }

        public Quaternion Get()
        {
            return Quaternion.Euler(X, Y, Z);
        }
    }
}