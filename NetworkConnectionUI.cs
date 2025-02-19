using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;

public class NetworkConnectionUI : MonoBehaviour
{
    public InputField ipInputField;
    public InputField portInputField;
    public Button connectButton;
    public Button hostButton;

    private void Start()
    {
        connectButton.onClick.AddListener(ConnectAsClient);
        hostButton.onClick.AddListener(StartHost);
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    private void ConnectAsClient()
    {
        string ip = ipInputField.text;
        ushort port;
        if (!ushort.TryParse(portInputField.text, out port))
        {
            Debug.LogError("Puerto inválido");
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ip;
        transport.ConnectionData.Port = port;

        NetworkManager.Singleton.StartClient();
    }
}
