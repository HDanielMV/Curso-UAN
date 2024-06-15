using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform tTarget;
    private Vector3 offset;
    private Vector3 refVelocity;
    [SerializeField] private float smooth;
    [SerializeField] private float smoothRotation;
    [SerializeField] private float angle = -270;

    public float Angle
    {
        private set => angle = value;
        get => angle;
    }

    [SerializeField] private float distance = 2;
    [SerializeField] private float height = 2;

    void Start()
    {
        tTarget = GameObject.FindWithTag("Player").transform;
        
        
    }

    
    void Update()
    {
        
        angle -= Input.GetAxis("Mouse X");
        distance -= Input.mouseScrollDelta.y / 10f;
        distance = Mathf.Clamp(distance, 1, 4);
        Vector3 pos = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector3 targetPosition = tTarget.position + pos * distance + Vector3.up * height;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref refVelocity, smooth * Time.deltaTime);
        
        Vector3 dir = (tTarget.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.Euler(25, Mathf.Atan2(-dir.z, dir.x) * Mathf.Rad2Deg + 90, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * smoothRotation);

    }
}
