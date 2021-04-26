using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBase : TimeKeeper
{

    public int port;
    public GameObject otherChar;
    //general
    public Character character;
    private Rigidbody2D rb;
    private bool facingRight = true;

    //for running

    private bool isGrounded;
    
    private MovementState currentMovementState;

    private bool ignoreInputs = false;

    //for blocking
    public GameObject blockBox;

    //controls
    public bool twinSticks;

    // for skills
    //public GameObject skillsMenu;
    //public GameObject firstSlot;

    [HideInInspector]
    public bool skillsMenuOpened = false;

    [HideInInspector]
    public bool canRez = false;
    [HideInInspector]
    public float rezTimer = 0f;
    [HideInInspector]
    public float fullRezTimer = 2f;
    [HideInInspector]
    public RezZone otherRezZone;
    public RezZone myRezZone;

    //public Animator animator;
    [HideInInspector]
    public bool canGetHit = true;

    [HideInInspector]
    public bool canDrop = false;
    [HideInInspector]
    public bool dragging = false;

    [HideInInspector]
    public Transform bedInRange;
    [HideInInspector]
    public Transform thisBed;
    [HideInInspector]
    public Vector3 previousPos;
    private bool nowHealing = false;
    [HideInInspector]
    public int direction = 6;
    [HideInInspector]
    public bool sideways = true;
    [HideInInspector]
    public int[] niveaux = new int[] { 0, 0, 0, 0 };
    [HideInInspector]
    public bool wasCombined = false;


    public SpriteRenderer mySprite;
    public Animator myAnimator;

    [HideInInspector]
    public Attack currentAttack;

    [HideInInspector]
    public int tiredness = 0;

    [HideInInspector]
    public bool RTPressed = false;

    private AudioSource audio;

    public AudioClip punch;

    // Start is called before the first frame update
    void Start()
    {
        audio = this.GetComponent<AudioSource>();
        SetMovementState(new Idle(this));
        rb = GetComponent<Rigidbody2D>();
        if (this is Nurse)
        {
            if (gameObject.layer == LayerMask.NameToLayer("TeamA"))
            {
                GameManagerPersistent.Instance.NurseA = this.gameObject;
                port = GameManagerPersistent.Instance.ports[1];
            }
            else if (gameObject.layer == LayerMask.NameToLayer("TeamB"))
            {
                GameManagerPersistent.Instance.NurseB = this.gameObject;
                port = GameManagerPersistent.Instance.ports[3];
            }


        }
        else
        {
            if (gameObject.layer == LayerMask.NameToLayer("TeamA"))
            {
                GameManagerPersistent.Instance.BoxerA = this.gameObject;
                port = GameManagerPersistent.Instance.ports[0];
            }
            else if (gameObject.layer == LayerMask.NameToLayer("TeamB"))
            {
                GameManagerPersistent.Instance.BoxerB = this.gameObject;
                port = GameManagerPersistent.Instance.ports[2];
            }
        }

        if (character.defaultAttack != null)
        {
            currentAttack = Instantiate(character.defaultAttack);

        }
        //attackManager = GetComponent<AttackManager>();
    }

    // Update is called once per frame
    void Update()
    {


        if (myTime == 0)
        {
            return;
        }

        if (nowHealing)
        {
            Heal();

        }
        if (currentMovementState != null)
        {
            currentMovementState.Tick();
        }

        if (Input.GetAxis(GetButtonName("RTrigger")) < 0.5f)
        {
            RTPressed = false;
        }

    }



     // State machines
    public void SetMovementState(MovementState state) 
    {
        if (currentMovementState is Dead)
        {
            if (state is Healing)
            {
            }
            else
            { return; }
        }

        if(currentMovementState != state)
        {
            //Debug.Log("changing state");
            if (currentMovementState != null)
            {
                currentMovementState.OnStateExit();
            }

            currentMovementState = state;

            if (state != null)
            {
                state.OnStateEnter();
            }
        }
    }

    public void Rez() // fonction exceptionnelle pour pouvoir changer de state quand on est mort et etre rez
    {
        myRezZone.Deactivate();
        this.GetComponent<Fighter>().currentHp = character.baseHp;
        
        LevelUp(0);
        canRez = false;
        MovementState state = new Idle(this);
        //Debug.Log("changing state");
        if (currentMovementState != null)
        {
            currentMovementState.OnStateExit();
        }

        currentMovementState = state;
        state.OnStateEnter();
        
    }

    public void GetHit(GameObject attacker)
    {
        if (!canGetHit)
        {
            return;
        }
        if (currentMovementState is Combine)
        {
            wasCombined = true;
        }

        audio.PlayOneShot(punch);
        Hitlag();
        attacker.GetComponent<AttackManager>().Hitlag();

        SetMovementState(new InHitstun(this));
        //StartCoroutine(HitstunWait(GameManagerPersistent.Instance.playerHitstunTime));
    }

    public string GetButtonName(string initialButton)
    {
        if (port != 1)
        {
            return "P" + port.ToString() + initialButton;
        }
        else
        {
            return initialButton;
        }
    }

    public void Death()
    {
        
        if (this is Nurse)
        {
            SetMovementState(new Stunned(this));
        }
        else
        {
            myRezZone.gameObject.SetActive(true);
            SetMovementState(new Dead(this));
            GameManagerPersistent.Instance.ScoreIncrement(this.gameObject, GameManagerPersistent.Instance.deadBonus) ;
        }
        
    }


    public MovementState GetCurrentMovementState()
    {
        return currentMovementState;
    }

    public void StartHeal()
    {
        previousPos = transform.position;
        transform.position = thisBed.position;
        rezTimer = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        nowHealing = true;
    }
    public void Heal()
    {
        rezTimer += Time.deltaTime*myTime;
        if (rezTimer >= fullRezTimer)
        {
            nowHealing = false;
            otherChar.GetComponent<PlayerBase>().canRez = false;
            transform.position = previousPos;
            Rez();
        }
    }

    public void LevelUp(int type)
    {
        switch (type)
        {
            case 0:
                if (niveaux[0] < 4)
                {
                    GetComponent<Fighter>().Popup("Attack+!");
                    niveaux[0] += 1;
                    currentAttack.cancellableFromFrame -= 2;
                    currentAttack.totalFrames -= 2;
                    currentAttack.hits[0].damage += 1;
                }
                break;
            case 1:
                if (niveaux[1] < 4)
                {
                    GetComponent<Fighter>().Popup("Hyper+!");
                    niveaux[1] += 1;
                    if (niveaux[1] == 1)
                    {
                        currentAttack.movement[1].targetPosition = new Vector2(0, -1);
                    }
                    else
                    {
                        currentAttack.movement[0].targetPosition += new Vector2(0, 1);
                        currentAttack.movement[1].targetPosition += new Vector2(0, -1);
                    }


                }
                else
                {
                    GetComponent<Fighter>().Popup("No effect!");
                }
                break;
            case 2:
                if (niveaux[2] <= 4)
                {
                    GetComponent<Fighter>().Popup("Hadoken+!");
                    niveaux[2] += 1;
                    currentAttack.spawnObjects = true;
                    currentAttack.objectsToSpawn = GameManagerPersistent.Instance.spawnedObjectsProgression[niveaux[2]-1].spawnedObjects;
                }
                else
                {
                    GetComponent<Fighter>().Popup("No effect!");
                }
                break;
        }
        
        
    }

    public void Sabotaged()
    { 
        
    }

    public void SabotagedHyper()
    {

    }

    public string GetAnimName(string initialAnim)
    {
        if (tiredness >= GameManagerPersistent.Instance.tirednessLevels.y)
        {
            return "Base Layer." + "Bloody" + initialAnim;
        }
        else if (tiredness >= GameManagerPersistent.Instance.tirednessLevels.x)
        {
            return "Base Layer." + "Tired" + initialAnim;
        }
        else
        {
            return "Base Layer."+initialAnim;
        }

    }

    

}
