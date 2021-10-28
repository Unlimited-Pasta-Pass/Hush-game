using System;
using UnityEngine;

namespace FieldOfView.Models
{
    [Serializable]
    internal class DebugParameters
    {
        [Tooltip("Should the field of view be visualized?")] 
        public bool visualizeFieldOfView = true;
        
        [Tooltip("Mesh Filter component that holds the generated mesh when drawing the field of view")] 
        public MeshFilter viewMeshFilter;
        public Mesh viewMesh;
    }
}