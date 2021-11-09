using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndlessTerrain : MonoBehaviour {

    const float scale = 1f;

    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    public LODInfo[] detailLevels;
    public static float maxViewDst;

    public Transform viewer;
    public Material mapMaterial;

    public static Vector2 viewerPosition;
    private Vector2 _viewerPositionOld;

    private static MapGenerator _mapGenerator;

    private int _chunkSize;
    private int _chunksVisibleInViewDst;

    private Dictionary<Vector2, TerrainChunk> _terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    private static List<TerrainChunk> _terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    void Start() {
        _mapGenerator = FindObjectOfType<MapGenerator>();

        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        _chunkSize = MapGenerator.mapChunkSize - 1;
        _chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / _chunkSize);
        
        UpdateVisibleChunks();
    }

    void Update() {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z) / scale;

        if ((_viewerPositionOld-viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) {
            _viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
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
                } else {
                    _terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord,_chunkSize, detailLevels, transform, mapMaterial));
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

        LODInfo[] _detailLevels;
        LODMesh[] _lodMeshes;

        MapData _mapData;
        bool _mapDataReceived;
        int _previousLODIndex = -1;

        public TerrainChunk(Vector2 _coord, int _size, LODInfo[] _detailLevels, Transform _parent, Material _material) {
            this._detailLevels = _detailLevels;

            _position = _coord * _size;
            _bounds = new Bounds(_position, Vector2.one * _size);
            Vector3 _positionV3 = new Vector3(_position.x, 0, _position.y);

            _meshObject = new GameObject("Terrain Chunk");
            _meshRenderer = _meshObject.AddComponent<MeshRenderer>();
            _meshFilter = _meshObject.AddComponent<MeshFilter>();
            _meshRenderer.material = _material;

            _meshObject.transform.position = _positionV3 * scale;
            _meshObject.transform.parent = _parent;
            _meshObject.transform.localScale = Vector3.one * scale;
            SetVisible(false);

            _lodMeshes = new LODMesh[_detailLevels.Length];
            for (int i = 0; i < _detailLevels.Length; i++) {
                _lodMeshes[i] = new LODMesh(_detailLevels[i].lod, UpdateTerrainChunk);
            }

            _mapGenerator.RequestMapData(_position, OnMapDataReceived);
        }

        void OnMapDataReceived(MapData _mapData) {
            this._mapData = _mapData;
            _mapDataReceived = true;

            Texture2D _texture = TextureGenerator.TextureFromColorMap(_mapData.colorMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            _meshRenderer.material.mainTexture = _texture;

            UpdateTerrainChunk();
        }

        public void UpdateTerrainChunk() {
            if (_mapDataReceived) {
                float _viewerDstFromNearestEdge = Mathf.Sqrt(_bounds.SqrDistance(viewerPosition));
                bool _visible = _viewerDstFromNearestEdge <= maxViewDst;

                if (_visible) {
                    int _lodIndex = 0;
                    for (int i = 0; i < _detailLevels.Length - 1; i++) {
                        if (_viewerDstFromNearestEdge > _detailLevels[i].visibleDstThreshold) {
                            _lodIndex = i + 1;
                        } else {
                            break;
                        }
                    }
                    if (_lodIndex != _previousLODIndex) {
                        LODMesh _lodMesh = _lodMeshes[_lodIndex];
                        if (_lodMesh.hasMesh) {
                            _previousLODIndex = _lodIndex;
                            _meshFilter.mesh = _lodMesh.mesh;
                        } else if (!_lodMesh.hasRequestedMesh) {
                            _lodMesh.RequestMesh(_mapData);
                        }
                    }
                    _terrainChunksVisibleLastUpdate.Add(this);
                }
                SetVisible(_visible);
            }
        }
        public void SetVisible(bool _visible) {
            _meshObject.SetActive(_visible);
        }
        public bool IsVisible() {
            return _meshObject.activeSelf;
        }
    }

    class LODMesh {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;

        private int _lod;

        System.Action _updateCallback;

        public LODMesh(int _lod, System.Action _updateCallback) {
            this._lod = _lod;
            this._updateCallback = _updateCallback;
        }

        void OnMeshDataReceived(MeshData _meshData) {
            mesh = _meshData.CreateMesh();
            hasMesh = true;

            _updateCallback();
        }

        public void RequestMesh(MapData _mapData) {
            hasRequestedMesh = true;
            _mapGenerator.RequestMeshData(_mapData, _lod, OnMeshDataReceived);
        }

    }

    [System.Serializable]
    public struct LODInfo {
        public int lod;
        public float visibleDstThreshold;

    }

}