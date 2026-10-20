using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePoints : MonoBehaviour {
    
    public int headPoints = 2;
    public int bodyPoints = 1;
    public PossibleTargets possibleTarget = PossibleTargets.Nothing;
    
    public void OnShotHead() {
        MainController.Instance.CheckIfNextTurn(possibleTarget, transform.localPosition, headPoints);
    }

    public void OnShotBody() {
        MainController.Instance.CheckIfNextTurn(possibleTarget, transform.localPosition, bodyPoints);
    }
}
