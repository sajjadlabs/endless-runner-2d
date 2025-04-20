using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _player;
    public float gravityScale;

    public float speed;
    public float jumpForce;
    private bool _isJumping;
    public float jumpTime;
    private float _jumpTimeCounter;


    private float _moveX;
    private float _moveY;
    private bool _movingRight;
    private bool _movingLeft;

    private bool _facingRight = true;

    private bool _isGrounded;
    public LayerMask ground;
    public Transform groundCheck;
    public float checkRadius;

    public int extraJump;
    private int _extraJumpCount;

    public float distance;
    public LayerMask ladder;
    private bool _isClimbing;

    private void Start()
    {
        _player = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Jump();
    }

    private void Jump()
    {
        _extraJumpCount = _isGrounded ? extraJump : _extraJumpCount;

        if (Input.GetKeyDown(KeyCode.Space) && _extraJumpCount > 0)
        {
            _player.velocity = Vector2.up * jumpForce;
            _extraJumpCount--;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _extraJumpCount >= 0)
        {
            _isJumping = true;
            _jumpTimeCounter = jumpTime;
            _extraJumpCount--;
        }

        if (Input.GetKey(KeyCode.Space) && _isJumping)
        {
            if (_jumpTimeCounter > 0)
            {
                _player.velocity = Vector2.up * jumpForce;
                _jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                _isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        Move();

        FlipBasedOnMoveX();

        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, ground);

        var hitLadder =
            Physics2D.Raycast(transform.position, Vector2.up, distance, ladder);

        if (hitLadder.collider)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                _isClimbing = true;
            }
        }
        else
        {
            _isClimbing = false;
        }


        if (_isClimbing)
        {
            Climb();
        }
        else
        {
            _player.gravityScale = gravityScale;
        }
    }

    private void Climb()
    {
        _player.velocity = new Vector2(0, _moveY * speed);
        _player.gravityScale = 0;
    }

    private void Move()
    {
        _moveX = Input.GetAxisRaw("Horizontal");
        _moveY = Input.GetAxisRaw("Vertical");

        _player.velocity = new Vector2(_moveX * speed, _player.velocity.y);
    }

    private void FlipBasedOnMoveX()
    {
        _movingRight = _moveX > 0;
        _movingLeft = _moveX < 0;

        if ((!_facingRight && _movingRight) || (_facingRight && _movingLeft))
        {
            Flip();
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        var scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}