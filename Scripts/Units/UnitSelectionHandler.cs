using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] LayerMask layerMask = new LayerMask();
    [SerializeField] RectTransform unitSelectionArea;

    private RTSPlayer player;   
    private Camera mainCamera;
    public List<Unit> selectedUnits { get; } = new List<Unit>();
    private Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
       
    }

    private void Update()
    {
        if(player == null)
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            clearSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed) {
            updateSelectionArea();
        }
    }

    private void updateSelectionArea() {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - startPosition.x;
        float areaHeight = mousePosition.y - startPosition.y;

        unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        unitSelectionArea.anchoredPosition = startPosition + new Vector2(areaWidth/2, areaHeight/2);
    }

    private void startSelectionArea() {

        if (!Keyboard.current.leftShiftKey.isPressed) {
            foreach (Unit selectedUnit in selectedUnits)
            {
                selectedUnit.deSelect();
            }
            selectedUnits.Clear();
        }
        
        unitSelectionArea.gameObject.SetActive(true);
        startPosition = Mouse.current.position.ReadValue();
        updateSelectionArea();
    }

    private void clearSelectionArea() {

        unitSelectionArea.gameObject.SetActive(false);

        if(unitSelectionArea.sizeDelta.magnitude == 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
                return;

            if (!hit.collider.TryGetComponent<Unit>(out Unit unit))
                return;

            if (!unit.hasAuthority)
                return;

            selectedUnits.Add(unit);

            foreach (Unit selectedUnit in selectedUnits)
            {
                selectedUnit.select();
            }
            return;
        }

        Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
        Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2);

        foreach (Unit unit in player.getMyUnits()) {
            if (selectedUnits.Contains(unit))
                continue;

            Vector3 screenPosition = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPosition.x > min.x && screenPosition.x < max.x && screenPosition.y > min.y && screenPosition.y < max.y) {
                selectedUnits.Add(unit);
                unit.select();
            }
        }


    }
}
