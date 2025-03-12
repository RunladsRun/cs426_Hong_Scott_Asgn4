using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 2f;
    public float rotationSpeed = 200f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 3f; // Added for mouse sensitivity

    Rigidbody rb;
    public List<Color> colors = new List<Color>();

    [SerializeField] private GameObject spawnedPrefab;
    private GameObject instantiatedPrefab;

    public GameObject cannon;
    public GameObject bullet;

    [SerializeField] private AudioListener audioListener;
    [SerializeField] private Camera playerCamera;

    private Animator animator;
    private float rotationX = 0f; // Added for vertical rotation control

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDirection.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDirection.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDirection.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDirection.x = +1f;

        Vector3 worldMoveDirection = transform.TransformDirection(moveDirection);
        transform.position += worldMoveDirection * speed * Time.deltaTime;

        if (moveDirection.magnitude > 0) animator.SetBool("isWalking", true);
        else animator.SetBool("isWalking", false);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("jump");
        }

        // Mouse Rotation (Third-Person View)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0, mouseX, 0); // Horizontal rotation

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -30f, 60f); // Limit vertical rotation range
        playerCamera.transform.localEulerAngles = new Vector3(rotationX, 0, 0);

        if (Input.GetKeyDown(KeyCode.I))
        {
            instantiatedPrefab = Instantiate(spawnedPrefab);
            instantiatedPrefab.GetComponent<NetworkObject>().Spawn(true);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            instantiatedPrefab.GetComponent<NetworkObject>().Despawn(true);
            Destroy(instantiatedPrefab);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            BulletSpawningServerRpc(cannon.transform.position, cannon.transform.rotation);
        }
    }

    public override void OnNetworkSpawn()
    {
        GetComponent<MeshRenderer>().material.color = colors[(int)OwnerClientId];
        if (!IsOwner) return;

        audioListener.enabled = true;
        playerCamera.enabled = true;
    }

    [ServerRpc]
    private void BulletSpawningServerRpc(Vector3 position, Quaternion rotation)
    {
        BulletSpawningClientRpc(position, rotation);
    }

    [ClientRpc]
    private void BulletSpawningClientRpc(Vector3 position, Quaternion rotation)
    {
        GameObject newBullet = Instantiate(bullet, position, rotation);
        newBullet.GetComponent<Rigidbody>().linearVelocity += Vector3.up * 2;
        newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * 1500);
    }
}