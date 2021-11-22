using System;
using UnityEngine;

namespace Common.Models
{
    [Serializable]
    public class SerializablePosition
    {
        private float X { get; }
        private float Y { get; }
        private float Z { get; }

        public SerializablePosition(Vector3 position)
        {
            X = position.x;
            Y = position.y;
            Z = position.z;
        }

        public Vector3 Get()
        {
            return new Vector3(X, Y, Z);
        }
    }
}