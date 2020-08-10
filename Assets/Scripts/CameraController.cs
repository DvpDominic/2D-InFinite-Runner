using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    private Vector3 lastPlayerPos;
    private float dist;

    // Start is called before the first frame update
    void Start()
    {
        lastPlayerPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        dist = player.transform.position.x - lastPlayerPos.x;
        this.transform.position = new Vector3(this.transform.position.x + dist, this.transform.position.y, this.transform.position.z);
        lastPlayerPos = player.transform.position;  
    }
}
