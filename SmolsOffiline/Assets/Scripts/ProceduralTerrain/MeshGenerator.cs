using UnityEngine;
using System.Collections;

public static class MeshGenerator {

    public static MeshData GenerateTerrainMesh(float[,] _heightMap, float _heightMultiplier, AnimationCurve _heightCurve, int _levelOfDetail) {
        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);

        int _meshSimplificationIncrement = (_levelOfDetail == 0) ? 1 : _levelOfDetail * 2;

        int _borderedSize = _heightMap.GetLength(0);
        int _meshSize = _borderedSize - 2 * _meshSimplificationIncrement;
        int _meshSizeUnsimplified = _borderedSize - 2;

        float _topLeftX = (_meshSizeUnsimplified - 1) / -2f;
        float _topLeftZ = (_meshSizeUnsimplified - 1) / 2f;

        
        int _verticesPerLine = (_meshSize - 1) / _meshSimplificationIncrement + 1;

        MeshData _meshData = new MeshData(_verticesPerLine);

        int[,] _vertexIndicesMap = new int[_borderedSize, _borderedSize];
        int _meshVertexIndex = 0;
        int _borderVertexIndex = -1;

        for (int y = 0; y < _borderedSize; y += _meshSimplificationIncrement) {
            for (int x = 0; x < _borderedSize; x += _meshSimplificationIncrement) {
                bool _isBorderVertex = y == 0 || y == _borderedSize - 1 || x == _borderedSize - 1;

                if (_isBorderVertex) {
                    _vertexIndicesMap[x, y] = _borderVertexIndex;
                    _borderVertexIndex++;
                } else {
                    _vertexIndicesMap[x, y] = _meshVertexIndex;
                    _meshVertexIndex++;
                }
            }
        }

        for (int y = 0; y < _borderedSize; y += _meshSimplificationIncrement) {
            for (int x = 0; x < _borderedSize; x += _meshSimplificationIncrement) {
                int _vertexIndex = _vertexIndicesMap[x, y];
                Vector2 _percent = new Vector2((x - _meshSimplificationIncrement) / (float)_meshSize,
                    (y - _meshSimplificationIncrement) / (float)_meshSize);
                float _height = heightCurve.Evaluate(_heightMap[x, y]) * _heightMultiplier;
                Vector3 _vertexPosition = new Vector3(_topLeftX + _percent.x * _meshSizeUnsimplified, 
                    _height, _topLeftZ - _percent.y * _meshSizeUnsimplified);

                _meshData.AddVertex(_vertexPosition, _percent, _vertexIndex);

                if (x < _borderedSize - 1 && y < _borderedSize - 1) {
                    int a = _vertexIndicesMap[x, y];
                    int b = _vertexIndicesMap[x + _meshSimplificationIncrement, y];
                    int c = _vertexIndicesMap[x, y + _meshSimplificationIncrement];
                    int d = _vertexIndicesMap[x + _meshSimplificationIncrement, y + _meshSimplificationIncrement];
                    _meshData.AddTriangle(a, d, c);
                    _meshData.AddTriangle(d, a, b);
                }
                _vertexIndex++;
            }
        }
        return _meshData;
    }
}

public class MeshData {
    Vector3[] _vertices;
    int[] _triangles;
    Vector2[] _uvs;

    Vector3[] _borderVertices;
    int[] _borderTriangles;

    int _triangleIndex;
    int _borderTriangleIndex;

    public MeshData(int _verticesPerLine) {
        _vertices = new Vector3[_verticesPerLine * _verticesPerLine];
        _uvs = new Vector2[_verticesPerLine * _verticesPerLine];
        _triangles = new int[(_verticesPerLine - 1) * (_verticesPerLine - 1) * 6];

        _borderVertices = new Vector3[_verticesPerLine * 4 + 4];
        _borderTriangles = new int[24 * _verticesPerLine];
    }

    public void AddVertex(Vector3 _vertexPosition, Vector2 _uv, int _vertexIndex) {
        if (_vertexIndex < 0) {
            _borderVertices[-_vertexIndex - 1] = _vertexPosition;
        } else {
            _vertices[_vertexIndex] = _vertexPosition;
            _uvs[_vertexIndex] = _uv;
        }
    }

    public void AddTriangle(int a, int b, int c) {
        if (a < 0 || b < 0 || c < 0) {
            _borderTriangles[_borderTriangleIndex] = a;
            _borderTriangles[_borderTriangleIndex + 1] = b;
            _borderTriangles[_borderTriangleIndex + 2] = c;
            _borderTriangleIndex += 3;
        } else {
            _triangles[_triangleIndex] = a;//
            _triangles[_triangleIndex + 1] = b;
            _triangles[_triangleIndex + 2] = c;
            _triangleIndex += 3;
        }
    }

    Vector3[] CalculateNormals() {
        Vector3[] _vertexNormals = new Vector3[_vertices.Length];
        int _triangleCount = _triangles.Length / 3;
        for (int i = 0; i < _triangleCount; i++) {
            int _normalTriangleIndex = i * 3;
            int _vertexIndexA = _triangles[_normalTriangleIndex];
            int _vertexIndexB = _triangles[_normalTriangleIndex + 1];
            int _vertexIndexC = _triangles[_normalTriangleIndex + 2];

            Vector3 _triangleNormal = SurfaceNormalFromIndices(_vertexIndexA, _vertexIndexB, _vertexIndexC);
            _vertexNormals[_vertexIndexA] += _triangleNormal;
            _vertexNormals[_vertexIndexB] += _triangleNormal;
            _vertexNormals[_vertexIndexC] += _triangleNormal;
        }

        int _borderTriangleCount = _borderTriangles.Length / 3;
        for (int i = 0; i < _borderTriangleCount; i++) {
            int _normalTriangleIndex = i * 3;
            int _vertexIndexA = _borderTriangles[_normalTriangleIndex];
            int _vertexIndexB = _borderTriangles[_normalTriangleIndex + 1];
            int _vertexIndexC = _borderTriangles[_normalTriangleIndex + 2];

            Vector3 _triangleNormal = SurfaceNormalFromIndices(_vertexIndexA, _vertexIndexB, _vertexIndexC);
            if (_vertexIndexA >= 0)
                _vertexNormals[_vertexIndexA] += _triangleNormal;
            if (_vertexIndexB >= 0)
                _vertexNormals[_vertexIndexB] += _triangleNormal;
            if (_vertexIndexC >= 0)
                _vertexNormals[_vertexIndexC] += _triangleNormal;
        }

        for (int i = 0; i < _vertexNormals.Length; i++) {
            _vertexNormals[i].Normalize();
        }

        return _vertexNormals;
    }

    Vector3 SurfaceNormalFromIndices(int _indexA, int _indexB, int _indexC) {
        Vector3 _pointA = (_indexA < 0) ? _borderVertices[-_indexA - 1] : _vertices[_indexA];
        Vector3 _pointB = (_indexB < 0) ? _borderVertices[-_indexB - 1] : _vertices[_indexB];
        Vector3 _pointC = (_indexC < 0) ? _borderVertices[-_indexC - 1] : _vertices[_indexC];

        Vector3 _sideAB = _pointB - _pointA;
        Vector3 _sideAC = _pointC - _pointA;

        return Vector3.Cross(_sideAB, _sideAC).normalized;
    }

    public Mesh CreateMesh() {
        Mesh _mesh = new Mesh();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uvs;
        _mesh.normals = CalculateNormals();
        return _mesh;
    }

}