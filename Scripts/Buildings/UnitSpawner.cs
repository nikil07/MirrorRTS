using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject unitPrefab;
    [SerializeField] Transform unitSpwanPoint;

    

    #region Server

    [Command]
    private void CmdSpawnUnit() {

        GameObject unitInstance = Instantiate(unitPrefab, unitSpwanPoint.position, unitSpwanPoint.rotation);

        NetworkServer.Spawn(unitInstance, connectionToClient);
    }


    #endregion


    #region Client

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!hasAuthority)
            return;

        CmdSpawnUnit();
    }

    #endregion


}
