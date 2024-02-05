using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform followTarget;
    public float smoothTime = 3;
    private Vector3 velocity;

    [HideInInspector]
    public static Vector3 viewPort;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(followTarget.position.x, followTarget.position.y, -10), ref velocity, smoothTime);
        viewPort = new Vector3(transform.position.x + 16, transform.position.y + 5);
    }
}
