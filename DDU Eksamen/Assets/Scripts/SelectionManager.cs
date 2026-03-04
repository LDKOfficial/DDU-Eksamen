using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private TurnControler turnController;

    public LayerMask selectionMask;

    private Vector3 mousePosition;

    public UnityEvent<GameObject> OnUnitSelected;
    public UnityEvent<GameObject> TerrainSelected;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    // called when player clicks with their mouse
    public void HandleClick()
    {
        // player cant move if it isnt their turn
        if (!turnController.isPlayerTurn)
            return;

        GameObject result;

        if (FindTarget(mousePosition, out result))
        {
            if (UnitSelected(result))
            {
                OnUnitSelected?.Invoke(result);
            }
            else
            {
                TerrainSelected?.Invoke(result);
            }
        }
    }

    private bool UnitSelected(GameObject result)
    {
        return result.GetComponent<Unit>() != null;
    }

    public void MousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        //Debug.Log(mousePos);
    }

    private bool FindTarget(Vector3 mousePosition, out GameObject result)
    {
        //RaycastHit hit;
        RaycastHit2D hit2D;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        //hit2D = GetRayIntersection(ray, 100f, selectionMask);

        hit2D = Physics2D.Raycast(mainCamera.transform.position, ray.direction, selectionMask);


        result = hit2D.collider.gameObject;
        return true;

        //result = null;
        //return false;
    }
}
