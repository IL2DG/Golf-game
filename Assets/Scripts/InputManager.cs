using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float distanceLimit = 1.5f;

    private float dragDistance;
    private bool canRotate = false;

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !canRotate){
            GetDistance();
            canRotate = true;
            if(dragDistance <= distanceLimit){
                BallController.instance.MouseDownMethod();
            }
        }

        if(canRotate){
            if(Input.GetMouseButton(0)){
                if(dragDistance <= distanceLimit && BallController.instance != null){
                    BallController.instance.MouseNormalMethod();
                } else {
                    CameraFollow.instance.CameraRotation.RotateCamera(Input.GetAxis("Mouse X"));
                }
            }

            if(Input.GetMouseButtonUp(0)){
                canRotate = false;

                if(dragDistance <= distanceLimit){
                    BallController.instance.MouseUpMethod();
                }
            }
        }
    }

    void GetDistance(){
        var plane = new Plane(Camera.main.transform.forward, BallController.instance.transform.position);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist;
        if(plane.Raycast(ray, out dist)){
            var v3Pos = ray.GetPoint(dist);
            dragDistance = Vector3.Distance(v3Pos, BallController.instance.transform.position);
        }
    }
}
