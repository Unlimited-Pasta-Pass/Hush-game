using UnityEditor;
using UnityEngine;

namespace Environment.Passage
{
    [CustomEditor(typeof(SecretPassage))]
    public class SecretPassageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            var script = (SecretPassage) target;
            if (GUILayout.Button("Fetch References"))
            {
                script.Reset();
            }
        }
    }
}
