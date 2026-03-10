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

    public LayerMask selectionMaskUnit;
    public LayerMask selectionMaskTerrain;

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
            if (result.tag == "Player")
            {
                //Debug.Log("player event");
                OnUnitSelected?.Invoke(result);
            }
            else
            {
                //Debug.Log("terrain event");
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
        //Debug.Log(mainCamera.ScreenToWorldPoint(mousePosition));
    }

    private bool FindTarget(Vector3 mousePosition, out GameObject result)
    {
        //RaycastHit hit;
        RaycastHit2D hit2D;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        Vector2 direction = new Vector2(0f, 0f);

        if (hit2D = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(mousePosition), direction, Mathf.Infinity, selectionMaskUnit)) //dstance infinity might be a bit much
        {
            result = hit2D.collider.gameObject;
            //Debug.Log("Game object " + hit2D.collider.gameObject);
            //Debug.Log("overlap " + hit2D.collider.OverlapPoint(mainCamera.ScreenToWorldPoint(mousePosition)));
            return true;
        }

        else if (hit2D = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(mousePosition), direction, Mathf.Infinity, selectionMaskTerrain)) //dstance infinity might be a bit much
        {
            result = hit2D.collider.gameObject;
            //Debug.Log("Game object " + hit2D.collider.gameObject);
            //Debug.Log("overlap " + hit2D.collider.OverlapPoint(mainCamera.ScreenToWorldPoint(mousePosition)));
            return true;
        }

        result = null;
        return false;
    }
}
