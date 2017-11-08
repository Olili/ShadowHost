using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBrain : Brain {

    Steering steering;
    public override void Awake()
    {
        base.Awake();
        steering = GetComponent<Steering>();
    }
    public override void Update()
    {
        base.Update();
       
    }
    public override void FixedUpdate()
    {
        Vector3 velocity = Vector3.zero;
        if (puppet.leader == null)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    steering.Seek(hit.point);
                }
            }
        }

        steering.Separation();
        velocity = steering.ComputedVelocity;

        puppet.SetVelocity(velocity);
        velocity.y = 0;
        puppet.Rotate(velocity);
    }
      
}
