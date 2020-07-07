using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCounterSystem : MonoBehaviour
{

    [SerializeField]
    private Text turnText;

    int turn = 0;

    void OnEnable()
    {
        //method Advance gets called as well
        TimeControlSystem.OnTimeAdvance += Advance;
    }

    // Update is called once per frame
    void OnDisable()
    {

        //removes timecontrolsytem if scene is changed
        TimeControlSystem.OnTimeAdvance -= Advance;
    }

    public void Advance()
    {
        turn++;
        turnText.text = string.Format("Turn: {0}", turn);
    }
}
