using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBulletManager : MonoBehaviour {
    public GameObject meteor;
    public List<GameObject> trails;

    private float _timer = 1.5f;
    private float _curTimer;
    private ObjectPooler _objectPooler;

    public void OnObjectSpawn() {
        meteor.SetActive(true);
        _curTimer = 0f;
    }

    private void Start() {
        _objectPooler = ObjectPooler.instance;
    }

    private void Update() {
        if(!meteor.activeSelf) {
            _curTimer += Time.deltaTime;
            if(_curTimer >= _timer) {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision other) {

        if(other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            ContactPoint _contact = other.contacts[0];
            Quaternion _rot = Quaternion.FromToRotation(Vector3.up, _contact.normal);
            Vector3 _pos = new Vector3(_contact.point.x, 0, _contact.point.z);

            _objectPooler.SpawnFromPool("RangedEnemyPoison", _pos, _rot);

            //Destroy(_impactVFX, 5);

            if(trails.Count > 0) {
                for(int i = 0; i < trails.Count; i++) {
                    //trails[i].transform.parent = null;
                    ParticleSystem _ps = trails[i].GetComponent<ParticleSystem>();
                    if(_ps != null) {
                        _ps.Stop();
                        //Destroy(_ps.gameObject, _ps.main.duration + _ps.main.startLifetime.constantMax);
                    }
                }
            }
            meteor.SetActive(false);
        }
    }
}
