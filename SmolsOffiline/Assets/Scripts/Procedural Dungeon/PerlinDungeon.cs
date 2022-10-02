using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinDungeon : MonoBehaviour {
    public GameObject[] groundTiles;
    public GameObject[] walls;
    public int mapWidth, mapHeight;

    List<List<int>> _mapTiles = new List<List<int>>(); // 0 = Empty, 1 = Ground, 2 = Cracked Ground, 3 = Cracked Ground2
    List<List<GameObject>> _mapGrounds = new List<List<GameObject>>();
    const int GROUND_OFFSET = 6;
    const int NORMAL_WALL = 0;
    const int MIDDLE_WALL = 1;

    private void Start() {
        GenerateMap();
    }

    void GenerateMap() {
        for(int x = 0; x < mapWidth; x++) {
            _mapTiles.Add(new List<int>());
            _mapGrounds.Add(new List<GameObject>());
            for(int y = 0; y < mapHeight; y++) {
                int _tileId = Random.Range(0, 4); //GetIdUsingPerlinNoise(x, y);
                _mapTiles[x].Add(_tileId);
                _mapGrounds[x].Add(CreateTile(_tileId, x, y));
            }
        }
        CreateWalls();
    }

    int GetIdUsingPerlinNoise(int x, int y) {
        float _rawPerlin = Mathf.PerlinNoise(x, y);
        Debug.Log(_rawPerlin);
        float _clampPerlin = Mathf.Clamp(_rawPerlin, 0.0f, 1.0f);
        float _scaledPerlin = _clampPerlin * groundTiles.Length;
        if(_scaledPerlin == 4) _scaledPerlin = 3;
        

        return Mathf.FloorToInt(_scaledPerlin);
    }

    void CreateWalls() {
        GameObject _wall;
        bool hLeft = false;
        for(int x = 0; x < mapWidth; x++) {

            Vector3 outterWallsPosition = _mapGrounds[x][0].transform.localPosition;
            if(hLeft) {
                Vector3 vector3 = outterWallsPosition;

                vector3 += new Vector3(-3, 1, -3.5f);
                outterWallsPosition += new Vector3(1, 1, -3.5f);

                _wall = Instantiate(walls[MIDDLE_WALL], vector3,
                _mapGrounds[x][0].transform.rotation, _mapGrounds[x][0].transform);
                //left = false;
            } else if(!hLeft) { 
                outterWallsPosition += new Vector3(-1, 1, -3.5f); 
                //left = true;
            }
            _wall = Instantiate(walls[NORMAL_WALL], outterWallsPosition,
                _mapGrounds[x][0].transform.rotation, _mapGrounds[x][0].transform);

            outterWallsPosition = _mapGrounds[x][mapWidth - 1].transform.localPosition;
            if(hLeft) {
                Vector3 vector3 = outterWallsPosition;

                vector3 += new Vector3(-3, 1, 3.5f);
                outterWallsPosition += new Vector3(1, 1, 3.5f);

                _wall = Instantiate(walls[MIDDLE_WALL], vector3,
                new Quaternion(0, 180, 0, 0), _mapGrounds[x][mapWidth - 1].transform);
                hLeft = false;
            } else if(!hLeft) {
                outterWallsPosition += new Vector3(-1, 1, 3.5f);
                hLeft = true;
            }
            _wall = Instantiate(walls[NORMAL_WALL], outterWallsPosition,
                _mapGrounds[x][mapWidth - 1].transform.rotation, _mapGrounds[x][mapWidth - 1].transform);

            bool vLeft = false;
            for(int y = 0; y < mapHeight; y++) {

                outterWallsPosition = _mapGrounds[0][y].transform.localPosition;

                if(vLeft) {
                    Vector3 vector3 = outterWallsPosition;

                    vector3 += new Vector3(-3.5f, 1, 3);
                    outterWallsPosition += new Vector3(-3.5f, 1, -1);

                    _wall = Instantiate(walls[MIDDLE_WALL], vector3,
                    new Quaternion(0, 180, 0, 0), _mapGrounds[0][y].transform);
                    //left = false;
                } else if(!vLeft) {
                    outterWallsPosition += new Vector3(-3.5f, 1, -3f);
                    //left = true;
                }
                _wall = Instantiate(walls[NORMAL_WALL], outterWallsPosition,
                    new Quaternion(0, 180, 0, 0), _mapGrounds[0][y].transform);

                //_wall = Instantiate(walls[NORMAL_WALL], _mapGrounds[mapHeight - 1][y].transform.localPosition,
                //    _mapGrounds[mapHeight - 1][y].transform.rotation, _mapGrounds[0][mapHeight - 1].transform);

                if(x < mapWidth && y < mapHeight) {
                    if(_mapTiles[x][y] == 0) {
                        _wall = Instantiate(walls[NORMAL_WALL], _mapGrounds[x][y].transform.localPosition,
                            _mapGrounds[x][y].transform.rotation, _mapGrounds[x][y].transform);
                    }
                    //if(_mapTiles[x - 1][y] == 0) {
                    //    _wall = Instantiate(wall, _mapGrounds[x][y].transform.localPosition,
                    //        _mapGrounds[x][y].transform.rotation, _mapGrounds[x][y].transform);
                    //}
                    //if(_mapTiles[x - 1][y - 1] == 0) {
                    //    _wall = Instantiate(wall, _mapGrounds[x][y].transform.localPosition,
                    //        _mapGrounds[x][y].transform.rotation, _mapGrounds[x][y].transform);
                    //}
                    //if(_mapTiles[x + 1][y - 1] == 0) {
                    //    _wall = Instantiate(wall, _mapGrounds[x][y].transform.localPosition,
                    //        _mapGrounds[x][y].transform.rotation, _mapGrounds[x][y].transform);
                    //}
                }
            }
        }
    }

    GameObject CreateTile(int _tileId, int x, int y) {
        GameObject _prefab = groundTiles[_tileId];
        GameObject _tile = Instantiate(_prefab);
        _tile.transform.localPosition = new Vector3(x *= GROUND_OFFSET, 0, y *= GROUND_OFFSET);
        return _tile;
        

        //if (x + 1 < mapWidth && y + 1 < mapHeight) {
        //    if(_mapTiles[x + 1][y + 1] == 0) {
        //        _wall = Instantiate(
        //            wall, _tile.transform.localPosition, _tile.transform.rotation, _tile.transform);
        //    }
        //}
       
        //if(x > 0 && y > 0) {
        //    if(x < _mapTiles.Count - 1 && y < _mapTiles[x].Count - 1) {
        //        if(_mapTiles[x - 1][y] == 0) {
        //            _wall = Instantiate(
        //                wall, _tile.transform.localPosition, _tile.transform.rotation, _tile.transform);
        //        }
        //        if(_mapTiles[x - 1][y - 1] == 0) {
        //            _wall = Instantiate(
        //                wall, _tile.transform.localPosition, _tile.transform.rotation, _tile.transform);
        //        }
        //        if(_mapTiles[x + 1][y - 1] == 0) {
        //            _wall = Instantiate(
        //                wall, _tile.transform.localPosition, _tile.transform.rotation, _tile.transform);
        //        }
        //        if(_mapTiles[x + 1][y + 1] == 0) {
        //            _wall = Instantiate(
        //                wall, _tile.transform.localPosition, _tile.transform.rotation, _tile.transform);
        //        }
        //    }
        //}
    }

}
