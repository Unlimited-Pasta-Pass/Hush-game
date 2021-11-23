using System;
using UnityEngine;

namespace Common.Models
{
    [Serializable]
    public class SerializableTransform
    {
        public SerializablePosition Position { get; }
        public SerializableRotation Rotation { get; }

        public SerializableTransform(Transform transform)
        {
            Position = new SerializablePosition(transform.position);
            Rotation = new SerializableRotation(transform.rotation);
        }

        public SerializableTransform(Vector3 position, Quaternion rotation)
        {
            Position = new SerializablePosition(position);
            Rotation = new SerializableRotation(rotation);
        }

        public static implicit operator SerializableTransform(Transform t) => new SerializableTransform(t);
    }
}