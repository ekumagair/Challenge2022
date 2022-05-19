using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Camera Information
    public Transform cameraTransform;
    public Transform cameraParent;
    private Vector3 orignalCameraPos;

    // Shake Parameters
    public float shakeDuration = 1f;
    public float shakeAmount = 0.5f;

    private bool canShake = false;
    private float _shakeTimer;

    void Start()
    {
        orignalCameraPos = cameraTransform.localPosition;
        cameraParent = cameraTransform.parent;
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShakeCamera();
        }
        */

        if (canShake)
        {
            StartCameraShakeEffect();
        }
    }

    public void ShakeCamera()
    {
        canShake = true;
        _shakeTimer = shakeDuration;
    }

    public void StartCameraShakeEffect()
    {
        if (_shakeTimer > 0)
        {
            cameraTransform.localPosition = orignalCameraPos + Random.insideUnitSphere * shakeAmount;
            _shakeTimer -= Time.deltaTime;
        }
        else
        {
            _shakeTimer = 0f;
            cameraTransform.position = cameraParent.position;
            canShake = false;
        }
    }

}