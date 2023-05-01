using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;
using NPBehave;





public class ProtossBehavior : MonoBehaviour
{
    private List<GameObject> planetList;
    private List<GameObject> starList;
    private List<GameObject> zergList;
    private List<GameObject> tarrenList;
    private List<GameObject> protossList;
    private Wander1 wander;
    private Flee flee;
    private Root tree;
    private Pursue pursue;
    private SteeringBasics steeringBasics;
    public GameObject fleeTarget;
    public GameObject attackTarget ;
    public float distanceThreshold;
    int behave = 0;




    private void Awake()
    {
        
    }
    private void Start()
    {
        planetList = new List<GameObject>();
        steeringBasics= GetComponent<SteeringBasics>();
        flee = GetComponent<Flee>();
        wander = GetComponent<Wander1>();
        fleeTarget = GameObject.FindGameObjectWithTag("Zerg");
        attackTarget = GameObject.FindGameObjectWithTag("Terran");
        pursue = GetComponent<Pursue>();
        tree = InitialiseBehaviourTree(0);
        tree.Start();
        
    }
    private void Update()
    {
       
        
       

    }

    // initialize behavior tree based on behavior variable
    private Root InitialiseBehaviourTree( int behave)
    {
        switch(behave)
        {

            case 0: return ProtossBehave(attackTarget, fleeTarget);
            case 1: return AttackBehave(attackTarget);
            case 2: return FleeBehave();
            default: return WonderBehave();
        }

    }

    private Root ProtossBehave(GameObject attackTarget, GameObject fleeTarget)
    {
        return new Root(new Selector(
            // Attack behavior
            new Sequence(
                new Condition(() => CheckDistance(attackTarget) < 20f, new Action(() => SeekTarget(attackTarget))),
                new Condition(() => CheckDistance(attackTarget) < 5f, new Action(() => Attack(attackTarget))),
                new Wait(2f)
            ),
            // Flee behavior
            new Sequence(
                new Action(() => fleeFrom(fleeTarget)),
                new Wait(2f)
            ),
            // Wander behavior
            new Sequence(
                new Action(() => Explore()),
                new Wait(2f)
            )
        ));
    }
   
  
    // behavior tree for wandering
    private Root WonderBehave()
    {
        return new Root(new Sequence(
        new Action(() => Explore()),
        new Wait(2f)
        ));
    }
    
  // behavior tree for attacking
  private Root AttackBehave(GameObject attackTarget)
  {
      return new Root(new Sequence(
      new Action(() => PersueTarget(attackTarget)),
      new Condition(() => CheckDistance(attackTarget) < 50f,
      new Action(() => Attack(attackTarget))),
      new Wait(2f)
      ));
  }
  // behavior tree for fleeing
  private Root FleeBehave()
  {
      return new Root(new Sequence(
      new Action(() => fleeFrom(fleeTarget)),
      new Wait(2f)
      ));
  }
  private Root PersueBehave()
  {
      return new Root(new Sequence(
      new Action(() => PersueTarget(attackTarget)),
          new Wait(2f)
          ));
  }
    // function for exploring/wandering
    private void Explore () 
    {
        Vector3 accel = wander.GetSteering();
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();

    }
    //function for attacking
    private void Attack( GameObject target)
    {
        Debug.Log("attacking" + target);
    }
    // behavior for fleeing from target
    private void fleeFrom(GameObject target)
    {
        Vector3 accel = flee.GetSteering(target.transform.position);
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }
    // behavior for pursuing
    private void PersueTarget(GameObject target)
    {
        Vector3 accel = pursue.GetSteering(target.GetComponent<MovementAIRigidbody>());
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }

    // calculate distance to target
    float CheckDistance(GameObject target)
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        return distance;
    }
    private void SeekTarget(GameObject target)
    {
        Vector3 accel = steeringBasics.Seek(target.transform.position);// Calculate the desired steering acceleration using the Seek behavior
        steeringBasics.Steer(accel); // Apply the steering force to move the agent
        steeringBasics.LookWhereYoureGoing();  // Rotate the agent to face the direction of motion
    }







    //Refrance : Scripts from UnityMovementAI have been used in this colde including some of the functions



}
