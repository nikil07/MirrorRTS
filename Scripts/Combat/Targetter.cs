using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Targetter : NetworkBehaviour
{
    [SerializeField] Targetable target;

    public Targetable getTarget() {
        return target;
    }

    #region Server

    [Command]
    public void CmdSetTarget(GameObject targetGameObject) {

        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable target)) {
            return;
        }

        this.target = target;
        
    }

    [Server]
    public void clearTarget() {
        target = null;
    }

    #endregion
}
