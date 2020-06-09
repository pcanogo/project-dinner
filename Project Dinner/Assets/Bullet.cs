using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public static readonly float BULLET_SPEED = 40f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() 
    {
        transform.position += transform.forward * BULLET_SPEED * Time.deltaTime;
    }
}
