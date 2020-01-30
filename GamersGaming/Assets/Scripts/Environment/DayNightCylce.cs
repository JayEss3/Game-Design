using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Light))]
public class DayNightCylce : MonoBehaviour
{
    public DayState CurrentDayState;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float NightSpeedMultiplier;
    [SerializeField] private Vector3 Direction;
    private void Awake()
    {
        cycle();
    }
    void Update()
    {
        cycle();
    }
    private void OnValidate()
    {
        cycle();
    }

    private void cycle() {
        if (transform.eulerAngles.x > 15 && CurrentDayState == DayState.Dawn && transform.eulerAngles.x < 91)
            CurrentDayState = DayState.Day;
        else if (transform.eulerAngles.x < 345 && CurrentDayState == DayState.Dusk && transform.eulerAngles.x > 269)
            CurrentDayState = DayState.Night;
        else if (transform.eulerAngles.x > 345 && CurrentDayState == DayState.Night && transform.eulerAngles.x > 269)
            CurrentDayState = DayState.Dawn;
        else if (transform.eulerAngles.x < 15 && CurrentDayState == DayState.Day && transform.eulerAngles.x < 91)
            CurrentDayState = DayState.Dusk;

        if(CurrentDayState == DayState.Night)
            transform.Rotate(Direction * (RotationSpeed * NightSpeedMultiplier * Time.deltaTime));
        else
            transform.Rotate(Direction * (RotationSpeed * Time.deltaTime));
    }
}

public enum DayState {
    Dawn,
    Day,
    Dusk,
    Night
}
