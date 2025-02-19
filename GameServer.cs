using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class GameServer : NetworkBehaviour
{
    public GameObject playerPrefab; // Prefab del jugador
    private Dictionary<ulong, GameObject> connectedPlayers = new Dictionary<ulong, GameObject>();
    private Text statusText; // Referencia al UI Text en el Canvas

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;

            // Buscar el Text en la escena
            GameObject statusTextObject = GameObject.Find("ServerStatusText");
            if (statusTextObject != null)
            {
                statusText = statusTextObject.GetComponent<Text>();
                statusText.text = "Servidor iniciado y esperando conexiones...";
            }

            Debug.Log("Servidor iniciado y esperando conexiones...");
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = false;
        response.Position = Vector3.zero;
        response.Rotation = Quaternion.identity;

        SpawnPlayer(request.ClientNetworkId);
    }

    private void SpawnPlayer(ulong clientId)
    {
        GameObject newPlayer = Instantiate(playerPrefab, GetSpawnPosition(), Quaternion.identity);
        newPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        connectedPlayers[clientId] = newPlayer;

        if (statusText != null)
        {
            statusText.text = $"Jugador {clientId} conectado";
        }
    }

    private Vector3 GetSpawnPosition()
    {
        return new Vector3(UnityEngine.Random.Range(0f, 4f), 1f, UnityEngine.Random.Range(3.5f, 11f));
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if (connectedPlayers.ContainsKey(clientId))
        {
            Destroy(connectedPlayers[clientId]);
            connectedPlayers.Remove(clientId);
        }

        if (statusText != null)
        {
            statusText.text = $"Jugador {clientId} desconectado";
        }
    }
}
