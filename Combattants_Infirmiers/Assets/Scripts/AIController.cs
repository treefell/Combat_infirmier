using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;



public class AIController : TimeKeeper
{



    public float movementSpeed = 6f;
    public float stunTime = 3f;
    
    public enum AICondition { None, SelfHpBetween, DistanceToPlayerBetween, ActionsCount } //possible conditions
    public enum AIAction { Chase, Attack, Wait } //possible actions

    [System.Serializable]
    public struct AIConditionContainer // conditions and values to pass to them
    {
        public AICondition condition;

        [ShowIf("@this.condition == AICondition.SelfHpBetween || this.condition == AICondition.DistanceToPlayerBetween")] //Show if the condition takes a vector2
        public Vector2 value;

        [ShowIf("@this.condition == AICondition.ActionsCount")] //Show if the condition takes an int
        public Vector2 countModulo;
        
    }


    [System.Serializable]
    public struct AIPatternBlock // part of the pattern
    {
        public AIConditionContainer[] conditions;

        public AIAction action;

        [ShowIf("@this.action == AIAction.Attack")] // Show if relevant
        public Attack attack;
        [ShowIf("@this.action == AIAction.Attack")] // If true, doesn't move until in range before attacking
        public bool ignoreDistance;

        [ShowIf("@this.action == AIAction.Wait || this.action == AIAction.Chase || (this.action == AIAction.Attack && !this.ignoreDistance)")]
        public float waitTime;
        [ShowIf("@this.action == AIAction.Wait")]
        public bool autoBlock;

        [ShowIf("@this.action == AIAction.Chase")]
        public float minDistance;
        [ShowIf("@this.action == AIAction.Chase")]
        public float maxDistance;

        public int numberOfTimes;


        public int priority;
    }

    public AIPatternBlock[] pattern;
    private List<AIPatternBlock> validPattern = new List<AIPatternBlock>();


    private Fighter stats;
    private AIState currentAIState;

    private int actionCount = 0;

    [HideInInspector]
    public GameObject currentTarget;
    
    // Start is called before the first frame update
    void Start()
    {

        stats = this.GetComponent<Fighter>();
        currentTarget = GameManagerPersistent.Instance.BoxerA;
        NewAction();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAIState == null)
        {
            return;
        }
        if (myTime == 0)
        {
            return;
        }

        CalcTarget(); 
        if (currentTarget == null)
        {
            return;
        }
        currentAIState.Tick();
    }

    public void NewAction()
    {
        //maybe other kinds of systems implemented later
        actionCount += 1;
        ParseByPriority();
    }

    void ParseByPriority()
    {
        //if nothing is in the pattern, stop here
        if (pattern.Length == 0)
        {
            return;
        }

        
         validPattern.Clear();
        
        // we first check which blocks have their conditions met
        foreach (AIPatternBlock block in pattern)
        {
            int conditionsMet = 0; // in the case of multiple conditions, it counts them

            if (block.conditions.Length == 0)
            {
                validPattern.Add(block);
                continue; //passe à l'iteration suivante
            }

            foreach (AIConditionContainer condition in block.conditions)
            {
                conditionsMet += CheckCondition(condition);
            }

            if (conditionsMet >= block.conditions.Length)
            {
                validPattern.Add(block);
            }
            
        }

        // then we use the priority value to choose our next action
        int totalPriority = 0;
        foreach (AIPatternBlock validBlock in validPattern)
        {
            if (validBlock.priority <= 0) // priority of 0 or negative means "force this action". If multiple actions have it, the first one in the list will be executed
            {
                ExecuteAction(validBlock);
                return;
            }

            totalPriority += validBlock.priority;
        }

        int prioritySeed = Random.Range(1, totalPriority + 1);
        int currentPriority = 0;

        foreach (AIPatternBlock validBlock in validPattern)
        {
            currentPriority += validBlock.priority;
            if (currentPriority >= prioritySeed)
            {
                ExecuteAction(validBlock);
                return;
            }
        }

    }


    int CheckCondition(AIConditionContainer container)
    {
        switch (container.condition)
        {
            case AICondition.None:
                return 1;
            case AICondition.SelfHpBetween:
                if (SelfHpBetween(container.value))
                {
                    return 1;
                }
                return 0;
            case AICondition.DistanceToPlayerBetween:
                if (DistanceToPlayerBetween(container.value))
                {
                    return 1;
                }
                return 0;
            case AICondition.ActionsCount:
                if (ActionCount(container.countModulo))
                {
                    return 1;
                }
                return 0;
                //add new cases here
        }
        return 0;
    }


    void ExecuteAction(AIPatternBlock act)
    {
        
        switch (act.action)
        {
            case AIAction.Attack:
                if (act.ignoreDistance)
                {
                    SetAIState(new AIAttack(this, act.attack));
                }
                else
                {
                    if (GetComponent<AttackManager>().AttackSimulation(act.attack) == false)
                    {
                        SetAIState(new AIChaseToAttack(this, GetComponent<AttackManager>(), act.attack, movementSpeed));
                        StartCoroutine(WaitSpecifiedTime(act.waitTime));
                    }
                    else
                    {
                        SetAIState(new AIAttack(this, act.attack));
                    }
                }
                break;
            case AIAction.Chase:
                SetAIState(new AIChase(this, movementSpeed, act.minDistance, act.maxDistance));
                if (act.waitTime > 0)
                {
                    StartCoroutine(WaitSpecifiedTime(act.waitTime));
                }
               break;
            case AIAction.Wait:
                SetAIState(new AIWait(this, act.waitTime, act.autoBlock));
                if (act.waitTime > 0)
                {
                    StartCoroutine(WaitSpecifiedTime(act.waitTime));
                }
                break;
        }
    }

    public void Stunned()
    {
        SetAIState(new AIStun(this, stunTime));
        if (stunTime > 0)
        {
            StartCoroutine(WaitSpecifiedTime(stunTime));
        }
    }

    public void GetHit(GameObject attacker)
    {

        StopCoroutine("WaitSpecifiedTime");


        Hitlag();
        attacker.GetComponent<AttackManager>().Hitlag();

        SetAIState(new AIInHitstun(this));
        //StartCoroutine(HitstunWait(GameManagerPersistent.Instance.playerHitstunTime));
    }

    //state machine
    public void SetAIState(AIState state)
    {
        if (currentAIState != null)
        {
            currentAIState.OnStateExit();
        }

        currentAIState = state;

        if (currentAIState != null)
        {
            currentAIState.OnStateEnter();
        }
    }

    // ============================================================
    //Conditions
    // ============================================================
    public bool SelfHpBetween(Vector2 percentage)
    {
        if (stats.GetCurrentHp() / stats.hp * 100 <= Mathf.Max(percentage.x, percentage.y))
        {
            if (stats.GetCurrentHp() / stats.hp * 100 >= Mathf.Min(percentage.x, percentage.y))
            {
                return true;
            }

        }
        return false;
    }

    

    public bool DistanceToPlayerBetween(Vector2 range)
    {
        if (Vector3.Distance(this.transform.position, currentTarget.transform.position) <= Mathf.Max(range.x, range.y))
        {
            if (Vector3.Distance(this.transform.position, currentTarget.transform.position) >= Mathf.Min(range.x, range.y))
            {
                return true;
            }

        }
        return false;
    }

    public bool ActionCount(Vector2 target)
    {
        if (actionCount % target.x == target.y)
        {
            return true;
        }
        return false;
    }


    //===========================================
    // l'etat wait est special, il est vide car il ne peut pas contenir de coroutine (herite de monobehaviour)

    IEnumerator WaitSpecifiedTime(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        
        NewAction();
    }

    public void StopWaitingAndAttack(Attack attack)
    {
        StopCoroutine("WaitSpecifiedTime");
        SetAIState(new AIAttack(this, attack));
    }



    public void Death()
    {
        
        GameManagerPersistent.Instance.stunnedEnemies.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    void CalcTarget()
    {
        if (GameManagerPersistent.Instance.BoxerA.GetComponent<PlayerBase>().GetCurrentMovementState() is Dead)
        {
            
            currentTarget = GameManagerPersistent.Instance.NurseA;
        }
        else
        {
            currentTarget = GameManagerPersistent.Instance.BoxerA;
        }
    }
}
