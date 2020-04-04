using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Grid
    {
        int cellSize;
        Soldier[,] cells;

        public Grid(int mapWidth, int cellSize)
        {
            this.cellSize = cellSize;
            int numberOfCells = mapWidth / cellSize;
            cells = new Soldier[numberOfCells, numberOfCells];
            for (int i = 0; i < numberOfCells; i++)
            {
                cells[i, i] = null;
            }
        }

        public void Add(Soldier soldier)
        {
            int cellX = Mathf.FloorToInt((soldier.soldierTrans.position.x / cellSize));
            int cellZ = Mathf.FloorToInt((soldier.soldierTrans.position.z / cellSize));

            // See the previous Soldier to null - Will be set by a Soldier coming after
            soldier.previousSoldier = null;
            // Set the next soldier to the previous soldier residing on the list
            // For the case of a cell which previously hosted no units
            if (cells[cellX,cellZ] != null)
            {
                soldier.nextSoldier = cells[cellX, cellZ];
            } 
            cells[cellX, cellZ] = soldier;
            if (soldier.nextSoldier != null)
            {
                soldier.nextSoldier.previousSoldier = soldier;
            }
        }

        public Soldier FindClosestEnemy(Soldier friendlySoldier)
        {
            //Debug.Log(cellSize);
            int cellX = Mathf.FloorToInt((friendlySoldier.soldierTrans.position.x / cellSize));
            int cellZ = Mathf.FloorToInt((friendlySoldier.soldierTrans.position.z / cellSize));

            Soldier enemy;
            enemy = cells[cellX, cellZ];
            if (enemy != null && enemy.soldierTrans == null)
                enemy = null;

            Soldier closestSoldier = null;
            float bestDistSqr = Mathf.Infinity;
            while (enemy != null)
            {
                //  Sometimes an object that was just destroyed gets queired before its removed from the list
                //  if (enemy.soldierTrans != null)
                // {
                    float distSqr = (enemy.soldierTrans.position - friendlySoldier.soldierTrans.position).sqrMagnitude;
                    if (distSqr < bestDistSqr)
                    {
                        bestDistSqr = distSqr;
                        closestSoldier = enemy;
                    }
               // }
                enemy = enemy.nextSoldier;
            }
            return closestSoldier;
        }
        public void Move(Soldier soldier, Vector3 oldPos, bool removeFromGrid)
        {
            int oldCellX = Mathf.FloorToInt((soldier.soldierTrans.position.x / cellSize));
            int oldCellZ = Mathf.FloorToInt((soldier.soldierTrans.position.z / cellSize));


            int cellX = Mathf.FloorToInt((soldier.soldierTrans.position.x / cellSize));
            int cellZ = Mathf.FloorToInt((soldier.soldierTrans.position.z / cellSize));

            if (oldCellX == cellX && oldCellZ == cellZ && !removeFromGrid)
                return;
            if (soldier.previousSoldier != null)
                soldier.previousSoldier.nextSoldier = soldier.nextSoldier;
            if (soldier.nextSoldier != null)
                soldier.nextSoldier.previousSoldier = soldier.previousSoldier;
            if (cells[oldCellX, oldCellZ] == soldier)
                cells[oldCellX, oldCellZ] = soldier.nextSoldier;
            if (!removeFromGrid)
                Add(soldier);

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

