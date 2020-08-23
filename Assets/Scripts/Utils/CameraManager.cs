﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static Camera[] cams;

    private void Awake()
    {
        cams = FindObjectsOfType<Camera>();
    }
}