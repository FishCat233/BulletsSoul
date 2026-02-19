using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float normalMoveSpeed = 1f;
    [SerializeField] private float sprintMoveSpeed = 1.5f;

    private float currentMoveSpeed = 0f;

    private Vector2 _inputDirection;
    private bool _inputSprint;

    private void Update()
    {
        HandleInput();

        HandleMove();
    }

    private void HandleInput()
    {
        _inputDirection = new Vector2();
        if (Keyboard.current.wKey.isPressed) _inputDirection.y = 1;
        if (Keyboard.current.sKey.isPressed) _inputDirection.y = -1;
        if (Keyboard.current.aKey.isPressed) _inputDirection.x = -1;
        if (Keyboard.current.dKey.isPressed) _inputDirection.x = 1;
        _inputDirection.Normalize();
        
        _inputSprint = Keyboard.current.leftShiftKey.isPressed;
    }

    private void HandleMove()
    {
        currentMoveSpeed = _inputSprint ? sprintMoveSpeed : normalMoveSpeed;
        
        transform.Translate(_inputDirection * (currentMoveSpeed * Time.deltaTime));
    }
}
