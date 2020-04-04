using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialPartitionPattern
{
    public class GameController : MonoBehaviour
    {
        public GameObject friendlyObj;
        public GameObject enemyObj;

        public Material enemyMaterial;
        public Material friendlyMaterial;
        public Material takingDamageMaterial;

        public Transform enemyParent;
        public Transform friendlyParent;

        List<Soldier> enemySoldiers = new List<Soldier>();
        List<Soldier> friendlySoldiers = new List<Soldier>();
        List<Soldier> closestEnemies = new List<Soldier>();
        List<Soldier> closestFriendlies = new List<Soldier>();

        float mapWidth = 100f;
        int cellSize = 10;
        int numberOfSoldiers = 200;

        float summedTime;
        float avgTime;
        int numberOfSamples;

        bool isSpatial;

        Grid enemyGrid;
        Grid friendlyGrid;


        // Start is called before the first frame update
        void Start()
        {
            enemyGrid = new Grid((int)mapWidth, cellSize);
            friendlyGrid = new Grid((int)mapWidth, cellSize);
            for (int i = 0; i < numberOfSoldiers; i++)
            {
                Vector3 randomPos = new Vector3(Random.Range(5f, mapWidth/2f - 5f), 1f, Random.Range(5f, mapWidth-5f));
                GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity) as GameObject;
                enemySoldiers.Add(new Soldier(newEnemy, enemyGrid, mapWidth));
                //Debug.Log("");
                newEnemy.transform.parent = enemyParent;

                randomPos = new Vector3(Random.Range(mapWidth / 2f - 5f, mapWidth - 5f), 1f, Random.Range(5f, mapWidth - 5f)); ;
                GameObject newFriendly = Instantiate(friendlyObj, randomPos, Quaternion.identity) as GameObject;
                friendlySoldiers.Add(new Soldier(newFriendly, friendlyGrid, mapWidth));
                newFriendly.transform.parent = friendlyParent;
            }
        }

        // Update is called once per frame
        
        void Update()
        {
            closestFriendlies.Clear();
            closestEnemies.Clear();
            // The code to change materials is duplicated to make analyzing timestamps easy and accurate as the only line to change is the search algorthim
            // Timestamp start for using slow algorthm
            float startTimeStamp = Time.realtimeSinceStartup * 1000;
            bool isDestroyed = false;
            for (int i = 0; i < friendlySoldiers.Count; i++)
            {
                // For Some Reason Destoyed units will Still Look for a closest Enemy. To avoid that check for a null soldier and remove
                /*
                if (friendlySoldiers[i] == null)
                {
                    friendlySoldiers.RemoveAt(i);
                    continue;
                }
                */
                Soldier closestEnemy;   
                closestEnemy = enemyGrid.FindClosestEnemy(friendlySoldiers[i]);
                
                friendlySoldiers[i].soldierMeshRenderer.material = friendlyMaterial;
                //closestEnemy = FindClosestEnemySlow(friendlySoldiers[i]);
                if (closestEnemy != null)
                {
                    //closestEnemy.soldierMeshRenderer.material = closestEnemyMaterial;
                    closestEnemies.Add(closestEnemy);
                    friendlySoldiers[i].Move(closestEnemy);
                    if (isSameLocation(friendlySoldiers[i], closestEnemy))
                    {
                        friendlySoldiers[i].soldierMeshRenderer.material = takingDamageMaterial;
                        isDestroyed = friendlySoldiers[i].TakeDamage();
                        if (isDestroyed)
                        {
                            friendlySoldiers.RemoveAt(i);
                        }
                    }
                } else
                    friendlySoldiers[i].Move();
            }
            float endTimeStamp = Time.realtimeSinceStartup * 1000;
            for (int i = 0; i < enemySoldiers.Count; i++)
            {
                // For Some Reason Destoyed units will Still Look for a closest Enemy. To avoid that check for a null soldier and remove
                /*
                if (enemySoldiers[i] == null)
                {
                    enemySoldiers.RemoveAt(i);
                    continue;
                }
                */
                Soldier closestFriendly;
                closestFriendly = friendlyGrid.FindClosestEnemy(enemySoldiers[i]);
                enemySoldiers[i].soldierMeshRenderer.material = enemyMaterial;

                if (closestFriendly != null)
                {
                    //closestFriendly.soldierMeshRenderer.material = closestMaterial;
                    closestFriendlies.Add(closestFriendly);
                    enemySoldiers[i].Move(closestFriendly);
                    if (isSameLocation(enemySoldiers[i], closestFriendly))
                    {
                        enemySoldiers[i].soldierMeshRenderer.material = takingDamageMaterial;
                        isDestroyed = enemySoldiers[i].TakeDamage();
                        if (isDestroyed)
                        {
                            enemySoldiers.RemoveAt(i);
                        }
                    }
                    
                } else
                    enemySoldiers[i].Move();
            }

            numberOfSamples++;
            summedTime += (endTimeStamp - startTimeStamp);
           // Debug.Log(summedTime);
            avgTime = summedTime / numberOfSamples;
        }
        bool isSameLocation(Soldier s1, Soldier s2)
        {
            float threshold = 2f;
            return (s1.soldierTrans.position - s2.soldierTrans.position).magnitude <= threshold;
        }
        /*
        Soldier FindClosestEnemySlow(Soldier soldier)
        {
            Soldier closestEnemy = null;
            float bestDistSqr = Mathf.Infinity;
            for (int i = 0; i < enemySoldiers.Count; i++)
            {
                float distSqr = (soldier.soldierTrans.position - enemySoldiers[i].soldierTrans.position).sqrMagnitude;
                if (distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;
                    closestEnemy = enemySoldiers[i];
                }
            }
            return closestEnemy;
            
        }   
        */

        public void toggleSpatialPartition()
        {
            isSpatial = !isSpatial;
            summedTime = 0;
            numberOfSamples = 0;
        }

        
    }
}

