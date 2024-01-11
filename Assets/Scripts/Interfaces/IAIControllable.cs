using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIControllable
{
    //Meant to run a couple times a second for optimisation, may be desynct or multi-threaded.
    //This method can be put inside the general update method with an int parameter that tells it how many times a second it should run
    public void AIUpdate();
    
    //To set the AI controller of this object.
    public void SetAIController(); 

}
