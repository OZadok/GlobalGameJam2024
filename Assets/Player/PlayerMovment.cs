using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Rigidbody2D _rigidbody;
    private float _direction = 1;
    private bool isGrounded;
    public LayerMask GroundCheckLayerMask;
    public Collider2D collider;

    private void Awake()
    {
        Reset();
    }

    private void Reset()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    }

    private void Start()
    {
        UpdateVelocity();
    }
    
    private void ChangeDirection()
    {
        _direction *= -1;
        UpdateVelocity();
        Messenger.Default.Publish(new PlayerChangedDirectionEvent(_direction));
    }

    private void GroundCheck()
    {
        RaycastHit2D[] raycasts = new RaycastHit2D[2];
        // var hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f,GroundCheckLayerMask);
        
        var amount = collider.Cast(Vector2.down,raycasts, 1f);
        // isGrounded = hit;
        isGrounded = amount > 0;
    }

    private void FixedUpdate()
    {
        // var previousGrounded = isGrounded;
        GroundCheck();
        
        if (!isGrounded)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x * 0.9f, _rigidbody.velocity.y);
            // Debug.Log("0 x !!");
            // if (previousGrounded)
            // {
            //     
            //     Debug.Log("0 x !!");
            // }
            return;
        }

        if (_rigidbody.velocity.sqrMagnitude >= _speed * _speed)
        {
            return;
        }

        ChangeDirectionWhenStuck();
        
        UpdateVelocity();
    }

    private float lastStuckTime = float.MaxValue;
    private void ChangeDirectionWhenStuck()
    {
        //detect stuck
        if (!isGrounded)
        {
            return;
        }

        if (_rigidbody.velocity.magnitude < _speed / 4f)
        {
            if (lastStuckTime == float.MaxValue)
            {
                lastStuckTime = Time.time;
            }
            
            if (Time.time - lastStuckTime > 0.15f)
            {
                ChangeDirection();

                lastStuckTime = float.MaxValue;
            }
        }
        
}

    private void UpdateVelocity()
    {
        _rigidbody.velocity = Vector2.right * (_speed * _direction);
    }

    public void OnChangeDirection(InputValue inputValue)
    {
        ChangeDirection();
    }
    
    IEnumerator ChangeSpeed()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
