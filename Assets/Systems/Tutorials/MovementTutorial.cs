using UnityEngine;

public class MovementTutorial : Tutorial
{
    private void Update()
    {
        if (active && !completed)
        {
            float input = Input.GetAxis("Horizontal") + Input.GetAxis("Vertical");
            if (input > 0 || input < 0)
                Complete();
        }
    }
}
