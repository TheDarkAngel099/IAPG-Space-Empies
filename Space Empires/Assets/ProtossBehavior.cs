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
        
    }
    private void Update()
    {
        //ListUpdate();
        /* if (attackTarget ==  null)
         {
             Debug.Log(" no one to chase");

         }
         else
         {
             ChangeBehavior(attackTarget, distanceThreshold); // Change behavior based on distance
         }*/

        ChangeBehavior(attackTarget, 20f);
        SwitchTree(InitialiseBehaviourTree(behave));


        
       

    }


    private Root InitialiseBehaviourTree( int behave)
    {
        switch(behave)
        {
            
            case 1: return AttackBehave(attackTarget);
            case 2: return FleeBehave();
            default: return WonderBehave();
        }

    }

    private Root WonderBehave()
    {
        return new Root(new Sequence(
            new Action(() => Explore()),
            new Wait(2f)
            ));
    }
    private Root AttackBehave(GameObject attackTarget)
    {
        return new Root(new Sequence(

            new Action(() => PersueTarget(attackTarget)),
            new Condition(() => CheckDistance(attackTarget) < 50f, 
            new Action(() => Attack(attackTarget))),
            new Wait(2f)
            ));
    }
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

    private void Explore () 
    {
        Vector3 accel = wander.GetSteering();
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();

    }
    private void Attack( GameObject target)
    {
        Debug.Log("attacking" + target);
    }
    private void fleeFrom(GameObject target)
    {
        Vector3 accel = flee.GetSteering(target.transform.position);
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }

    private void PersueTarget(GameObject target)
    {
        Vector3 accel = pursue.GetSteering(target.GetComponent<MovementAIRigidbody>());
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }


    float CheckDistance(GameObject target)
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        return distance;
    }


    private void ChangeBehavior(GameObject attacktarget, float distanceThreshold)
    {
        float distance = Vector3.Distance(this.transform.position, attacktarget.transform.position);

        if (distance < distanceThreshold)
        {
            
            tree = InitialiseBehaviourTree(1); // Change to AttackBehave()
            behave = 1;
            Debug.Log("attackBehave");
        }
        else
        {
            tree = InitialiseBehaviourTree(0); // Change to WonderBehave()
            behave = 0;
        }
    }

   

    void ListUpdate() 
    {
        foreach (GameObject zerg in GameObject.FindGameObjectsWithTag("Zerg"))
        {
            zergList.Add(zerg);
        }
        foreach (GameObject tarren in GameObject.FindGameObjectsWithTag("Tarren"))
        {
            tarrenList.Add(tarren);
        }
        foreach (GameObject protoss in GameObject.FindGameObjectsWithTag("Protoss"))
        {
            protossList.Add(protoss);
        }
        foreach (GameObject star in GameObject.FindGameObjectsWithTag("Star"))
        {
            starList.Add(star);
        }
        foreach (GameObject planet in GameObject.FindGameObjectsWithTag("Planet"))
        {
            planetList.Add(planet);
        }
    }
    private void SwitchTree(Root newTree)
    {
        if (tree != null)
        {
            tree.Stop();
        }
        tree = newTree;
        tree.Start();

    }




}
