using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f; // Velocidad de rotación
    private Text statusText;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
            Debug.LogError("No se encontró el CharacterController en " + gameObject.name);

        GameObject statusTextObject = GameObject.Find("StatusText");
        if (statusTextObject != null)
        {
            statusText = statusTextObject.GetComponent<Text>();
        }

        if (IsOwner)
        {
            Debug.Log("Cliente conectado exitosamente al servidor.");
            if (statusText != null)
            {
                statusText.text = "Conectado al servidor";
            }
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W)) moveX = -1f; // Adelante en el eje X
        if (Input.GetKey(KeyCode.S)) moveX = 1f; // Atrás en el eje X
        if (Input.GetKey(KeyCode.D)) moveZ = 1f; // Derecha en el eje Z
        if (Input.GetKey(KeyCode.A)) moveZ = -1f; // Izquierda en el eje Z

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (moveDirection != Vector3.zero)
        {
            // Rotar hacia la dirección del movimiento
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        MovePlayerServerRpc(moveDirection);
    }

    [ServerRpc]
    private void MovePlayerServerRpc(Vector3 moveDirection)
    {
        if (characterController == null) return;

        Vector3 move = moveDirection * moveSpeed * Time.deltaTime;
        characterController.Move(move); // Ahora el CharacterController manejará el movimiento

        UpdatePlayerPositionClientRpc(transform.position);
    }

    [ClientRpc]
    private void UpdatePlayerPositionClientRpc(Vector3 newPosition)
    {
        if (!IsOwner)
        {
            transform.position = newPosition;
        }
    }
}