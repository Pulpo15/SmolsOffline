using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinDungeon : MonoBehaviour {
    public GameObject[] groundTiles;
    public GameObject wall;
    public int mapWidth, mapHeight;

    List<List<int>> _mapTiles = new List<List<int>>();
    const int GROUND_OFFSET = 6;

    private void Start() {
        GenerateMap();
    }

    void GenerateMap() {
        for(int x = 0; x < mapWidth; x++) {
            _mapTiles.Add(new List<int>());
            for(int y = 0; y < mapHeight; y++) {
                int _tileId = Random.Range(0, 4); //GetIdUsingPerlinNoise(x, y);
                _mapTiles[x].Add(_tileId);
                CreateTile(_tileId, x, y);
            }
        }
    }

    int GetIdUsingPerlinNoise(int x, int y) {
        float _rawPerlin = Mathf.PerlinNoise(x, y);
        Debug.Log(_rawPerlin);
        float _clampPerlin = Mathf.Clamp(_rawPerlin, 0.0f, 1.0f);
        float _scaledPerlin = _clampPerlin * groundTiles.Length;
        if(_scaledPerlin == 4) _scaledPerlin = 3;
        

        return Mathf.FloorToInt(_scaledPerlin);
    }

    void CreateTile(int _tileId, int x, int y) {
        GameObject _prefab = groundTiles[_tileId];
        GameObject _tile = Instantiate(_prefab);
        _tile.transform.localPosition = new Vector3(x *= GROUND_OFFSET, 0, y *= GROUND_OFFSET);

        //if(x > 0 && y > 0) {
        //    if(x < _mapTiles.Count - 1 && y < _mapTiles[x].Count - 1) {
        //        if(_mapTiles[x - 1][y] == 0) {
        //            GameObject _wall = Instantiate(wall, _tile.transform.localPosition, _tile.transform.rotation, _tile.transform);
        //        }
        //        if(_mapTiles[x - 1][y - 1] == 0) {
        //            GameObject _wall = Instantiate(wall, _tile.transform.localPosition, _tile.transform.rotation, _tile.transform);
        //        }
        //        if(_mapTiles[x + 1][y - 1] == 0) {
        //            GameObject _wall = Instantiate(wall, _tile.transform.localPosition, _tile.transform.rotation, _tile.transform);
        //        }
        //        if(_mapTiles[x + 1][y + 1] == 0) {
        //            GameObject _wall = Instantiate(wall, _tile.transform.localPosition, _tile.transform.rotation, _tile.transform);
        //        }
        //    }
        //}
    }

}
