using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    // Ajustes globales
    [SerializeField] public float motorForce = 1000f;
    [SerializeField] public float breakForce = 800f;
    [SerializeField] public float maxSteerAngle = 35f;
    [SerializeField] public float turboMultiplier = 1.5f;

    // Ruedas
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private bool turboActivo = false;

    private float uiVerticalInput = 0.0f;
    private float uiHorizontalInput = 0.0f;
    private bool uiIsBraking = false;
    private bool motorHabilitado = true;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = uiHorizontalInput;
        verticalInput = uiVerticalInput;
        isBreaking = uiIsBraking;
    }

    public void SetVerticalInput(float value) { uiVerticalInput = value; }
    public void SetHorizontalInput(float value) { uiHorizontalInput = value; }
    public void SetBrake(bool braking) { uiIsBraking = braking; }
    public void SetTurbo(bool turbo) { turboActivo = turbo; }

    public float GetVerticalInput() { return uiVerticalInput; }
    public bool IsBraking() { return uiIsBraking; }

    private void HandleMotor()
    {
        float fuerza = motorForce;
        if (turboActivo) fuerza *= turboMultiplier;

        if (!motorHabilitado)
        {
            frontLeftWheelCollider.motorTorque = 0f;
            frontRightWheelCollider.motorTorque = 0f;
        }
        else
        {
            frontLeftWheelCollider.motorTorque = verticalInput * fuerza;
            frontRightWheelCollider.motorTorque = verticalInput * fuerza;
        }

        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        float actualVelocity = rb.linearVelocity.magnitude;
        float maxVelocity = 30f;
        float steer = Mathf.Clamp01(1 - (actualVelocity / maxVelocity));

        float steerAngle = maxSteerAngle * steer;
        currentSteerAngle = steerAngle * horizontalInput;

        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void SetMotorActivo(bool activo)
    {
        motorHabilitado = activo;
    }
    
    public bool MotorActivo() => motorHabilitado;

}