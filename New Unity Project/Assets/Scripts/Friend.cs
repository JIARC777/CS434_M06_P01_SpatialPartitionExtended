using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    /*
    public class Friend : Soldier
    {
        // Start is called before the first frame update
        public Friend(GameObject soldierObj, float mapWidth, Grid grid)
        {
            this.soldierTrans = soldierObj.transform;
            this.walkSpeed = 2f;

            this.soldierTrans = soldierObj.transform;
            this.soldierMeshRenderer = soldierObj.GetComponent<MeshRenderer>();
            this.mapWidth = mapWidth;
            this.grid = grid;

            grid.Add(this);

            oldPos = soldierTrans.position;
            this.walkSpeed = 5f;
        }
        public override void Move(Soldier closestEnemy)
        {
            soldierTrans.rotation = Quaternion.LookRotation(closestEnemy.soldierTrans.position - soldierTrans.position);
            soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
        }
    }
    */
}

