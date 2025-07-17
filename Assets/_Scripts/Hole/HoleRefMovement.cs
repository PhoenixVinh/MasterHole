using UnityEngine;

public class HoleRefMovement : MonoBehaviour, IMovement
{


    [SerializeField] private GameObject arrow;

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
        ShowArrow();

    }


    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void ShowArrow()
    {
        if (_movementDirection != Vector2.zero)
        {
            arrow.SetActive(true);
            
            float angle = Mathf.Atan2(_movementDirection.y, _movementDirection.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0f, -(angle+90), 0f);  
        }
        else
        {
            arrow.SetActive(false);
        }
    }
}