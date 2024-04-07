using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class SoldierGrunt : MonoBehaviour
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

    AIPath pathfinder;

    AIDestinationSetter goToDest;

    //layers to ignore when detecting targets
    [SerializeField] LayerMask enemyLayers;

    [SerializeField] LayerMask stopMovementLayerMask;

    //fire angle deviation
    [SerializeField] float accuracy;

    //behavior types
    bool isAttacking = false;


    void Awake(){
        goToDest = GetComponent<AIDestinationSetter>();
        pathfinder = GetComponent<AIPath>();
        manager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    void Start(){

        moveTarget.position = manager.GetAvailablePosition();

        goToDest.target = moveTarget;

        pathfinder.maxSpeed = speed;

        //DisturbanceCall();
    }

    void FixedUpdate(){
        HandleMovement();
        HandleAttack();
        HandleDialogue();
        
    }

    #region movement
    /*0 = wandering
    1: suppress
    2: intercept up close
    3: running the hell away
    4: investigate
    */
    void HandleMovement(){
        pathfinder.maxSpeed = speed;
        if (!isAttacking){
            speed = 3.5f;
            if (Vector2.Distance(transform.position, moveTarget.transform.position) < 2f && attackTarget == null){
                moveTarget.position = manager.GetAvailablePosition();
            }
        } 
        else{

        }
    }


    #endregion

    #region Attack

    void HandleAttack(){
        //if enemy in crosshair and in line of sight, fire
        if (attackTarget != null){
            
        }
    }

    void ShootBullet(){
        GameObject proj = Instantiate(projectile, transform);
        proj.transform.parent = null;

        proj.transform.up = 
            attackTarget.transform.position - transform.position + new Vector3(Random.Range(-1 * accuracy, accuracy), Random.Range(-1 * accuracy, accuracy), 0);

    }
    #endregion

    #region Dialogue

    //dialogue variables
    [SerializeField] Dialogue dialogue;

    string[] dialogueSamples = {
    "Wasserschwein",
    "Zufallige Worter!",
    "Laternenpfahl",
    "Insektenfresser",
    "Pappmaus",
    "zufallige Phrasen",
    "brauche Wasser",
    "Ich bin mude",
    "zu viele Scheren"
    };

    void HandleDialogue(){
        if (Random.Range(0, 1000) == 1){
            string sent = dialogueSamples[Random.Range(0, dialogueSamples.Length)];
            dialogue.StopText();
            dialogue.StartSentence(sent);
        }
    }

    #endregion
    
    void OnTriggerEnter2D(Collider2D hit){
        /*RaycastHit2D hit = Physics2D.Raycast(transform.position, coll.transform.position, 100f, enemyLayers);
        if (hit.collider != null){
            GameObject target = hit.collider.gameObject;
            
        }*/
        if (hit.GetComponent<Collider>().gameObject.tag == "Player"){
            isAttacking = true;
            attackTarget = hit.GetComponent<Collider>().gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D hit){
        /*attackTarget = null;
        pathfinder.enableRotation = true;
        pathfinder.maxSpeed = 3.5f;*/
        if (hit.GetComponent<Collider>().gameObject.tag == "Player"){
            isAttacking = false;
            attackTarget = null;//hit.collider.gameObject;
        }
    }

}