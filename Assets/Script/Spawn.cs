using UnityEngine;

public class Spawn : MonoBehaviour
{
    private Transform _Spawn;
    private void Start() {
        _Spawn = GameObject.Find("Spawn").transform;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject != null){
            other.gameObject.transform.position = _Spawn.position;
        }
    }
}
