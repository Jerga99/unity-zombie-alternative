using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    public Transform FollowTo;
    public Vector3 Offset;
    public float CameraDistance;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            FollowTo.position.x + Offset.x,
            FollowTo.position.y + Offset.y,
            CameraDistance
        );
    }
}

