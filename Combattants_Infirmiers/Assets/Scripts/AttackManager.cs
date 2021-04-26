using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class AttackManager : TimeKeeper
{
    

    public Attack currentAttack;
    //private Hit currentHit;
    
    
    private LayerMask layersForHitbox;
    
    private float currentFrame; // avoir ca en float permet de ralentir et d'accelerer le temps de tel objet
   

   // [ProgressBar(0, 10)]
    public int hitToVisualize;
    

    [ProgressBar(1, 100)]
    public int visualizeFrame;

    private Collider2D[] colliders;
    private ColliderState _state;
    


    private bool nextInputPressed;


    //for player
    private bool isPlayer;
    private PlayerBase playerBase;

    //for ai
    private bool aiActive;
    private AIController aiController;

    //for gizmos
    public Color inactiveColor;
    public Color openColor;
    public Color collidingColor;

    private Vector2 rotationFactor;
    private bool friend;

    //only for projectiles
    [ShowIf("@this is Projectile")]
    public bool piercing = false;

    //public so that the projectile can access it 
    [HideInInspector]
    public LayerMask friendLayers;
    [HideInInspector]
    public LayerMask foeLayers;

    [HideInInspector]
    public GameObject circleHitbox;
    [HideInInspector]
    public GameObject squareHitbox;
    

    public GameObject attackSign;

    private bool interruptAttack = false;
    [HideInInspector]
    public bool canMove = false;

    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<PlayerBase>() != null)
        {
            SetPlayer(true, this.GetComponent<PlayerBase>()); 
        }
        else if (this.GetComponent<AIController>() != null)
        {
            SetAI(true, this.GetComponent<AIController>());
        }
        //defines which layers are friend and foe
        friendLayers = GameManagerPersistent.Instance.friends;
        foeLayers = GameManagerPersistent.Instance.foes;

        //checks if friend or foe
        if (friendLayers == (friendLayers | (1 << gameObject.layer)))
        {
            friend = true;
        }
        else
        {
            friend = false;
        }
        circleHitbox = GameManagerPersistent.Instance.circleHitbox;
        squareHitbox = GameManagerPersistent.Instance.squareHitbox;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentAttack == null)
        {
            return;
        }

        if (myTime == 0)
        {
            return;
        }

        currentFrame += myTime; // on fait défiler les frames. A terme ce sera géré par animator. 
        visualizeFrame = Mathf.FloorToInt(currentFrame);
        

        //Changes to next hit if total frames reached
        if (currentFrame >= currentAttack.totalFrames + 1)
        {
            //can be rehit
            foreach (Hit h in currentAttack.hits)
            {
                h.hitExceptions.Clear();
            }

            if (currentAttack.hitboxLoops)
            {
                currentFrame = 1;
                visualizeFrame = 1;
                StartCheckingCollision();
            }
            else
            {

                if ((currentAttack.autoCombo || nextInputPressed) && currentAttack.nextAttack != null)
                {
                    InitializeAttack(currentAttack.nextAttack); //si jamais on a differentes routes, ce sera géré ici
                    //currentAttack.currentHit = currentAttack.hits[System.Array.IndexOf(currentAttack.hits, currentAttack.currentHit) + 1];
                    //currentHit = currentAttack.currentHit;
                    //hitToVisualize = System.Array.IndexOf(currentAttack.hits, currentHit);

                }
                else
                {
                    AttackEnd();
                    return;
                }

            }
        }
        
        
            
        

        if (_state == ColliderState.Closed) { return; }


        foreach (Vector2 f in currentAttack.canMoveFrames)
        {
            if (currentFrame >= f.x && currentFrame <= f.y)
            {
                canMove = true;
                break;
            }
            else
            {
                canMove = false;
            }
        }
        

            //sur chaque hitbox
            foreach (Hit currentHit in currentAttack.hits)
        {
            for (int i = 0; i < currentHit.hitboxes.Length; i++)
            {

                Hitbox hb = currentHit.hitboxes[i];

                if (hb.frames.x <= currentFrame && hb.frames.y >= currentFrame)
                {

                    if (hb.layersOverride != 0)
                    {
                        layersForHitbox = hb.layersOverride;
                    }
                    else
                    {
                        //determines if player or enemy
                        layersForHitbox = CheckFriendFoe(currentHit);

                    }


                    Vector2 hitboxPos = (Vector2)transform.position + hb.position.RotateVector(transform.rotation.eulerAngles.z);



                    // cree la bonne forme pour tester l'overlap
                    if (hb._shape == HitboxShape.Cube)
                    {
                        colliders = Physics2D.OverlapBoxAll(hitboxPos, hb.size, transform.rotation.z, layersForHitbox);

                    }
                    else if (hb._shape == HitboxShape.Sphere)
                    {
                        colliders = Physics2D.OverlapCircleAll(hitboxPos, hb.size.x, layersForHitbox); 
                    }
                    //on instancie les hitbox ingame ici pour pas qu'elles repop à chaque frame active
                    if (hb.frames.x == currentFrame)
                    {
                        
                        if (hb._shape == HitboxShape.Cube)
                        {
                            //rendre la hitbox visible ingame grace à un prefab - on supprimera quand on aura de vraies anims
                            GameObject cHitbox = Instantiate(squareHitbox);
                            Vector3 hbScale = new Vector3(hb.size.x, hb.size.y, cHitbox.transform.localScale.z);
                            cHitbox.transform.position = hitboxPos;
                            cHitbox.transform.rotation = this.transform.rotation;
                            cHitbox.transform.localScale = hbScale;
                            cHitbox.GetComponent<HitboxManager>().destroyAfter = Mathf.RoundToInt(hb.frames.y - hb.frames.x);
                            cHitbox.transform.SetParent(this.transform);

                        }
                        else if (hb._shape == HitboxShape.Sphere)
                        {
                            //rendre la hitbox visible ingame grace à un prefab - on supprimera quand on aura de vraies anims
                            GameObject cHitbox = Instantiate(circleHitbox);
                            Vector3 hbScale = new Vector3(hb.size.x, hb.size.x, cHitbox.transform.localScale.z);
                            cHitbox.transform.position = hitboxPos;
                            cHitbox.transform.rotation = this.transform.rotation;
                            cHitbox.transform.localScale = hbScale;
                            cHitbox.GetComponent<HitboxManager>().destroyAfter = Mathf.RoundToInt(hb.frames.y - hb.frames.x);
                            cHitbox.transform.SetParent(this.transform);
                        }
                        
                        
                    }




                    

                    //si elle touche un truc dans le bon layermask
                    if (colliders.Length > 0)
                    {
                        
                        //chaque truc overlapped prend des degats
                        for (int j = 0; j < colliders.Length; j++)
                        {

                            Collider2D aCollider = colliders[j];
                            // if it hasnt already been hit, or not blocked, etc
                            if (!currentHit.hitExceptions.Contains(aCollider))
                            {
                                
                                RaycastHit2D rayhit = Physics2D.Raycast(this.transform.position, aCollider.transform.position - this.transform.position,Mathf.Infinity, layersForHitbox);

                                if (rayhit.collider != null)
                                {
                                    if (rayhit.collider.gameObject.GetComponent<BlockBoxBehaviour>()) // the ray hit the block before hitting the player
                                    {

                                        currentHit.hitExceptions.Add(aCollider);
                                        rayhit.collider.GetComponent<BlockBoxBehaviour>().charaCollider.GetComponent<Fighter>().Block(currentHit.damage);
                                        interruptAttack = true;


                                        if (!isPlayer)
                                        {


                                            if (this is Projectile)
                                            {
                                                this.GetComponent<Projectile>().HitSomething();
                                            }
                                            else
                                            {
                                                this.GetComponent<AIController>().Stunned();
                                            }
                                            return;
                                        }
                                    }
                                    else if (rayhit.collider == aCollider) // the ray hit the player or projectile
                                    {
                                        if (aCollider.GetComponent<Fighter>() != null)
                                        {
                                            if (hb.damageOverride <= 0)
                                            {
                                                currentHit.hitExceptions.Add(aCollider.GetComponent<Fighter>().TakeDamage(currentHit.damage, this.gameObject));
                                            }
                                            else
                                            {
                                                currentHit.hitExceptions.Add(aCollider.GetComponent<Fighter>().TakeDamage(hb.damageOverride, this.gameObject));
                                            }
                                        }
                                        else if (aCollider.GetComponent<Projectile>() != null && !aCollider.GetComponent<Projectile>().piercing && layersForHitbox == (layersForHitbox | (1 << aCollider.GetComponent<Projectile>().originator.layer)) )
                                        {
                                            aCollider.GetComponent<Projectile>().HitSomething();
                                            Debug.Log("yeah");
                                        }
                                        
                                    }
                                }
                                
                                



                                //detruire le projo au contact s'il est pas piercing - on pourra mettre une anim etc
                                if (this is Projectile)
                                {
                                    //this.GetComponent<Projectile>().HitSomething();
                                }
                            }

                        }
                        if (interruptAttack)
                        {
                            interruptAttack = false;
                            AttackInterrupt();
                            return;
                        }

                    }

                }

            }


            // _state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open; // if length isnt 0, state is colliding, else it's open

        }
        
        

        //spawn projos if necessary
        if (currentAttack.spawnObjects)
        {
            foreach (SpawnedObject obj in currentAttack.objectsToSpawn)
            {
                if (currentFrame == obj.frame)
                {
                    GameObject g = Instantiate(obj.objectToSpawn);

                    if (!obj.ignoreRotation) // local scale c'est sale pour check si le perso est retourné ou pas
                    {
                        g.transform.position = this.transform.position + obj.position;
                        g.transform.rotation = this.transform.rotation;
                        g.transform.Rotate(obj.rotation.eulerAngles);
                    }
                    else
                    {
                        g.transform.position = this.transform.position + obj.position;
                        g.transform.rotation = obj.rotation;
                    }

                    if (g.GetComponent<Projectile>())
                    {
                        g.GetComponent<Projectile>().friend = this.friend;
                        g.GetComponent<Projectile>().originator = this.gameObject;
                    }
                }
            }
        }


        if (currentAttack.addMovement) // kinda works, sur les grands nombres c'est bof mais c'est a cause du freinage je pense
        {
            foreach (AttackMovement move in currentAttack.movement)
            {
                if (move.frames.x <= currentFrame && currentFrame <= move.frames.y)
                {
                    //mouvement uniquement lineaire actuellement, peut etre ajouter de la customisation
                    float speed = Mathf.Abs(move.targetPosition.magnitude / (move.frames.y - move.frames.x));
                    if (move.absolute)
                    {

                        this.transform.position = Vector2.MoveTowards((Vector2)transform.position, (Vector2)transform.position + move.targetPosition, speed);
                    }
                    else
                    {

                        this.transform.position = Vector2.MoveTowards((Vector2)transform.position, (Vector2)transform.position + move.targetPosition.RotateVector(transform.rotation.eulerAngles.z), speed);
                    }
                    
                }
            
            }
        }

        //move hitbox if homing projo
        if (this is Projectile)
        {
            if ((this as Projectile).homing)
            {
                (this as Projectile).MoveTowardsTarget();
            }
        }


    }

    void Update() // c'est des inputs faut les mettre dans update pas fixed
    {
        if (myTime == 0)
        {
            return;
        }

        // combos and cancels
        if (currentAttack!=null)
        {

            if (isPlayer && currentAttack.nextAttack != null && !currentAttack.autoCombo)
            {
                CheckForNextAttack();
            }
        }
        
    }

    // combos and cancels
    private void CheckForNextAttack()
    {
        
        if (Input.GetButtonDown(playerBase.GetButtonName("X")))
        {
            if (currentFrame >= currentAttack.cancellableFromFrame - GameManagerPersistent.Instance.bufferingWindow)
            {

                nextInputPressed = true; // successfully buffered next attack
            }

        }
            
        if (currentFrame >= currentAttack.cancellableFromFrame)
        {
            if(nextInputPressed)
            {
                nextInputPressed = false;
                InitializeAttack(currentAttack.nextAttack);
            }
        }

    }




    private void OnDrawGizmosSelected()
    {
        if (currentAttack == null)
        {
            return;
        }
        
        //rotationFactor = currentAttack.ignoreFlip ? Vector2.one : (Vector2)transform.right;

        foreach (Hit hit in currentAttack.hits)
        {
            foreach (Hitbox h in hit.hitboxes)
            {
                if (h.frames.x <= visualizeFrame && h.frames.y >= visualizeFrame)
                {
                    Gizmos.color = Color.red;
                    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
                    if (h._shape == HitboxShape.Cube)
                    {
                        Gizmos.DrawCube(h.position, new Vector3(h.size.x, h.size.y, 1)); // Because size is halfExtents
                    }
                    else if (h._shape == HitboxShape.Sphere)
                    {
                        Gizmos.DrawSphere(h.position, h.size.x); // size x replaces radius
                    }
                }
            }
        }
        
        
        
    }


    private void CheckGizmoColor()
    {
        switch (_state)
        {

            case ColliderState.Closed:

                Gizmos.color = inactiveColor;

                break;

            case ColliderState.Open:

                Gizmos.color = openColor;

                break;

            case ColliderState.Colliding:

                Gizmos.color = collidingColor;

                break;

        }

    }


    public void StartCheckingCollision()
    {
        _state = ColliderState.Open;

    }

    public void StopCheckingCollision()
    {
        _state = ColliderState.Closed;

    }

    
    LayerMask CheckFriendFoe(Hit hit)
    {
        //determines if player or enemy
        if (hit.friendlyFire)
        {
            return friendLayers + foeLayers;
        }

        if (friend)
        {
            return foeLayers;
        }
        else
        {
            return friendLayers;
        }
    }

    public void InitializeAttack(Attack attack)
    {
        currentAttack = Instantiate(attack);
        //layersForHitbox = CheckFriendFoe(currentAttack.currentHit);
        rotationFactor = GetRotationFactor(currentAttack); 
        //Debug.Log(rotationFactor);
        //hitExceptions.Clear();

        StartCheckingCollision();

        currentFrame = 0;
        visualizeFrame = 0;
        hitToVisualize = 0;

        if (aiActive)
        {
            GoingToAttackSign();
        }
    }

    // determines if player or ai
    public void SetPlayer(bool player, PlayerBase newPlayerBase)
    {
        isPlayer = player;
        playerBase = newPlayerBase;

    }
    public void SetAI(bool newAi, AIController newAiController)
    {
        aiActive = newAi;
        aiController = newAiController;
        
    }


    private void AttackEnd()
    {
        currentAttack = null; // pour pas continuer de compter les frames
        if (aiActive && aiController != null)
        {
            aiController.NewAction(); // lance l'action suivante pour l'ia
        }
        else if (isPlayer && playerBase != null)
        {
            playerBase.SetMovementState(new Idle(playerBase));
        }

        
    }

    public void AttackInterrupt()
    {
        currentAttack = null; // pour pas continuer de compter les frames

        if( GetComponentInChildren<HitboxManager>() != null)
        {

            GetComponentInChildren<HitboxManager>().destroyAfter = 0;
        }
    }

    public bool AttackSimulation(Attack attack) // teste si l'ennemi est à portée pour attaquer
    {
        //sur chaque hitbox
        for (int i = 0; i < attack.hits[0].hitboxes.Length; i++)
        {

            Hitbox hb = attack.hits[0].hitboxes[i];

            if (hb.layersOverride != 0)
            {
                layersForHitbox = hb.layersOverride;
            }
            else
            {
                //determines if player or enemy
                layersForHitbox = CheckFriendFoe(attack.hits[0]);

            }
            
            // cree la bonne forme pour tester l'overlap
            if (hb._shape == HitboxShape.Cube)
            {
                colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + (hb.position * GetRotationFactor(attack)), hb.size, 0, layersForHitbox); //le truc de localscale est archi sale, faut check si le perso face right et utiliser un bool dans l'attack
            }
            else if (hb._shape == HitboxShape.Sphere)
            {
                colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + (hb.position * GetRotationFactor(attack)), hb.size.x, layersForHitbox); //Same. On verra quand on aura le comportement de l'ennemi
            }

            int playerTouched = 0;
            foreach (Collider2D col in colliders)
            {
                if (!col.gameObject.GetComponent<BlockBoxBehaviour>())
                {
                    playerTouched += 1;
                }
            }
            //si elle touche un truc dans le bon layermask
            if (playerTouched > 0)
            {
                return true;             
            }


        }
        
        return false;
    }

    public void GoingToAttackSign()
    {
        //put here
        attackSign.SetActive(true);
        StartCoroutine("StopSignAfterTime");
        
    }

    IEnumerator StopSignAfterTime()
    {
        yield return new WaitForSeconds(0.3f);
        attackSign.SetActive(false);
    }

    private Vector2 GetRotationFactor(Attack attack)
    {
        return attack.ignoreRotation ? Vector2.one : (Vector2)transform.right.normalized;
    }

    /*private Quaternion ReflectRotation(Quaternion source, Vector3 normal) //recup sur le net
    {
        return Quaternion.LookRotation(Vector3.Reflect(source * Vector3.forward, normal), Vector3.Reflect(source * Vector3.up, normal));
    }*/

}
