using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScpt : MonoBehaviour
{
    Rigidbody _playerRB;

    public LayerMask _groundLayerMask;

    #region JumpValues
    [SerializeField] float _gravityModifierValue, _maxGravityValue;
    [SerializeField] float _jumpForce;
    [SerializeField] float _baseCoyoteTime, _baseJumpBuffer;
    float _coyoteTime, _jumpBuffer;
    bool _extraGravity;
    #endregion

    [SerializeField] float _aceleration;
    [SerializeField] float _maxSpeed;

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleTimes();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpBuffer = _baseJumpBuffer;
        }
    }

    public void Walk()
    {
        Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector2 desiredVelocity = moveDirection.normalized * _maxSpeed;
        
        Vector3 currentVelocity = _playerRB.velocity;

        float currentAceleration = _aceleration;

        if (moveDirection == Vector2.zero)
            currentAceleration *= 4;

        currentVelocity.x = Mathf.Lerp(currentVelocity.x, desiredVelocity.x, _aceleration * Time.deltaTime);
        currentVelocity.z = Mathf.Lerp(currentVelocity.z, desiredVelocity.y, _aceleration * Time.deltaTime);

        _playerRB.velocity = currentVelocity;
    }

    public void HandleTimes()
    {
        IsOnGround();

        _coyoteTime -= Time.deltaTime;
        _jumpBuffer -= Time.deltaTime;
    }

    public void IsOnGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f, _groundLayerMask))
        {
            _coyoteTime = _baseCoyoteTime;
        }
    }

    public void Jump()
    {
        Debug.Log(_coyoteTime + " " + _jumpBuffer);

        if (_coyoteTime <= 0 || _jumpBuffer <= 0)
            return;

        _coyoteTime = 0;
        _jumpBuffer = 0;

        Vector2 velocity = _playerRB.velocity;

        velocity.y = _jumpForce;

        _playerRB.velocity = velocity;
    }

    public void HandleGravity()
    {
        if (_coyoteTime > 0)
            return;

        Vector3 velocity = _playerRB.velocity;

        velocity.y += -_gravityModifierValue * Time.deltaTime;

        if(velocity.y < -_maxGravityValue)
        {
            velocity.y = -_maxGravityValue;
        }

        _playerRB.velocity = velocity;
    }
}
