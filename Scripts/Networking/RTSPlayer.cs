using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] private List<Unit> myUnits = new List<Unit>();

    public List<Unit> getMyUnits() {
        return myUnits;
    }

    #region Server

    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += serverHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += serverHandleUnitDespawned;
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitDespawned -= serverHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= serverHandleUnitDespawned;
    }

    private void serverHandleUnitSpawned(Unit unit) {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
            return;

        myUnits.Add(unit);

    }

    private void serverHandleUnitDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
            return;

        myUnits.Remove(unit);
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        if (!isClientOnly)
            return;

        Unit.AuthorityOnUnitSpwaned += authorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespwaned += authorityHandleUnitDespawned;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    private void authorityHandleUnitSpawned(Unit unit) {
        if (!hasAuthority)
            return;

        myUnits.Add(unit);
    }
    private void authorityHandleUnitDespawned(Unit unit)
    {
        if (!hasAuthority)
            return;
        myUnits.Remove(unit);
    }

    #endregion

}
