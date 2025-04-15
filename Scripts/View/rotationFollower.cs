using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationFollower : MonoBehaviour
{
    public Transform Target;
    // Update is called once per frame
    private void Update()
    {
        transform.rotation = Target.rotation;
    }
}
