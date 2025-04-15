using System.Linq.Expressions;
using UnityEngine;

public class perkMove : MonoBehaviour
{
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, Time.time * 100f, 0);
    }
}
