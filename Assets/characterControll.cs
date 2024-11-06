using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    public float gravity = -9.81f;
    public float speed = 10f;
    public float jumpPower = 5f;
    private Vector3 _velocity;
    public float touchSensitivity = 0.1f;
    public Button Buttonjump;
    private FixedJoystick _joystick;
    public RectTransform touchPanel;

    private CharacterController controller;

    public CharacterController Controller { get { return controller = controller ?? GetComponent<CharacterController>(); } }

    void Start()
    {
        _joystick = FindObjectOfType<FixedJoystick>();
        Buttonjump.onClick.AddListener(Jump);
    }

    void Update()
    {
        bool isGrounded = Controller.isGrounded;

        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -1f;
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _velocity.y = jumpPower;
        }

        float vertical = Input.GetAxis("Vertical") + _joystick.Vertical;
        float horizontal = Input.GetAxis("Horizontal") + _joystick.Horizontal;
        float rotation = Input.GetAxis("Mouse X");

        Vector3 movement = new Vector3(horizontal * speed, _velocity.y, vertical * speed);
        Controller.Move(transform.TransformDirection(movement) * Time.deltaTime);
        Controller.transform.Rotate(Vector3.up, rotation);

        if (!isGrounded)
        {
            _velocity.y += gravity * Time.deltaTime;
        }

        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (RectTransformUtility.RectangleContainsScreenPoint(touchPanel, touch.position))
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    float touchRotation = touch.deltaPosition.x * touchSensitivity;
                    Controller.transform.Rotate(Vector3.up, touchRotation);
                }
            }
        }
    }

    public void Jump()
    {
        if (Controller.isGrounded)
        {
            _velocity.y = jumpPower;
        }
    }
}
