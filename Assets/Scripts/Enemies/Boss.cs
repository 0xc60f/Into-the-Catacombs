using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class Boss: MonoBehaviour
{
    //target to attack
    [SerializeField] GameObject attackTarget;

    //target to move to
    [SerializeField] Transform moveTarget;


    //game manager
    GameManager manager;

    //projectile to fire
    [SerializeField] GameObject projectile;

    //guard speed
    [SerializeField] float speed = 3f;

    //a* depen
    AIPath pathfinder;

    AIDestinationSetter goToDest;

    //layers to ignore when detecting targets
    [SerializeField] LayerMask enemyLayers;

    [SerializeField] LayerMask stopMovementLayerMask;
    Animator animator;

    //attack properties
    int maxClipSize = 30;

    int currClipSize = 30;

    int reloadTime = 300;

    float fireRate = 0.06f;

    int frameReloaded = -1;

    bool isShooting = false;

    //fire angle deviation
    [SerializeField] float accuracy;

    //behavior types
    [SerializeField] int behaviorType = 0;

    void Awake(){
        goToDest = GetComponent<AIDestinationSetter>();
        pathfinder = GetComponent<AIPath>();
        manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    void Start(){

        moveTarget.position = manager.GetAvailablePosition();

        goToDest.target = moveTarget;

        pathfinder.maxSpeed = speed;

    }

    void FixedUpdate(){
        HandleMovement();
        HandleAttack();
    }


    #region movement
    
    void HandleMovement(){
        pathfinder.maxSpeed = speed;
        if (attackTarget != null){
            moveTarget.position = attackTarget.transform.position;
        }
        
    }

    #endregion

    #region Attack

    bool targetInCrosshair = false;

    Vector2 lastKnownPosition;

    void HandleAttack(){
        
        if (attackTarget != null && !isShooting){
            StartCoroutine(SlowSuppress(5));
        }
        
    }

    IEnumerator Suppress(int numBullets){
        isShooting = true;
        for (int i = 0; i < numBullets; i++){
            ShootBullet(accuracy * 1.2f);
            float secs = fireRate * Random.Range(1f, 1.5f);
            //if (manager.timeSlow)
                //secs *= manager.slowFactor;
            yield return new WaitForSeconds(secs);
        }
        isShooting = false;
    }

    IEnumerator SlowSuppress(int numBullets){
        isShooting = true;
        for (int i = 0; i < numBullets; i++){
            ShootBullet(accuracy);
            float secs = fireRate * Random.Range(8f, 18f);
            //if (manager.timeSlow)
              //  secs *= manager.slowFactor;
            yield return new WaitForSeconds(secs);
        }
        isShooting = false;
    }

    void Reload(){
        frameReloaded = manager._fixedFrame + reloadTime;
    }

    void ShootBullet(float dev){
        if (attackTarget == null)
            return;
        currClipSize -= 1;
        GameObject proj = Instantiate(projectile, transform);//, (Vector2) transform.position + new Vector2(0f, 1.8f) * , transform.rotation);
        proj.transform.parent = null;

        proj.transform.up = attackTarget.transform.position - transform.position + new Vector3(Random.Range(-dev, dev), Random.Range(-dev, dev), 0);

        if (manager.timeSlow)
        {
            proj.GetComponent<Projectile>().vel /= manager.slowFactor;
        }
    }


    #endregion

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.CompareTag("Player")){
            attackTarget = coll.gameObject;
        }
    }


    

    void OnTriggerExit2D(Collider2D coll){
        if (coll.gameObject.CompareTag("Player")){
            attackTarget = null;
        }

    }


}
