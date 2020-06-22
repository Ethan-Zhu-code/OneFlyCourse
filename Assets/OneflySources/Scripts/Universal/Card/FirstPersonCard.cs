using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Card;

public class FirstPersonCard : CardControl
{
    float OldAngel=0;
   public  CourseControl courseControl;
    
    [SerializeField]
    Transform cameraTransform;

    Vector3 originalPos;
    Vector3 originalRotation;

    private void OnDisable()
    {
        CameraControl.Instance.ResetPos();
        OldAngel = 0;

        cameraTransform.localEulerAngles = originalPos;
        cameraTransform.localPosition = originalRotation;


    }
    public override void HandlerEventByAngle(float angle)
    {
        base.HandlerEventByAngle(angle);
        //print(angle);
        CameraControl.Instance.Rotate(angle-OldAngel);
        OldAngel = angle;
    }


    private void OnEnable()
    {
        //foreach (SceneObj item in courseControl.objs)
        //{
        //    PointLock = false;
        //}

        if (gameObject.activeInHierarchy)
        {
            originalPos = cameraTransform.localEulerAngles;
            originalRotation = cameraTransform.localPosition;
        }
    }
   

}
