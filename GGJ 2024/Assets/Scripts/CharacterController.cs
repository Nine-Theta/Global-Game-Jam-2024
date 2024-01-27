using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(Player))]
public class CharacterController : MonoBehaviour
{
    private Rigidbody _body;
    private Player _player;

    [SerializeField, ReadOnly]
    private Vector3 _movement = Vector3.zero;
    [SerializeField, ReadOnly]
    private float _rotationAngle = 0f;

    [SerializeField]
    private float _rotationSpeed = 1f;

    [SerializeField]
    private float _moveSpeed = 100f;

    [SerializeField]
    private float _jumpStrength = 10f;

    [SerializeField]
    private float _gravityMult = 20f;

    [SerializeField, ReadOnly]
    private bool _isGrounded = true;
    [SerializeField, ReadOnly]
    private bool _jumped = false;

    private void Start()
    {
        _body = this.GetComponent<Rigidbody>();
        _player = this.GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        _body.AddForce(_movement, ForceMode.Acceleration);

        if (_jumped)
        {
            _body.AddForce(new Vector3(0, _jumpStrength, 0), ForceMode.VelocityChange);
            _jumped = false;
        }
        else if (!_isGrounded)
        {
            _body.AddForce(new Vector3(0, -1 * _gravityMult, 0), ForceMode.Acceleration);
        }

       
    }

    public void OnMove(InputAction.CallbackContext pContext)
    {
        _movement.x = pContext.ReadValue<Vector2>().x * _moveSpeed;
        _movement.z = pContext.ReadValue<Vector2>().y * _moveSpeed;
    }

    public void OnLook(InputAction.CallbackContext pContext)
    {
        _rotationAngle = Mathf.Atan2(pContext.ReadValue<Vector2>().x, pContext.ReadValue<Vector2>().y);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(Mathf.Rad2Deg * _rotationAngle, Vector3.up), Time.deltaTime * _rotationSpeed);

        //_targetRotation.x = pContext.ReadValue<Vector2>().x;
        //_targetRotation.z = pContext.ReadValue<Vector2>().y;
    }

    public void OnJump(InputAction.CallbackContext pContext)
    {
        if (_isGrounded)
        {
            _jumped = true;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            _isGrounded = true;
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}
