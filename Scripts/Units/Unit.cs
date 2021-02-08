using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class Unit : NetworkBehaviour
{
    [SerializeField] UnityEvent onSelected;
    [SerializeField] UnityEvent onDeSelected;
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] Targetter targetter;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpwaned;
    public static event Action<Unit> AuthorityOnUnitDespwaned;

    public UnitMovement getUnitMovement() {
        return unitMovement;
    }

    public Targetter getTargetter() {
        return targetter;
    }

    #region Server

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        if (!isClientOnly ||  !hasAuthority )
            return;
        AuthorityOnUnitSpwaned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority)
            return;
        AuthorityOnUnitDespwaned?.Invoke(this);
    }

    [Client]
    public void select() {
        if (!hasAuthority)
            return;

        onSelected?.Invoke();
    }
    [Client]
    public void deSelect()
    {
        if (!hasAuthority)
            return;

        onDeSelected?.Invoke();
    }


    #endregion
}
