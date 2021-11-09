using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour, IPooledObject {
    public int damage;
    public GameObject impact;
    public int diseableTime = 4;
    public ParticleSystem Beam;
    public GameObject Trail;

    private float time;

    public void OnObjectSpawn() {
        time = diseableTime;
        gameObject.SetActive(true);
        Beam.Pause();
        Trail.SetActive(true);
    }

    private void Update() {
        time -= Time.deltaTime;
        if (time <= 0) {
            gameObject.SetActive(false);
            Beam.Pause();
            Trail.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<EnemyHealthManager>().RecieveDamage(damage);
            GameObject _impact = Instantiate(impact, gameObject.transform.position, Quaternion.identity);
            Destroy(_impact, 2);
        }
        if (collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "Player") {
            GameObject _impact = Instantiate(impact, gameObject.transform.position, Quaternion.identity);
            Destroy(_impact,2);
            gameObject.SetActive(false);
        }
    }

}
