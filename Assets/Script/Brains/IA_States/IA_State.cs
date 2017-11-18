using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_State  {


    public ptrStateFct UpdateFct;
    public ptrStateFct FixedUpdateFct;
    protected Puppet puppet;
    protected Steering steering;

    public IA_State(Puppet _puppet)
    {
        puppet = _puppet;
        steering = _puppet.GetComponent<Steering>();
    }
    public virtual void OnBegin()
    {

    }
    public virtual void OnEnd()
    {

    }
}
