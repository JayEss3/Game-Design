using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    [SerializeField] private float RotationSpeed;
    [SerializeField] private Vector3 Direction;
    void Update()
    {
        transform.Rotate(Direction * (RotationSpeed * Time.deltaTime));
    }
}
