using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollowing : MonoBehaviour
{

    public Transform TargetTransform;
    public Vector3 Offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = TargetTransform.position + Offset;
    }
}
