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
    private Wander1 wander; // Wander1 script for random movement behavior
    private Flee flee; // Flee script for fleeing behavior
    private Root tree; // Behavior tree
    private SteeringBasics steeringBasics; // Steering basics script for movement
    public GameObject fleeTarget; // GameObject for the ship tarren is going to flee from
    private Blackboard blackboard; // Behavior tree blackboard
    int behave = 1; // variable for behavior selection

    // Start is called before the first frame update
    void Start()
    {
        // Get necessary components and set the fleeTarget to the closest Zerg
        steeringBasics = GetComponent<SteeringBasics>();
        flee = GetComponent<Flee>();
        wander = GetComponent<Wander1>();
        fleeTarget = GameObject.FindGameObjectWithTag("Zerg");
        // Create behavior tree and start it
        tree = CreateBehaviourTree(behave);
        blackboard = tree.Blackboard;
        tree.Start();

    }

    // Create the behavior tree based on the selected behavior
    private Root CreateBehaviourTree(float behave)
    {
        switch (behave)
        {
            case 1:
                return FleeFromZerg(); // Return behavior for fleeing from Zerg
            default:
                return WonderBehave(); // Return behavior for random wandering
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // behavior for fleeing from the Zerg
    private Root FleeFromZerg()
    {
        return new Root(new Sequence(
            new Action(() => fleeFrom(fleeTarget))
            //,new Wait(2f)
            ));
    }
    // Function for fleeing from the target object
    private void fleeFrom(GameObject target)
    {
        Vector3 accel = flee.GetSteering(target.transform.position); // Calculate steering acceleration using the Flee script
        steeringBasics.Steer(accel);  // Apply steering to the object
        steeringBasics.LookWhereYoureGoing();  // Make the object look where it's going
    }

    // Create behavior for random wandering
    private Root WonderBehave()
    {
        return new Root(new Sequence(
            new Action(() => Explore()), // Explore by wandering randomly
            new Wait(2f) // Wait for 2 seconds
            ));
    }

    // Function for random wandering
    private void Explore()
    {
        Vector3 accel = wander.GetSteering(); // Calculate steering acceleration using the Wander1 script
        steeringBasics.Steer(accel); // Apply steering to the object
        steeringBasics.LookWhereYoureGoing();  // Make the object look where it's going

    }
}
