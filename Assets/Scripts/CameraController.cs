using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Transform leftScreenCollider;
    [SerializeField]
    private Transform garbageCollector;
    [SerializeField]
    private GameObject parallaxTransform;
    [SerializeField]
    private float parallaxSpeed;
    private Camera myCamera;
    private float parallaxOffset;
    private float parallaxStartPos;
    private Vector3 cameraLastPos;
    
    // Use this for initialization
    void Start () {
        myCamera = GetComponent<Camera>();
        transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
        
        parallaxOffset = parallaxTransform.GetComponent<Renderer>().bounds.size.x / 3;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
        //Keep the camera moving with the player, only update as it moves right
        if (playerTransform.position.x > transform.position.x)
            transform.position = Vector3.Lerp(transform.position, new Vector3(playerTransform.position.x, transform.position.y, transform.position.z), 0.1f);


        //Keep the collider just off the left of the camera to stop the player going back
        float halfWidth = -(myCamera.aspect * myCamera.orthographicSize);
        leftScreenCollider.position = transform.TransformPoint(new Vector3(halfWidth - 0.5f, 0f, 0f));
        garbageCollector.position = transform.TransformPoint(new Vector3(halfWidth - 2.5f, 0f, 0f));

        if ((transform.position.x - parallaxTransform.transform.position.x) >= parallaxOffset / 2) 
        {
            parallaxTransform.transform.position = new Vector3(parallaxTransform.transform.position.x + parallaxOffset, parallaxTransform.transform.position.y, parallaxTransform.transform.position.z);
        }
        else
        {
            float change = (transform.position.x - cameraLastPos.x) / parallaxSpeed;
            Vector3 newPosition = new Vector3(parallaxTransform.transform.position.x + change, parallaxTransform.transform.position.y, parallaxTransform.transform.position.z);
            parallaxTransform.transform.position = newPosition;
            cameraLastPos = transform.position;
        }
    }
}
