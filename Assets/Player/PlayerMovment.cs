using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Rigidbody2D _rigidbody;
    private int _direction = 1;

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
    }

    private void UpdateVelocity()
    {
        _rigidbody.velocity = Vector2.right * _speed * _direction;
    }

    public void OnChangeDirection(InputValue inputValue)
    {
        ChangeDirection();
    }
}
