using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform followTarget;
    public float smoothTime = 3;
    private Vector3 velocity;
    public static Vector3 viewPoint;

    private void OnEnable()
    {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        viewPoint = new Vector3(width * 2, height * 2);
    }

    public static bool OnView(Transform obj)
    {
        Transform pos = Camera.main.transform;
        Vector3 max = new Vector3(pos.position.x + 14.25f * 2, pos.position.y + Camera.main.orthographicSize * 2);
        Vector3 min = new Vector3(pos.position.x - 14.25f * 2 , pos.position.y - Camera.main.orthographicSize * 2);

        if (obj.position.x < max.x && obj.position.x > min.x && obj.position.y < max.y && obj.position.y > min.y)
            return true;
        return false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(followTarget.position.x, followTarget.position.y, -10), ref velocity, smoothTime);
    }
}
