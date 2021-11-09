using UnityEngine;
using System.Collections;

public static class Noise {

    public enum NormalizeMode {Local, Global};

    public static float[,] GenerateNoiseMap(int _mapWidth, int _mapHeight, int _seed, float _scale, int _octaves
        , float _persistance, float _lacunarity, Vector2 _offset, NormalizeMode _normalizedMode) {
        float[,] _noiseMap = new float[_mapWidth, _mapHeight];

        System.Random _prng = new System.Random(_seed);
        Vector2[] _octaveOffsets = new Vector2[_octaves];

        float _maxPossibleHeight = 0;
        float _amplitude = 1;
        float _frequency = 1;

        for (int i = 0; i < _octaves; i++) {
            float _offsetX = _prng.Next(-100000, 100000) + _offset.x;
            float _offsetY = _prng.Next(-100000, 100000) -_offset.y;
            _octaveOffsets[i] = new Vector2(_offsetX, _offsetY);

            _maxPossibleHeight += _amplitude;
            _amplitude *= _persistance;
        }

        if (_scale <= 0) {
            _scale = 0.0001f;
        }

        float _maxLocalNoiseHeight = float.MinValue;
        float _minLocalNoiseHeight = float.MaxValue;

        float _halfWidth = _mapWidth / 2f;
        float _halfHeight = _mapHeight / 2f;


        for (int y = 0; y < _mapHeight; y++) {
            for (int x = 0; x < _mapWidth; x++) {

                _amplitude = 1;
                _frequency = 1;
                float _noiseHeight = 0;

                for (int i = 0; i < _octaves; i++) {
                    float _sampleX = (x - _halfWidth + _octaveOffsets[i].x) / _scale * _frequency;
                    float _sampleY = (y - _halfHeight + _octaveOffsets[i].y) / _scale * _frequency;

                    float _perlinValue = Mathf.PerlinNoise(_sampleX, _sampleY) * 2 - 1;
                    _noiseHeight += _perlinValue * _amplitude;

                    _amplitude *= _persistance;
                    _frequency *= _lacunarity;
                }

                if (_noiseHeight > _maxLocalNoiseHeight) {
                    _maxLocalNoiseHeight = _noiseHeight;
                } else if (_noiseHeight < _minLocalNoiseHeight) {
                    _minLocalNoiseHeight = _noiseHeight;
                }
                _noiseMap[x, y] = _noiseHeight;
            }
        }

        for (int y = 0; y < _mapHeight; y++) {
            for (int x = 0; x < _mapWidth; x++) {
                if (_normalizedMode == NormalizeMode.Local)
                    _noiseMap[x, y] = Mathf.InverseLerp(_minLocalNoiseHeight, _maxLocalNoiseHeight, _noiseMap[x, y]);
                else {
                    float _normalizedHeight = (_noiseMap[x, y] + 1) / (2f * _maxPossibleHeight / 1.75f);
                    _noiseMap[x, y] = Mathf.Clamp(_normalizedHeight, 0, int.MaxValue);
                }
            }
        }

        return _noiseMap;
    }

}