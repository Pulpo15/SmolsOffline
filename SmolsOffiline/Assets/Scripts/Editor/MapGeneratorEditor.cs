using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        MapGenerator _mapGen = (MapGenerator)target;

        if (DrawDefaultInspector()) {
            if (_mapGen.autoUpdate)
                _mapGen.GenerateMap();
        }

        if (GUILayout.Button("Generate"))
            _mapGen.GenerateMap();
    }
}
