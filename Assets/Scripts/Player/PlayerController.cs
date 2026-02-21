using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using R3;

namespace BulletsSoul.Player
{
    public enum PlayerActionState
    {
        Move,
        Rolling,
    }

    public class PlayerController : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;

        [Header("Movement")]
        [SerializeField] private float normalMoveSpeed = 2f;
        [SerializeField] private float sprintMoveSpeed = 2.5f;

        [Header("Rolling")]
        [SerializeField] private float rollingTime = 0.4f;
        [SerializeField] private float rollingMoveSpeed = 3f;
        [SerializeField] private float rollingCooldown = 1f;

        [Header("Stamina")]
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float sprintStaminaCost = 10f;
        [SerializeField] private float rollStaminaCost = 30f;
        [SerializeField] private float staminaRecoveryRate = 15f;

        public ReactiveProperty<float> CurrentStamina { get; private set; }
        public ReactiveProperty<float> MaxStamina { get; private set; }

        private float _currentHealth = 1f;

        private float _currentMoveSpeed;
        private float _currentStamina;

        private float _rollingTimer;
        private float _rollingCooldownTimer;
        private Vector2 _rollingDirection;

        private PlayerActionState _actionState = PlayerActionState.Move;

        private Vector2 _inputDirection;
        private bool _inputSprint;
        private bool _inputRoll;

        private void Awake()
        {
            _currentHealth = maxHealth;
            _currentStamina = maxStamina;
            CurrentStamina = new ReactiveProperty<float>(_currentStamina);
            MaxStamina = new ReactiveProperty<float>(maxStamina);
        }

        private void Update()
        {
            HandleInput();

            HandleTimer();

            switch (_actionState)
            {
                case PlayerActionState.Move:
                    HandleMove();
                    TryStartRoll();
                    break;
                case PlayerActionState.Rolling:
                    HandleRolling();
                    break;
            }
        }

        private void HandleTimer()
        {
            if (_rollingCooldownTimer > 0f)
                _rollingCooldownTimer -= Time.deltaTime;

            if (!_inputSprint && _actionState != PlayerActionState.Rolling)
            {
                _currentStamina = Mathf.Min(_currentStamina + staminaRecoveryRate * Time.deltaTime, maxStamina);
                CurrentStamina.Value = _currentStamina;
            }
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

            _inputRoll = Keyboard.current.spaceKey.wasPressedThisFrame;
        }

        private void HandleMove()
        {
            bool canSprint = _inputSprint && _currentStamina > 0;
            _currentMoveSpeed = canSprint ? sprintMoveSpeed : normalMoveSpeed;

            if (canSprint && _inputDirection != Vector2.zero)
            {
                _currentStamina = Mathf.Max(_currentStamina - sprintStaminaCost * Time.deltaTime, 0f);
                CurrentStamina.Value = _currentStamina;
            }

            transform.Translate(_inputDirection * (_currentMoveSpeed * Time.deltaTime));
        }

        private void TryStartRoll()
        {
            if (_inputRoll && _rollingCooldownTimer <= 0f && _inputDirection != Vector2.zero && _currentStamina >= rollStaminaCost)
            {
                _actionState = PlayerActionState.Rolling;
                _rollingTimer = 0f;
                _rollingCooldownTimer = rollingCooldown;

                _rollingDirection = _inputDirection;
                _currentStamina -= rollStaminaCost;
                CurrentStamina.Value = _currentStamina;
            }
        }

        private void HandleRolling()
        {
            _rollingTimer += Time.deltaTime;

            if (_rollingTimer >= rollingTime)
            {
                _rollingTimer = 0f;
                _actionState = PlayerActionState.Move;
            }

            transform.Translate(_rollingDirection * (rollingMoveSpeed * Time.deltaTime));
        }
    }
}