using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] float destroyAfterSecond = 5f;
    [SerializeField] float launchForce = 10f;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Invoke("DestroySelf", destroyAfterSecond);
    }

    [Server]
    private void DestroySelf() {
        NetworkServer.Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
