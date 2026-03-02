using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    Camera mainCamera;
    //Rigidbody rb;
    CharacterController controller;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 lookDir = hit.point - transform.position;
            lookDir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookDir);
            transform.rotation = rotation;
        }
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        //rb.velocity = moveDirection * moveSpeed;
    }
}