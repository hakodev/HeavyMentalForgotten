using System.Collections.Generic;
using UnityEngine;

public class OrbCalculator : MonoBehaviour {
    public static OrbCalculator Ins { get; private set; }
    
    private MemoryPlateHandler memoryPlateHandler;

    private void Awake() {
        Ins = this;
        memoryPlateHandler = GetComponent<MemoryPlateHandler>();
    }

    public void Calculate() {
        if(memoryPlateHandler != null) {
            //for(int i = 0; i < FilledPlates.Count; i++) {
            //    //if(filledPlate's memory layer > currentLayer + 1) {
            //    //    filledPlate's memory layer = currentLayer + 1;
            //    //}
            //    if(FilledPlates[i]) {

            //    }
            //}
        }
    }
}
