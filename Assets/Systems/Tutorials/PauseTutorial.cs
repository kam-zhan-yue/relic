using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTutorial : Tutorial
{
    private void Update()
    {
        if (active && !completed)
        {
            if (Input.GetMouseButtonDown(1))
                Complete();
        }
    }
}
