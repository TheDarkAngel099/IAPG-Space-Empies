using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using UnityMovementAI;


public class ZergBehave : MonoBehaviour
{

    // Fields for behaviors, steering, blackboard, and targets
    private Wander1 wander;
    private Root tree;
    private Blackboard blackboard;
    private Pursue pursue;
 
    private SteeringBasics steeringBasics;
    
    public GameObject attackTarget;
    public GameObject protoss;
 
    int behave = 0; // State variable for behavior

    private void Awake()
    {
       
    }
    private void Start()
    {
        // Get components
        pursue = GetComponent<Pursue>();
        steeringBasics = GetComponent<SteeringBasics>();
        wander = GetComponent<Wander1>();
        // Set initial attack target and protoss target
        attackTarget = GameObject.FindGameObjectWithTag("Terran");
        protoss = GameObject.FindGameObjectWithTag("Protoss");
        // Create behavior tree and start it
        tree = CreateBehaviourTree(behave);
        blackboard = tree.Blackboard;
        tree.Start();
    }
    private void Update()
    {
      
    }


   
    private Root CreateBehaviourTree(float behave)
    {
        switch(behave)
        {
            case 0:
                return ZergBerg(attackTarget);
            case 1:
                return AttackProtoss();
            default:
                return AttackTarren(attackTarget);
        }

    }



    private Root ZergBerg(GameObject attackTarget)
    {
        return new Root(new Selector(
            // If close enough to Tarren, attack Tarren
            new Sequence(
                new Condition(() => CheckDistance(attackTarget) < 700f, new Action(() => SeekTarget(attackTarget))),
                new Condition(() => CheckDistance(attackTarget) < 5f, new Action(() => Attack(attackTarget))),
                new Wait(2f)
            ),
            // If close enough to Protoss, attack Protoss
            new Sequence(
                new Condition(() => CheckDistance(protoss) < 10f,
                new Action(() => SeekTarget(protoss))),
                new Condition(() => CheckDistance(protoss) < 5f, new Action(() => Attack(protoss))),
                new Wait(10f)
            )
        

        ));
    }

  


    // AttackTerran method returns a behavior tree to attack the Terran target
    private Root AttackTarren(GameObject attackTarget)
    {
        return new Root(new Sequence(
            new Action(() => SeekTarget(attackTarget)),
            // If close enough to attack, attack the target
            new Condition(() => CheckDistance(attackTarget) < 5f,
            new Action(() => Attack(attackTarget))),
            // Wait before repeating behavior
            new Wait(5f)
            ));
    }

    // AttackProtoss method returns a behavior tree to attack the Protoss target
    private Root AttackProtoss()
    {
        return new Root(new Sequence(
            new Condition(() => CheckDistance(protoss) < 20f,
            new Action(() => SeekTarget(protoss))),
            // If close enough to attack, attack protoss
            new Condition(() => CheckDistance(protoss) < 5f,
            new Action(() => Attack(protoss))),
            new Wait(10f)
            ));
    }
    // PersueTarget method applies pursue steering behavior to target
    private void PersueTarget(GameObject target)
    {
        Vector3 accel = pursue.GetSteering(target.GetComponent<MovementAIRigidbody>());
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }
    // SeekTarget method applies seek steering
    private void SeekTarget(GameObject target)
    {
        Vector3 accel = steeringBasics.Seek(target.transform.position);// Calculate the desired steering acceleration using the Seek behavior
        steeringBasics.Steer(accel); // Apply the steering force to move the agent
        steeringBasics.LookWhereYoureGoing();  // Rotate the agent to face the direction of motion
    }

    // Attack method print a message to indicate attacking the target GameObject 
    private void Attack(GameObject target)
    {
       print("attacking" + target.name);
    }

    // method to calculate the distance between the agent and the target GameObject
    float CheckDistance(GameObject target)
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position); // Calculate the distance between the agent and the target
        return distance;// Return the calculated distance
    }



    //Refrance : Scripts from UnityMovementAI have been used in this colde including some of the functions

}
