using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour {

    public int maxNumber;
    private Queue<Ragdoll> ragdolls;

    private void Start() {
        ragdolls = new Queue<Ragdoll>();
    }

    public void AddRagdoll(Ragdoll rd) {
        //ragdolls.Add(rd);
        ragdolls.Enqueue(rd);
        UpdateRagdollCount();
    }

    private void UpdateRagdollCount() {
        if (ragdolls.Count > maxNumber) {
            //Destroy(ragdolls[0].gameObject);
            Ragdoll rd = ragdolls.Dequeue();
            Destroy(rd.gameObject);
        }
    }
}
