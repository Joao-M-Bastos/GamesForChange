using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScpt : MonoBehaviour
{
    Rigidbody _playerRB;
    [SerializeField] GameObject spriteRenderer;

    public LayerMask _groundLayerMask;

    #region JumpValues
    [SerializeField] float _gravityModifierValue, _maxGravityValue;
    [SerializeField] float _jumpForce;
    [SerializeField] float _baseCoyoteTime, _baseJumpBuffer;
    float _coyoteTime, _jumpBuffer;
    #endregion

    Vector2 lastMoveDirection;

    [SerializeField] float _aceleration;
    [SerializeField] float _maxSpeed;


    [SerializeField] private Animator animator;

    [SerializeField] private Material[] frontMaterials;  // Materiais padrão (de frente)
    [SerializeField] private Material[] backMaterials;   // Materiais para as costas

    public SkinnedMeshRenderer _renderer;

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

        RotatePlayer(moveDirection);

        bool isWalking = moveDirection != Vector2.zero;
        animator.SetBool("IsWalking", isWalking);

        Vector2 desiredVelocity = moveDirection.normalized * _maxSpeed;
        Vector3 currentVelocity = _playerRB.velocity;

        float currentAceleration = _aceleration;

        // ------------------- checar a direção

        float vertical = Input.GetAxisRaw("Vertical");

        if (vertical > 0)
        {
            // Indo pra cima, então mostrar as costas
            _renderer.materials = backMaterials;
        }
        else if (vertical < 0)
        {
            // Indo pra baixo, então mostrar a frente
            _renderer.materials = frontMaterials;
        }

        // --------------------------------


        if (moveDirection == Vector2.zero)
        {
            //Jogador não está se movendo
            
            currentAceleration *= 4;
        }
        else
        {
            //Jogador está se movendo
           
            lastMoveDirection = moveDirection;
        }

        currentVelocity.x = Mathf.Lerp(currentVelocity.x, desiredVelocity.x, _aceleration * Time.deltaTime);
        currentVelocity.z = Mathf.Lerp(currentVelocity.z, desiredVelocity.y, _aceleration * Time.deltaTime);

        _playerRB.velocity = currentVelocity;
    }

    private void RotatePlayer(Vector2 moveDirection)
    {
        if (moveDirection.x != 0)
            transform.localScale = new Vector3(moveDirection.x, 1, 1);

        spriteRenderer.transform.rotation = Quaternion.Euler(25, moveDirection.x * -20, 0);
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
            //Jogador no chão
            _coyoteTime = _baseCoyoteTime;
        }
        else
        {
            //Jogador no ar
        }
    }

    public void Jump()
    {
        if (_coyoteTime <= 0 || _jumpBuffer <= 0)
            return;

        //Jogador conseguiu pular

        animator.SetTrigger("Jump");
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

    public void Attack()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        //Jogador atacou
        animator.SetTrigger("Attack");

        Vector3 lastDirection = new Vector3(lastMoveDirection.x,0, lastMoveDirection.y);

        RaycastHit[] hits = Physics.BoxCastAll(transform.position + lastDirection, Vector3.one, Vector3.up, Quaternion.identity);

        if (hits.Length <= 0)
            return;
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out IHitable objHit))
               objHit.TakePlayerHit();
        }
    }
}
