using System.Collections;
using UnityEngine;

public static class FalloffGenerator {

    public static float[,] GenerateFalloffMap(int _size) {
        float[,] _map = new float[_size, _size];

        for (int i = 0; i < _size; i++) {
            for (int j = 0; j < _size; j++) {
                float x = i / (float)_size * 2 - 1;
                float y = j / (float)_size * 2 - 1;

                float _value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                _map[i, j] = Evaluate(_value);
            }
        }
        return _map;
    }

    static float Evaluate(float _value) {
        float a = 3;
        float b = 2.2f;

        return Mathf.Pow(_value, a) / (Mathf.Pow(_value, a) + Mathf.Pow(b - b * _value, a));
    }
}
