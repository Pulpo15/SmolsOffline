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
    const int CORNER_WALL = 2;

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

            //Instantiate outter walls Horizontal y = 0 *Start*
            Vector3 outterWallsPosition = _mapGrounds[x][0].transform.localPosition;
            if(hLeft) {
                Vector3 vector3 = outterWallsPosition;

                vector3 += new Vector3(-3, 1, -3.5f);
                outterWallsPosition += new Vector3(1, 1, -3.5f);

                _wall = Instantiate(walls[MIDDLE_WALL], vector3,
                _mapGrounds[x][0].transform.rotation, _mapGrounds[x][0].transform);
            } else if(!hLeft) { 
                outterWallsPosition += new Vector3(-1, 1, -3.5f);
            }
            _wall = Instantiate(walls[NORMAL_WALL], outterWallsPosition,
                _mapGrounds[x][0].transform.rotation, _mapGrounds[x][0].transform);
            //Instantiate outter walls Horizontal y = 0 *End*

            //Instantiate outter walls Horizontal y = mapWidth *Start*
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
            //Instantiate outter walls Horizontal y = mapWidth *End*

            bool vLeft = true;
            for(int y = 0; y < mapHeight; y++) {

                //Instantiate outter walls Vertical x = 0 *Start*
                outterWallsPosition = _mapGrounds[0][y].transform.localPosition;

                if(vLeft) {
                    Vector3 vector3 = outterWallsPosition;

                    vector3 += new Vector3(-3.5f, 1, 3);
                    outterWallsPosition += new Vector3(-3.5f, 1, -1);

                    if(x == 0) {
                        _wall = Instantiate(walls[MIDDLE_WALL], vector3,
                            new Quaternion(0, 180, 0, 0), _mapGrounds[0][y].transform);
                        _wall.transform.eulerAngles = new Vector3(0, 90, 0);
                    }
                    //vLeft = false;
                } else if(!vLeft) {
                    outterWallsPosition += new Vector3(-3.5f, 1, 1f);
                    //vLeft = true;
                }
                if (x == 0) {
                    _wall = Instantiate(walls[NORMAL_WALL], outterWallsPosition,
                        Quaternion.identity, _mapGrounds[0][y].transform);
                    _wall.transform.eulerAngles = new Vector3(0, 90, 0);
                }
                //Instantiate outter walls Vertical x = 0 *End*

                //Instantiate outter walls Vertical x = mapHeight *Start*
                outterWallsPosition = _mapGrounds[mapHeight-1][y].transform.localPosition;

                if(vLeft) {
                    Vector3 vector3 = outterWallsPosition;

                    vector3 += new Vector3(3.5f, 1, 3);
                    outterWallsPosition += new Vector3(3.5f, 1, -1);

                    if(x == 0) {
                        _wall = Instantiate(walls[MIDDLE_WALL], vector3,
                            Quaternion.identity, _mapGrounds[mapHeight - 1][y].transform);
                        _wall.transform.eulerAngles = new Vector3(0, -90, 0);
                    }
                    vLeft = false;
                } else if(!vLeft) {
                    outterWallsPosition += new Vector3(3.5f, 1, 1f);
                    vLeft = true;
                }
                if(x == mapHeight-1) {
                    _wall = Instantiate(walls[NORMAL_WALL], outterWallsPosition,
                        Quaternion.identity, _mapGrounds[mapHeight - 1][y].transform);
                    _wall.transform.eulerAngles = new Vector3(0, 90, 0);
                }
                //Instantiate outter walls Vertical x = mapHeight *End*

                //Instantiate inner ground walls
                if(x < mapWidth && y < mapHeight) {
                    if(_mapTiles[x][y] == 0) {
                        Transform _parent = _mapGrounds[x][y].transform;

                        //Up Left Corner
                        Vector3 _position = new Vector3(-4, 1, -4);
                        Quaternion _rotation = new Quaternion(0,0,0,0);

                        _wall = Instantiate(walls[CORNER_WALL],
                            _position,
                            _rotation,
                            _parent);

                        _wall.transform.localPosition = _position;
                        _wall.transform.eulerAngles = new Vector3(_rotation.x, _rotation.y, _rotation.z);

                        //Left Wall
                        _position = new Vector3(0, 1, -4);
                        _rotation = new Quaternion(0, 0, 0, 0);

                        _wall = Instantiate(walls[NORMAL_WALL],
                            _position,
                            _rotation,
                            _parent);

                        _wall.transform.localPosition = _position;
                        _wall.transform.eulerAngles = new Vector3(_rotation.x, _rotation.y, _rotation.z);

                        //Down Left Corner
                        _position = new Vector3(4, 1, -4);
                        _rotation = new Quaternion(0, -90, 0, 0);

                        _wall = Instantiate(walls[CORNER_WALL],
                            _position,
                            _rotation,
                            _parent);

                        _wall.transform.localPosition = _position;
                        _wall.transform.eulerAngles = new Vector3(_rotation.x, _rotation.y, _rotation.z);

                        //Down Wall
                        _position = new Vector3(4, 1, 0);
                        _rotation = new Quaternion(0, -90, 0, 0);

                        _wall = Instantiate(walls[NORMAL_WALL],
                            _position,
                            _rotation,
                            _parent);

                        _wall.transform.localPosition = _position;
                        _wall.transform.eulerAngles = new Vector3(_rotation.x, _rotation.y, _rotation.z);

                        //Down Right Corner
                        _position = new Vector3(4, 1, 4);
                        _rotation = new Quaternion(0, 180, 0, 0);

                        _wall = Instantiate(walls[CORNER_WALL],
                            _position,
                            _rotation,
                            _parent);

                        _wall.transform.localPosition = _position;
                        _wall.transform.eulerAngles = new Vector3(_rotation.x, _rotation.y, _rotation.z);

                        //Right Wall
                        _position = new Vector3(0, 1, 4);
                        _rotation = new Quaternion(0, 0, 0, 0);

                        _wall = Instantiate(walls[NORMAL_WALL],
                            _position,
                            _rotation,
                            _parent);

                        _wall.transform.localPosition = _position;
                        _wall.transform.eulerAngles = new Vector3(_rotation.x, _rotation.y, _rotation.z);

                        //Up Right Corner
                        _position = new Vector3(-4, 1, 4);
                        _rotation = new Quaternion(0, 90, 0, 0);

                        _wall = Instantiate(walls[CORNER_WALL],
                            _position,
                            _rotation,
                            _parent);

                        _wall.transform.localPosition = _position;
                        _wall.transform.eulerAngles = new Vector3(_rotation.x, _rotation.y, _rotation.z);

                        //Up Wall
                        _position = new Vector3(-4, 1, 0);
                        _rotation = new Quaternion(0, -90, 0, 0);

                        _wall = Instantiate(walls[NORMAL_WALL],
                            _position,
                            _rotation,
                            _parent);

                        _wall.transform.localPosition = _position;
                        _wall.transform.eulerAngles = new Vector3(_rotation.x, _rotation.y, _rotation.z);

                        //Vector3 _position = Vector3.zero;
                        //Quaternion _rotation = Quaternion.identity;
                        //Transform _parent = _mapGrounds[x][y].transform;

                        ////Up Left Corner
                        //_position = new Vector3(-2, 1, -2);
                        //_rotation = new Quaternion(0, 0, 0, 0);

                        //_wall = Instantiate(walls[CORNER_WALL],
                        //    _position,
                        //    _rotation,
                        //    _parent);

                        //_wall.transform.localPosition = _position;
                        //_wall.transform.eulerAngles = new Vector3(_rotation.x, _rotation.y, _rotation.z);

                        ////Down Left Corner
                        //_position = new Vector3(2, 1, -2);
                        //_rotation = new Quaternion(0, -90, 0, 0);

                        //_wall = Instantiate(walls[CORNER_WALL],
                        //    _position,
                        //    _rotation,
                        //    _parent);

                        //_wall.transform.localPosition = _position;
                        //_wall.transform.eulerAngles = new Vector3(_rotation.x, _rotation.y, _rotation.z);

                        ////Down Right Corner
                        //_position = new Vector3(2, 1, 2);
                        //_rotation = new Quaternion(0, 180, 0, 0);

                        //_wall = Instantiate(walls[CORNER_WALL],
                        //    _position,
                        //    _rotation,
                        //    _parent);

                        //_wall.transform.localPosition = _position;
                        //_wall.transform.eulerAngles = new Vector3(_rotation.x, _rotation.y, _rotation.z);

                        ////Up Right Corner
                        //_position = new Vector3(-2, 1, 2);
                        //_rotation = new Quaternion(0, 90, 0, 0);

                        //_wall = Instantiate(walls[CORNER_WALL],
                        //    _position,
                        //    _rotation,
                        //    _parent);

                        //_wall.transform.localPosition = _position;
                        //_wall.transform.eulerAngles = new Vector3(_rotation.x, _rotation.y, _rotation.z);
                    }
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
