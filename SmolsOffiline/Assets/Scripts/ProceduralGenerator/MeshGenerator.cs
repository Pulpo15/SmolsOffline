using System.Collections;
using UnityEngine;

public static class MeshGenerator {
    public static MeshData GenerateTerrainMesh(float[,] _heightMap, float _heightMultiplier, AnimationCurve _heightCurve, int _levelOfDetail) {
        int _width = _heightMap.GetLength(0);
        int _height = _heightMap.GetLength(1);
        float _topLeftX = (_width - 1) / -2f;
        float _topLeftZ = (_height - 1) / 2f;

        int _meshSimplificationIncrement = (_levelOfDetail == 0) ? 1: _levelOfDetail * 2;
        int _verticesPerLine = (_width - 1) / _meshSimplificationIncrement + 1;

        MeshData _meshData = new MeshData(_verticesPerLine, _verticesPerLine);
        int _vertexIndex = 0;


        for (int y = 0; y < _height; y += _meshSimplificationIncrement) {
            for (int x = 0; x < _width; x += _meshSimplificationIncrement) {

                _meshData.vertices[_vertexIndex] = new Vector3(_topLeftX + x, _heightCurve.Evaluate(_heightMap[x, y]) * _heightMultiplier, _topLeftZ - y);
                _meshData.uvs[_vertexIndex] = new Vector2(x / (float)_width, y / (float)_height);

                if (x < _width - 1 && y < _height - 1) {
                    _meshData.AddTriangle(_vertexIndex, _vertexIndex + _verticesPerLine + 1, _vertexIndex + _verticesPerLine);
                    _meshData.AddTriangle(_vertexIndex + _verticesPerLine + 1, _vertexIndex, _vertexIndex + 1);
                }

                _vertexIndex++;
            }
        }
        return _meshData;
    }
}

public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int _triangleIndex;

    public MeshData(int _meshWidth, int _meshHeight) {
        vertices = new Vector3[_meshWidth * _meshHeight];
        uvs = new Vector2[_meshWidth * _meshHeight];
        triangles = new int[(_meshWidth - 1) * (_meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c) {
        triangles[_triangleIndex] = a;
        triangles[_triangleIndex + 1] = b;
        triangles[_triangleIndex + 2] = c;
        _triangleIndex += 3;
    }

    public Mesh CreateMesh() {
        Mesh _mesh = new Mesh();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.uv = uvs;
        _mesh.RecalculateNormals();
        return _mesh;
    }
}