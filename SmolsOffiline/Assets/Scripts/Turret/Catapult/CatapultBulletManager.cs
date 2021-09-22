using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultBulletManager : MonoBehaviour {

    public GameObject impactPrefab;
    public List<GameObject> trails; 

    private void OnCollisionEnter(Collision other) {
        
        if (other.collider.tag == "Ground") {
            ContactPoint _contact = other.contacts[0];
            Quaternion _rot = Quaternion.FromToRotation(Vector3.up, _contact.normal);
            Vector3 _pos = _contact.point;

            if (impactPrefab != null) {
                GameObject _impactVFX = Instantiate(impactPrefab, _pos, _rot);
                Destroy(_impactVFX, 5);
            }
             
            if (trails.Count > 0) {
                for (int i = 0; i < trails.Count; i++) {
                    trails[i].transform.parent = null;
                    ParticleSystem _ps = trails[i].GetComponent<ParticleSystem>();
                    if (_ps != null) {
                        _ps.Stop();
                        Destroy(_ps.gameObject, _ps.main.duration + _ps.main.startLifetime.constantMax);
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}
