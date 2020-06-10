using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    /// Bullet life in seconds
    private static readonly int BULLET_LIFE = 1; 

    public static readonly float BULLET_SPEED = 40f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToDieCoroutine());
    }

    // Update is called once per frame
    void Update() 
    {
        transform.position += transform.forward * BULLET_SPEED * Time.deltaTime;
    }

    private IEnumerator WaitToDieCoroutine() {
        yield return new WaitForSeconds(BULLET_LIFE);
        Destroy(this.gameObject);
    }
}
