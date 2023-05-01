using NPBehave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;

public class TarrenBehave : MonoBehaviour

{ 

    private List<GameObject> zergList;
    private List<GameObject> tarrenList;
    private List<GameObject> protossList;
    private Wander1 wander;
    private Flee flee;
    private Root tree;
    private SteeringBasics steeringBasics;
    public GameObject fleeTarget;
    private Blackboard blackboard;
    int behave = 1;

    // Start is called before the first frame update
    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        flee = GetComponent<Flee>();
        wander = GetComponent<Wander1>();
        fleeTarget = GameObject.FindGameObjectWithTag("Zerg");
        tree = CreateBehaviourTree(behave);
        blackboard = tree.Blackboard;
        tree.Start();

    }
    private Root CreateBehaviourTree(float behave)
    {
        switch (behave)
        {
            case 1:
                return FleeFromZerg();
            default:
                return WonderBehave();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Root FleeFromZerg()
    {
        return new Root(new Sequence(
            new Action(() => fleeFrom(fleeTarget))
            //,new Wait(2f)
            ));
    }
    private void fleeFrom(GameObject target)
    {
        Vector3 accel = flee.GetSteering(target.transform.position);
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }
    private Root WonderBehave()
    {
        return new Root(new Sequence(
            new Action(() => Explore()),
            new Wait(2f)
            ));
    }
    private void Explore()
    {
        Vector3 accel = wander.GetSteering();
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();

    }
}
