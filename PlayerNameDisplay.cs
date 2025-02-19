using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerNameDisplay : NetworkBehaviour
{
    public Text playerNameText;  // Referencia al componente Text del jugador (esto lo asignas desde Unity)

    private void Start()
    {
        if (IsOwner)
        {
            // Asignar el nombre del jugador basado en su clientId
            playerNameText.text = "Player " + (NetworkManager.Singleton.LocalClientId + 1); // Agregamos 1 para que empiece desde 1
        }
    }
}
