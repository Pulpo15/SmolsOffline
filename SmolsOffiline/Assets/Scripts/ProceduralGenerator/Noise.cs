using System.Collections;
using UnityEngine;

public static class Noise {

    const int MAXRNG = 100000;
    const int MINRNG = -100000;

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
        float[,] _noiseMap = new float[mapWidth, mapHeight];

        System.Random _prng = new System.Random(seed);
        Vector2[] _octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) {
            float _offsetX = _prng.Next(MINRNG, MAXRNG) + offset.x;
            float _offsetY = _prng.Next(MINRNG, MAXRNG) + offset.y;
            _octaveOffsets[i] = new Vector2(_offsetX, _offsetY);

        }

        if (scale <= 0)
            scale = 0.0001f;

        float _maxNoiseHeight = float.MinValue;
        float _minNoiseHeight = float.MaxValue;

        float _halfWidth = mapWidth / 2f;
        float _halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {

                float _amplitude = 1;
                float _frequency = 1;
                float _noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float _sampleX = (x - _halfWidth) / scale * _frequency + _octaveOffsets[i].x;
                    float _sampleY = (y - _halfHeight) / scale * _frequency + _octaveOffsets[i].y;

                    float _perlinValue = Mathf.PerlinNoise(_sampleX, _sampleY) * 2 - 1;
                    _noiseHeight += _perlinValue * _amplitude;

                    _amplitude *= persistance;
                    _frequency *= lacunarity;
                }

                if (_noiseHeight > _maxNoiseHeight)
                    _maxNoiseHeight = _noiseHeight;
                else if (_noiseHeight < _minNoiseHeight)
                    _minNoiseHeight = _noiseHeight;

                _noiseMap[x, y] = _noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                _noiseMap[x, y] = Mathf.InverseLerp(_minNoiseHeight, _maxNoiseHeight, _noiseMap[x, y]);
            }
        }
        return _noiseMap;
    }
}
