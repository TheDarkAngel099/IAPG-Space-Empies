using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using UnityMovementAI;
using static UnityEngine.GraphicsBuffer;
using System.Diagnostics;

public class ZergBehave : MonoBehaviour
{
 
    private Wander1 wander;
    private Root tree;
    private Blackboard blackboard;
    private Pursue pursue;
 
    private SteeringBasics steeringBasics;
    
    public GameObject attackTarget;
    public GameObject protoss;
 
    int behave = 1;

    private void Awake()
    {
       
    }
    private void Start()
    {
        pursue = GetComponent<Pursue>();
        steeringBasics = GetComponent<SteeringBasics>();
        wander = GetComponent<Wander1>();
        attackTarget = GameObject.FindGameObjectWithTag("Terran");
        protoss = GameObject.FindGameObjectWithTag("Protoss");
        tree = CreateBehaviourTree(behave);
        blackboard = tree.Blackboard;
        tree.Start();
    }
    private void Update()
    {
        
    }

    private void UpdateBehave()
    {
        float distance = Vector3.Distance(transform.position, protoss.transform.position);
        if (distance < 25)
        {
            behave = 2;
        }
        else 
            behave = 1;

}

   

    private Root CreateBehaviourTree(float behave)
    {
        switch(behave)
        {
            case 1:
                return AttackTarren(attackTarget);
            case 2:
                return AttackProtoss();
            default:
                return WonderBehave();
        }

    }

    private Root AttackTarren(GameObject attackTarget)
    {
        return new Root(new Sequence(

            new Action(() => SeekTarget(attackTarget)),
            new Condition(() => CheckDistance(attackTarget) < 5f,
            new Action(() => Attack(attackTarget))),
            new Wait(2f)
            ));
    }
    private Root AttackProtoss()
    {
        return new Root(new Sequence(
            new Condition(() => CheckDistance(protoss) < 20f,
            new Action(() => SeekTarget(protoss))),
            new Condition(() => CheckDistance(protoss) < 5f,
            new Action(() => Attack(protoss))),
            new Wait(2f)
            ));
    }

    private void PersueTarget(GameObject target)
    {
        Vector3 accel = pursue.GetSteering(target.GetComponent<MovementAIRigidbody>());
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }
    private void SeekTarget(GameObject target)
    {
        Vector3 accel = steeringBasics.Seek(target.transform.position);

        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }
    private void Attack(GameObject target)
    {
       print("attacking" + target.name);
    }
    float CheckDistance(GameObject target)
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        return distance;
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
