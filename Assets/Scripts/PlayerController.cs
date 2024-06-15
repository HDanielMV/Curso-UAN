using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : character
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    private CameraManager _camera;

    [SerializeField] private float speedTarget;
    [SerializeField] private float speedCurrent;
    [SerializeField] private float speedRunningTarget;
    private Vector2 axis;
    private float refSpeed;
    [SerializeField] private float speedSmooth;
    private float angle;
    [SerializeField] private float rotationSmooth;
    private Quaternion targetRotation;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float maxDistance;
    [SerializeField] private bool IsOnGround;
    private int extraJump = 1;
    [SerializeField] private bool IsRunning;
    [SerializeField] private int health = 100;
    [SerializeField] private bool isDead;


    private void Awake()
    {
        _camera = Camera.main.GetComponent<CameraManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IsOnGround = Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, maxDistance, groundLayer);
        

        if (IsOnGround)
        {
            extraJump = 1;
            axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            IsRunning = Input.GetKey(KeyCode.LeftShift);
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                _animator.SetTrigger("Dodge");
            }
        }

        
        speedCurrent = Mathf.SmoothDamp(speedCurrent, (IsRunning ? speedRunningTarget : speedTarget) * axis.magnitude, ref refSpeed, speedSmooth);
             
       if (axis.magnitude > 0.1)
        {
            angle = Mathf.Atan2(axis.y, -axis.x) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(0, angle + 180 - _camera.Angle, 0);

        }
        if (Input.GetKeyDown(KeyCode.Space) && (IsOnGround || extraJump >= 1)) 
        {
            _rigidbody.AddForce(Vector3.up * jumpForce * _rigidbody.mass);
            extraJump --;
           
        }
       
       
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);
    }
    private IEnumerator Jump()
    {
        
        extraJump = 1;
        _animator.SetTrigger("Jump");
        yield return new WaitForSeconds(1);
    }
    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(transform.forward.x * speedCurrent, _rigidbody.velocity.y, transform.forward.z * speedCurrent);
	}
    private void LateUpdate()
    {
        _animator.SetFloat("Velocity", !IsOnGround ? 0 : (speedCurrent / (IsRunning ? speedRunningTarget : speedTarget)));
        _animator.SetBool("IsRunning", IsRunning);
    }

    
	private void OnDrawGizmos()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + Vector3.down * maxDistance);
	}

    public void ApplyDamage (int damage)
    {
        if (isDead)
        {
            return;
        }
        health -= damage;
        Debug.Log("ApplyDamage");
        if(health <= 0)
        {
            isDead = true;
            _animator.SetTrigger("IsDead");

        }

    }
}
