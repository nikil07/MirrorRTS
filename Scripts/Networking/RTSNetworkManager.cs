using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField] GameObject spawnerPrefab;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        GameObject unitSpwanerInstance =  Instantiate(spawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
        NetworkServer.Spawn(unitSpwanerInstance, conn);

    }
}
