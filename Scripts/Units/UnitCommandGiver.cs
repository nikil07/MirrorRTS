using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] LayerMask layerMask = new LayerMask();
    [SerializeField] UnitSelectionHandler unitSelectionHandler;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame)
            return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            return;

        if (hit.collider.TryGetComponent<Targetable>(out Targetable target)) {
            if (target.hasAuthority) {
                tryMove(hit.point);
                return;
            }
            tryTarget(target);
            return;
        }

        tryMove(hit.point);
    }

    private void tryTarget(Targetable target)
    {
        foreach (Unit unit in unitSelectionHandler.selectedUnits)
        {
            unit.getTargetter().CmdSetTarget(target.gameObject);
        }
    }

    private void tryMove(Vector3 point) {
        foreach (Unit unit in unitSelectionHandler.selectedUnits) {
            unit.getUnitMovement().cmdMove(point);
        }
    }
}
