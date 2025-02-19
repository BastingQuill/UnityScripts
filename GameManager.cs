using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public InputField ipInputField;
    public InputField portInputField;
    public Button hostButton;
    public Button clientButton;
    public Text statusText;

    private void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);

        // Leer argumentos de línea de comandos (para ejecutar múltiples instancias automáticamente)
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-host")
            {
                StartHost();
            }
            else if (args[i] == "-client")
            {
                StartClient();
            }
        }
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        statusText.text = "Servidor iniciado y esperando conexiones...";
        Debug.Log("Host iniciado");
    }

    private void StartClient()
    {
        string ip = ipInputField.text;
        ushort port = ushort.Parse(portInputField.text);

        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().ConnectionData.Address = ip;
        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().ConnectionData.Port = port;
        NetworkManager.Singleton.StartClient();
        statusText.text = $"Conectando como cliente a {ip}:{port}";
        Debug.Log($"Conectando como cliente a {ip}:{port}");
    }
}
