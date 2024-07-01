using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;

    [SerializeField] float _speed = 3f;
    [SerializeField] float _jumpForce = 5f;
    [SerializeField] float _dashSpeed = 8f;
    [SerializeField] float _crouchSpeed = 2.5f;
    
    private PlayerState _currentState;
    private bool _isGrounded;

    void Start()
    {
        _currentState = PlayerState.Idle;
    }

    void Update()
    {
        HandleInput();
        HandleState();
    }

    void HandleInput()
    {
        float move = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _currentState = PlayerState.Jumping;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            _currentState = PlayerState.Crouching;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            _currentState = PlayerState.Dashing;
        }
        else if (Mathf.Abs(move) > 0.1f)
        {
            _currentState = PlayerState.Walking;
        }
        else
        {
            _currentState = PlayerState.Idle;
        }
    }

    void HandleState()
    {
        switch (_currentState)
        {
            case PlayerState.Idle:
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                break;
            case PlayerState.Walking:
                float move = Input.GetAxis("Horizontal");
                _rb.velocity = new Vector2(move * _speed, _rb.velocity.y);
                break;
            case PlayerState.Jumping:
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
                _currentState = PlayerState.Idle;
                break;
            case PlayerState.Crouching:
                move = Input.GetAxis("Horizontal");
                _rb.velocity = new Vector2(move * _crouchSpeed, _rb.velocity.y);
                _currentState = PlayerState.Idle;
                break;
            case PlayerState.Dashing:
                move = Input.GetAxis("Horizontal");
                _rb.velocity = new Vector2(move * _dashSpeed, _rb.velocity.y);
                _currentState = PlayerState.Idle;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}
public enum PlayerState
{
    Idle,
    Walking,
    Jumping,
    Crouching,
    Dashing
}