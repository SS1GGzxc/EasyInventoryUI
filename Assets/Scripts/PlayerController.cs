using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rig;
    private bool isGrounded = false;
    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;
    private float _rotationY;
    private float _rotationX;
    private float _distanceFromTarget = 5f;

    [Header("Children compomponents")]
    public Camera playerView;

    [Header("Movment Props")]
    public float MovingSpeed = 5f;
    public float JumpHeight = 3f;



    [Header("Camera Props")]
    public float _mouseSensitivity = 1.5f;
    public float maxCameraDistanse = 10f;
    public float minCameraDistanse = 3f;
    public Vector2 _rotationXMinMax = new Vector2(-20, 20);
    public float ScrollStep = 0.5f;
    public float _smoothTime = 0.2f;


    private void Awake()
    {
        rig = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        gameObject.transform.position += new Vector3(horizontal * MovingSpeed * Time.unscaledDeltaTime, 0, vertical * MovingSpeed * Time.unscaledDeltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        playerView.transform.LookAt(gameObject.transform, Vector3.up);

        MoveCamera();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "ground")
        {
            isGrounded = true;
        }
    }

    private void Jump()
    {
        float JumpVelocity = (float)Math.Sqrt((JumpHeight - 1f) * Physics.gravity.y * -2) * rig.mass;
        rig.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
        isGrounded = false;
    }

    private void MoveCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _rotationY += mouseX;
        _rotationX += mouseY;

        // Apply clamping for x rotation
        _rotationX = Math.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        // Apply damping between rotation changes
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        playerView.transform.localEulerAngles = _currentRotation;

        if (Input.mouseScrollDelta.y == -1f && _distanceFromTarget > minCameraDistanse)
        {
            _distanceFromTarget -= ScrollStep;
        }
        if (Input.mouseScrollDelta.y == 1f && _distanceFromTarget < maxCameraDistanse)
        {
            _distanceFromTarget += ScrollStep;
        }

        // Substract forward vector of the GameObject to point its forward vector to the target
        playerView.transform.position = gameObject.transform.position - playerView.transform.forward * _distanceFromTarget;

    }

}
