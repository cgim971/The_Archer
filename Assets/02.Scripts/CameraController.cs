using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform _targetTs = null;

    [SerializeField] private float _distance = 5.0f;
    [SerializeField] private float _height = 6f;
    [SerializeField] private float _heightDamping = 2.0f;
    [SerializeField] private float _rotationDamping = 3.0f;
    [SerializeField] private float _rotationAngleX = 30f;

    void ThirdCamera()
    {
        float objHeight = _targetTs.position.y + _height;
        float nowRotationAngle = transform.eulerAngles.x;
        float nowHeight = transform.position.y;

        nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, _rotationAngleX, _rotationDamping * Time.deltaTime);

        nowHeight = Mathf.Lerp(nowHeight, objHeight, _heightDamping * Time.deltaTime);

        Quaternion nowRotation = Quaternion.Euler(nowRotationAngle, 0f, 0f);

        transform.rotation = nowRotation;
        transform.position = _targetTs.position;
        transform.position -= Vector3.forward * _distance;
        transform.position = new Vector3(transform.position.x, nowHeight, transform.position.z);
    }

    private void LateUpdate()
    {
        ThirdCamera();
    }

}
