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
    [Range(0.0f, 1.0f)][SerializeField] float _breakAtJump;
    float _coyoteTime, _jumpBuffer;
    #endregion

    Vector2 lastMoveDirection;

    [SerializeField] float _aceleration;
    [SerializeField] float _maxSpeed;


    [SerializeField] private Animator animator;

    [SerializeField] private Material[] frontMaterials;  // Materiais padr�o (de frente)
    [SerializeField] private Material[] backMaterials;   // Materiais para as costas

    public SkinnedMeshRenderer _renderer;
    private bool _isFacingBack = false;

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

        
        Vector3 currentVelocity = _playerRB.velocity;

        float currentMaxSpeed = _maxSpeed;
        float currentAceleration = _aceleration;

        // ------------------- checar a dire��o

        float vertical = Input.GetAxisRaw("Vertical");

        if (vertical > 0)
        {
            // Indo pra cima, ent�o mostrar as costas
            _renderer.materials = backMaterials;
            _isFacingBack = true;
        }
        else if (vertical < 0 || vertical == 0)
        {
            // Indo pra baixo, ent�o mostrar a frente
            _renderer.materials = frontMaterials;
            _isFacingBack = false;
        }

        // --------------------------------


        if (moveDirection == Vector2.zero)
        {
            //Jogador n�o est� se movendo
            
            currentAceleration *= 4;

            // Faz o personagem olhar para frente quando parado
            _renderer.materials = frontMaterials;
            _isFacingBack = false;
        }
        else
        {
            //Jogador est� se movendo
           
            lastMoveDirection = moveDirection;
        }

        if (_coyoteTime <= 0)
        {
            currentAceleration *= _breakAtJump;
            currentMaxSpeed *= _breakAtJump;
        }
        

        Vector2 desiredVelocity = moveDirection.normalized * currentMaxSpeed;

        currentVelocity.x = Mathf.Lerp(currentVelocity.x, desiredVelocity.x, currentAceleration * Time.deltaTime);
        currentVelocity.z = Mathf.Lerp(currentVelocity.z, desiredVelocity.y, currentAceleration * Time.deltaTime);

        _playerRB.velocity = currentVelocity;
    }

    private void RotatePlayer(Vector2 moveDirection)
    {
        if (moveDirection.x != 0)
            spriteRenderer.transform.localScale = new Vector3(moveDirection.x, 1, 1);

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
        if (Physics.Raycast(transform.position, Vector3.down, 1.01f, _groundLayerMask))
        {
            //Jogador no ch�o
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

        Vector3 velocity = _playerRB.velocity;

        velocity = new Vector3(velocity.x * _breakAtJump, _jumpForce, velocity.z * _breakAtJump);

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
        if (_isFacingBack)
            animator.SetTrigger("AttackBack");
        else
            animator.SetTrigger("AttackFront");

        

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
