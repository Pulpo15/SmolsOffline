using UnityEngine;

public class Waypoints : MonoBehaviour {

    [SerializeField]
    public static Transform[,] waypoints;

    private void Awake() {       
        waypoints = new Transform[transform.childCount, transform.GetChild(0).childCount];
        for (int i = 0; i < transform.childCount; i++) {
            for (int j = 0; j < transform.GetChild(i).childCount; j++) {
                waypoints[i,j] = transform.GetChild(i).gameObject.transform.GetChild(j);
            }
        }
    }

}
