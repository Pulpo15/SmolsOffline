using System.Collections;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode {NoiseMap, ColorMap, Mesh};
    public DrawMode drawMode;

    public const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void DrawMapInEditor() {
        MapData _mapData = GenerateMapData();

        MapDisplay _display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
            _display.DrawTexture(TextureGenerator.TextureFromHeightMap(_mapData.heightMap));
        else if (drawMode == DrawMode.ColorMap)
            _display.DrawTexture(TextureGenerator.TextureFromColorMap(_mapData.colorMap, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.Mesh)
            _display.DrawMesh(MeshGenerator.GenerateTerrainMesh(_mapData.heightMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), 
                TextureGenerator.TextureFromColorMap(_mapData.colorMap, mapChunkSize, mapChunkSize));
    }

    public MapData GenerateMapData() {
        float[,] _noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] _colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++) {
            for (int x = 0; x < mapChunkSize; x++) {
                float _currentHeight = _noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (_currentHeight <= regions[i].height) {
                        _colorMap[y * mapChunkSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        return new MapData(_noiseMap, _colorMap);
    }

    private void OnValidate() {
        if (lacunarity < 1) {
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

public struct MapData {
    public float[,] heightMap;
    public Color[] colorMap;

    public MapData(float[,] heightMap, Color[] colorMap) {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}