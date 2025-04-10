using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _player;

    public float speed;
    public float jumpForce;


    private float _moveX;
    private bool _movingRight;
    private bool _movingLeft;

    private bool _facingRight = true;

    private bool _isGrounded;
    public LayerMask ground;
    public Transform groundCheck;
    public float checkRadius;

    public int extraJump;
    private int _extraJumpCount;

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

        if (Input.GetKeyDown(KeyCode.UpArrow) && _extraJumpCount > 0)
        {
            _player.velocity = Vector2.up * jumpForce;
            _extraJumpCount--;
        } else if (Input.GetKeyDown(KeyCode.UpArrow) && _extraJumpCount == 0 && _isGrounded)
        {
            _player.velocity = Vector2.up * jumpForce;
        }
    }

    private void FixedUpdate()
    {
        Move();

        FlipBasedOnMoveX();

        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, ground);
    }

    private void Move()
    {
        _moveX = Input.GetAxisRaw("Horizontal");

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