using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    // TODO: Multiple cats?
    public GameObject catRef;
    enum Tools { Dustpan, SprayBottle, Mop, Hand };
    Tools currentTool = Tools.Hand;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void handleDustpanClicked()
    {
        currentTool = Tools.Dustpan;
    }

    void handleSprayBottleClicked()
    {
        currentTool = Tools.SprayBottle;
    }

    void handleMopClicked()
    {
        currentTool = Tools.Mop;
    }

    void handleHandClicked()
    {
        currentTool = Tools.Hand;
    }

}
