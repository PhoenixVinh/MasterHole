using UnityEngine;

public class HoleRefMovement : MonoBehaviour, IMovement
{

    public static HoleRefMovement Instance { get; private set; }
    public Vector2 GetDirectionMovement() => _movementDirection;

    private Vector2 _movementDirection;

    private float _speed = 5f;



    private Rigidbody _rigidbody;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    public void Move(Vector2 movementDirection)
    {
        _movementDirection = movementDirection;
    }

    public void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(_movementDirection.x, 0, _movementDirection.y) * _speed;
        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, targetVelocity, Time.fixedDeltaTime * 10f);
    }


    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }
}