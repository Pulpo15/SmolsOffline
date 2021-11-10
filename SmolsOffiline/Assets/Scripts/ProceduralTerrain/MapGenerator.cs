using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode { NoiseMap, ColorMap, Mesh, FallofMap };
    public DrawMode drawMode;

    public Noise.NormalizeMode normalizeMode;

    public const int mapChunkSize = 241;
    [Range(0, 6)]
    public int editorPreviewLOD;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool useFalloff;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;

    float[,] fallofMap;

    private Queue<MapThreadInfo<MapData>> _mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    private Queue<MapThreadInfo<MeshData>> _meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    private void Awake() {
        fallofMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
    }

    public void DrawMapInEditor() {
        MapData _mapData = GenerateMapData(Vector2.zero);

        MapDisplay _display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap) {
            _display.DrawTexture(TextureGenerator.TextureFromHeightMap(_mapData.heightMap));
        } else if (drawMode == DrawMode.ColorMap) {
            _display.DrawTexture(TextureGenerator.TextureFromColorMap(_mapData.colorMap, mapChunkSize, mapChunkSize));
        } else if (drawMode == DrawMode.Mesh) {
            _display.DrawMesh(MeshGenerator.GenerateTerrainMesh(_mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editorPreviewLOD)
                , TextureGenerator.TextureFromColorMap(_mapData.colorMap, mapChunkSize, mapChunkSize));
        } else if (drawMode == DrawMode.FallofMap) {
            _display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));
        }
    }

    public void RequestMapData(Vector2 _center, Action<MapData> _callback) {
        ThreadStart _threadStart = delegate {
            MapDataThread(_center, _callback);
        };

        new Thread(_threadStart).Start();
    }

    void MapDataThread(Vector2 _center, Action<MapData> _callback) {
        MapData _mapData = GenerateMapData(_center);
        lock (_mapDataThreadInfoQueue) {
            _mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(_callback, _mapData));
        }
    }

    public void RequestMeshData(MapData _mapData, int _lod, Action<MeshData> _callback) {
        ThreadStart _threadStart = delegate {
            MeshDataThread(_mapData, _lod, _callback);
        };

        new Thread(_threadStart).Start();
    }

    void MeshDataThread(MapData _mapData,int _lod, Action<MeshData> _callback) {
        MeshData _meshData = MeshGenerator.GenerateTerrainMesh(_mapData.heightMap, meshHeightMultiplier, meshHeightCurve, _lod);
        lock (_meshDataThreadInfoQueue) {
            _meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(_callback, _meshData));
        }
    }

    void Update() {
        if (_mapDataThreadInfoQueue.Count > 0) {
            for (int i = 0; i < _mapDataThreadInfoQueue.Count; i++) {
                MapThreadInfo<MapData> _threadInfo = _mapDataThreadInfoQueue.Dequeue();
                _threadInfo.callback(_threadInfo.parameter);
            }
        }
        if (_meshDataThreadInfoQueue.Count > 0) {
            for (int i = 0; i < _meshDataThreadInfoQueue.Count; i++) {
                MapThreadInfo<MeshData> _threadInfo = _meshDataThreadInfoQueue.Dequeue();
                _threadInfo.callback(_threadInfo.parameter);
            }
        }
    }

    MapData GenerateMapData(Vector2 _center) {
        float[,] _noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, 
            _center + offset, normalizeMode);

        Color[] _colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++) {
            for (int x = 0; x < mapChunkSize; x++) {
                if (useFalloff) {
                    _noiseMap[x, y] = Mathf.Clamp01(_noiseMap[x, y] - fallofMap[x, y]);
                }
                float _currentHeight = _noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (_currentHeight >= regions[i].height) {
                        _colorMap[y * mapChunkSize + x] = regions[i].color;
                    } else {
                        break;
                    }
                }
            }
        }


        return new MapData(_noiseMap, _colorMap);
    }

    void OnValidate() {
        if (lacunarity < 1) {
            lacunarity = 1;
        } if (octaves < 0) {
            octaves = 0;
        }
        fallofMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
    }

    struct MapThreadInfo<T> {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter) {
            this.callback = callback;
            this.parameter = parameter;
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
    public readonly float[,] heightMap;
    public readonly Color[] colorMap;

    public MapData(float[,] heightMap, Color[] colorMap) {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}
