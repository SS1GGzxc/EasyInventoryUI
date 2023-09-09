using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rig;
    private bool isGrounded = false;
    private Vector3 viewport;

    [Header("Children compomponents")]
    public Camera playerView;

    [Header("Movment Props")]
    public float MovingSpeed = 5f;
    public float JumpHeight = 3f;


    [Header("Camera Props")]
    public float CameraSpeed = 5f;
    public Vector3 MaxCameraPos = new Vector3(5f, 5f, 5f);
    public float ScrollStep = 0.5f;

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

        viewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Debug.Log(string.Format("x viewport: {0}; y viewport: {1}", viewport.x, viewport.y));

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
    private float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    private void MoveCamera()
    {
        float vertical;
        float horizontal;

        float value;

        //if (viewport.x < 0.5) value = Map(viewport.x, -1f, 1f, 0, 1f);
        //else horizontal = Map(viewport.x * 2, 0, 1f, -1f, 1f);

        //if (viewport.y < 0.5) vertical = -viewport.y;
        //else vertical = viewport.y;

        //value = Map(viewport.x, -1f, 1f, -1f, 1f);


        horizontal = Map(viewport.x, 0, 1f, 0, 1f);


        if (playerView.transform.position.x < MaxCameraPos.x)
        {
            playerView.transform.position += new Vector3(horizontal * CameraSpeed * Time.unscaledDeltaTime, 0, 0);
        } else if (playerView.transform.position.x > -MaxCameraPos.x) {
            playerView.transform.position += new Vector3(-horizontal * CameraSpeed * Time.unscaledDeltaTime, 0, 0);
        }

        //Debug.Log(vertical);
        Debug.Log(horizontal);
    }

}
