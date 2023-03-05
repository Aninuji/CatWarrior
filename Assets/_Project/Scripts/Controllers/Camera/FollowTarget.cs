using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Vector2 boundaries;
    // Update is called once per frame
    public void RotateLook(Vector2 playerLook, float speed)
    {
        transform.rotation *= Quaternion.AngleAxis(playerLook.x * speed, Vector3.up);

        transform.rotation *= Quaternion.AngleAxis(playerLook.y * speed, Vector3.right);

        var angles = transform.localEulerAngles;
        angles.z = 0;

        float bondX = 360 - boundaries.x;

        if (angles.x > 180 && angles.x < bondX)
        {
            angles.x = bondX;
        }
        else if (angles.x < 180 && angles.x > boundaries.y)
        {
            angles.x = boundaries.y;
        }

 


        transform.localEulerAngles = angles;
    }
}
