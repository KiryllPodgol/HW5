using UnityEngine;

public class CamControll : MonoBehaviour
{
    public Transform target;
    public float rotSpeed = 1.5f;
    private float _rotY;
    private float _rotX;
    private Vector3 _offset;
    public RectTransform touchPanel;
    public float touchSensitivity = 0.1f;
    public float minVerticalAngle = -45f;
    public float maxVerticalAngle = 45f;

    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _rotX = transform.eulerAngles.x;
        _offset = target.position - transform.position;
    }

    void LateUpdate()
    {
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0)
        {
            _rotY += horInput * rotSpeed;
        }
        else
        {
            _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
        }

        if (vertInput != 0)
        {
            _rotX -= vertInput * rotSpeed;
        }
        else
        {
            _rotX -= Input.GetAxis("Mouse Y") * rotSpeed * 3;
        }

        // Ограничение угла наклона по вертикали
        _rotX = Mathf.Clamp(_rotX, minVerticalAngle, maxVerticalAngle);

        // Обработка касаний для вращения камеры
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (RectTransformUtility.RectangleContainsScreenPoint(touchPanel, touch.position))
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    float touchRotationX = touch.deltaPosition.y * touchSensitivity;
                    float touchRotationY = touch.deltaPosition.x * touchSensitivity;
                    _rotX -= touchRotationX;
                    _rotY += touchRotationY;

                    // Ограничение угла наклона по вертикали
                    _rotX = Mathf.Clamp(_rotX, minVerticalAngle, maxVerticalAngle);
                }
            }
        }

        Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
        transform.position = target.position - (rotation * _offset);
        transform.LookAt(target);
    }
}
