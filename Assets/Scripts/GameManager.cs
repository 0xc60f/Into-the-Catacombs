using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public int _fixedFrame = 0;

    public bool timeSlow = false;

    public float slowFactor = 4f;

    [SerializeField] Tilemap tilemap;

    GridLayout grid;
    
    List<Vector2> openPositions;
    
    void Awake(){
        openPositions = new List<Vector2>();
        grid = tilemap.layoutGrid;
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
        //int x = 0;
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {   
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y);
            Vector3 place = tilemap.CellToWorld(localPlace);
            if (tilemap.HasTile(localPlace))
            {
                openPositions.Add(new Vector2(place.x, place.y));
            }
        }
 
    }

    public Vector2 GetAvailablePosition(){
        return openPositions[Random.Range(0, openPositions.Count)];
    }

}
