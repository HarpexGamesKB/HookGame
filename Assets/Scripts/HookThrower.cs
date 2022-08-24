using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookThrower : MonoBehaviour
{
    [SerializeField] private Hook Hook;
    [SerializeField] private LayerMask whatCanBeClickedOn;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject Line;
    private bool CanShoot;


    private void Update()
    {
        if (!CanShoot) return;

        if (Hook.fuel <= 0)
        {
            Invoke(nameof(StopGame), 3f);

        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out raycastHit, 100, whatCanBeClickedOn))
            {
                if (Hook.CanShoot)
                {
                    Hook.transform.LookAt(raycastHit.point);

                    if (Hook.CanShoot)
                    {
                        Hook.CanRotate = false;
                        Line.SetActive(true);
                    }
                }
            }
        }/*
        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out raycastHit,100,whatCanBeClickedOn))
            {
                
                    Hook.transform.LookAt(raycastHit.point);
                Line.SetActive(true);
               
                
            }
        }*/
        if (Input.GetMouseButtonUp(0))
        {
            Line.SetActive(false);
            Hook.CanRotate = true;
            if (Physics.Raycast(ray, out raycastHit, 100, whatCanBeClickedOn))
            {
                Hook.ThrowHook(raycastHit.point);
            }
        }
    }
    public void GameStarted()
    {
        CanShoot = true;
    }
    public void StopGame()
    {
        CanShoot = false;
        GameOverPanel.SetActive(true);
    }

}
