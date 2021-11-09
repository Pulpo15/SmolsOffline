using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndlessTerrain : MonoBehaviour {
    public const float maxViewDst = 450;
    public Transform viewer;
    public Material mapMaterial;

    public static Vector2 viewerPosition;

    private static MapGenerator _mapGenerator;

    private int _chunkSize;
    private int _chunksVisibleInViewDst;

    private Dictionary<Vector2, TerrainChunk> _terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    private List<TerrainChunk> _terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    void Start() {
        _mapGenerator = FindObjectOfType<MapGenerator>();
        _chunkSize = MapGenerator.mapChunkSize - 1;
        _chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / _chunkSize);
    }

    void Update() {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks() {
        for (int i = 0; i < _terrainChunksVisibleLastUpdate.Count; i++) {
            _terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        _terrainChunksVisibleLastUpdate.Clear();

        int _currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / _chunkSize);
        int _currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / _chunkSize);

        for (int yOffset = -_chunksVisibleInViewDst; yOffset <= _chunksVisibleInViewDst; yOffset++) {
            for (int xOffset = -_chunksVisibleInViewDst; xOffset <= _chunksVisibleInViewDst; xOffset++) {
                Vector2 viewedChunkCoord = new Vector2(_currentChunkCoordX + xOffset, _currentChunkCoordY + yOffset);

                if (_terrainChunkDictionary.ContainsKey(viewedChunkCoord)) {
                    _terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                    if (_terrainChunkDictionary[viewedChunkCoord].IsVisible()) {
                        _terrainChunksVisibleLastUpdate.Add(_terrainChunkDictionary[viewedChunkCoord]);
                    }
                } else {
                    _terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, _chunkSize, transform, mapMaterial));
                }
            }
        }
    }

    public class TerrainChunk {

        GameObject _meshObject;
        Vector2 _position;
        Bounds _bounds;

        MeshRenderer _meshRenderer;
        MeshFilter _meshFilter;

        public TerrainChunk(Vector2 _coord, int _size, Transform _parent, Material _material) {
            _position = _coord * _size;
            _bounds = new Bounds(_position, Vector2.one * _size);
            Vector3 _positionV3 = new Vector3(_position.x, 0, _position.y);

            _meshObject = new GameObject("Terrain Chunk");
            _meshRenderer = _meshObject.AddComponent<MeshRenderer>();
            _meshFilter = _meshObject.AddComponent<MeshFilter>();
            _meshRenderer.material = _material;

            _meshObject.transform.position = _positionV3;
            _meshObject.transform.parent = _parent;
            SetVisible(false);

            _mapGenerator.RequestMapData(OnMapDataReceived);
        }

        void OnMapDataReceived(MapData _mapData) {
            _mapGenerator.RequestMeshData(_mapData, OnMeshDataReceived);
        }

        void OnMeshDataReceived(MeshData _meshData) {
            _meshFilter.mesh = _meshData.CreateMesh();
        }

        public void UpdateTerrainChunk() {
            float _viewerDstFromNearestEdge = Mathf.Sqrt(_bounds.SqrDistance(viewerPosition));
            bool _visible = _viewerDstFromNearestEdge <= maxViewDst;
            SetVisible(_visible);
        }
        public void SetVisible(bool _visible) {
            _meshObject.SetActive(_visible);
        }
        public bool IsVisible() {
            return _meshObject.activeSelf;
        }
    }
}
