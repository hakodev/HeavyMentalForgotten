using System.Collections.Generic;
using UnityEngine;

public class OrbCalculator : MonoBehaviour {
    public static OrbCalculator Ins { get; private set; }
    public List<GameObject> FilledPlates { get; private set; }

    private void Awake() {
        Ins = this;
    }

    private void Calculate() {
        foreach(GameObject filledPlate in FilledPlates) {
            //if(filledPlate's memory layer > currentLayer + 1) {
            //    filledPlate's memory layer = currentLayer + 1;
            //}
        }
    }
}
