using System.Collections;
using UnityEngine;

public class TextureGenerator {
    public static Texture2D TextureFromColorMap(Color[] colorMap, int _width, int _height) {
        Texture2D _texture = new Texture2D(_width, _height);
        _texture.filterMode = FilterMode.Point;
        _texture.wrapMode = TextureWrapMode.Clamp;
        _texture.SetPixels(colorMap);
        _texture.Apply();
        return _texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap) {
        int _width = heightMap.GetLength(0);
        int _height = heightMap.GetLength(1);

        Color[] _colorMap = new Color[_width * _height];
        for (int y = 0; y < _height; y++) {
            for (int x = 0; x < _width; x++) {
                _colorMap[y * _width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        return TextureFromColorMap(_colorMap, _width, _height);
    }
}
