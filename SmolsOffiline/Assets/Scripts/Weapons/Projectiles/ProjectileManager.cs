using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour, IPooledObject {
    public int damage;
    public GameObject impact;

    public void OnObjectSpawn() {
        StartCoroutine(DiseableProjectile());
    }

    IEnumerator DiseableProjectile() {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
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
