using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GameManager : MonoBehaviour
{
    public int _fixedFrame = 0;

    public bool timeSlow = false;

    public float slowFactor = 4f;

    GridLayout grid;
    
    List<Vector2> openPositions;
    
    void Awake(){
        openPositions = new List<Vector2>();
        ProcessTiles();
    }

    void FixedUpdate(){
       _fixedFrame++;
    }

    public void setTimeSlow(){
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject proj in projectiles){
            proj.GetComponent<Projectile>().vel /= slowFactor;

        }
    }

    public void setTimeNormal(){
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject proj in projectiles){
            proj.GetComponent<Projectile>().vel *= slowFactor;
        }
    }

    public void ProcessTiles(){
        var grid = AstarPath.active.data.gridGraph;

        foreach (GraphNode gridNode in grid.nodes){
            if (gridNode.Walkable){
                openPositions.Add((Vector2) ((Vector3) gridNode.position));
            }
        }
    }

    void GetRandomAvailablePointInCircle(Vector2 pos1, Vector2 pos2) {
        Vector2 midPoint = (pos1 + pos2)/(new Vector2(2f, 2f));
        float rad = Vector2.Distance(pos1, pos2)/2f;
        //Random.inside   
    }

    public Vector2 GetAvailablePosition(){
        return openPositions[Random.Range(0, openPositions.Count)];
    }

}

