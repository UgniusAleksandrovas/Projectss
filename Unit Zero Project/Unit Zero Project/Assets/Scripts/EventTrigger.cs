using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour {

    public UnityEvent function;

    public void CallOtherFunctions() {
        function.Invoke();
    }
}
