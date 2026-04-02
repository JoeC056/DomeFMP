using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class DebugManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.stateOfGame = GameManager.States.UsingComputer;
    }
    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (GameManager.instance.dayNo) 
            {
                case 0:
                    DaysProgressionManager.instance.ProgressDay0();
                    break;
                case 1:
                    DaysProgressionManager.instance.ProgressDay1();
                    break;
                case 2:
                    DaysProgressionManager.instance.ProgressDay2();
                    break;
                case 3:
                    DaysProgressionManager.instance.ProgressDay3();
                    break;
                case 4:
                    DaysProgressionManager.instance.ProgressDay4();
                    break;
                case 5:
                    DaysProgressionManager.instance.ProgressDay5();
                    break;
            }

        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
