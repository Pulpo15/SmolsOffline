using System.Collections;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode {NoiseMap, ColorMap};
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void GenerateMap() {
        float[,] _noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] _colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                float _currentHeight = _noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (_currentHeight <= regions[i].height) {
                        _colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay _display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
            _display.DrawTexture(TextureGenerator.TextureFromHeightMap(_noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            _display.DrawTexture(TextureGenerator.TextureFromColorMap(_colorMap, mapWidth, mapHeight));
    }

    private void OnValidate() {
        if (mapWidth < 1) {
            mapWidth = 1;
            Debug.LogWarning("Map With can't be smaller than 1");
        } if (mapHeight < 1) {
            mapHeight = 1;
            Debug.LogWarning("Map Height can't be smaller than 1");
        } if (lacunarity < 1) {
            lacunarity = 1;
            Debug.LogWarning("Map Lacunarity can't be smaller than 1");
        } if (octaves < 0) {
            octaves = 0;
            Debug.LogWarning("Map Octaves can't be smaller than 0");
        }
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
}