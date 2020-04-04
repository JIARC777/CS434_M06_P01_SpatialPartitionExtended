using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Soldier
    {
        public MeshRenderer soldierMeshRenderer;
        public Transform soldierTrans;

        public GameObject soldierGO;
        public float health = 100f;
        public float baseDamage = 5f;
        public float damageModifier = 5f;
        public float mapWidth;
        protected float walkSpeed;
        Vector3 oldPos;
        Vector3 wanderTarget;
        Grid grid;

        //Solider Class is a linked List with soldiers being links - each has a reference to the next and previous
        public Soldier previousSoldier;
        public Soldier nextSoldier;

        // Start is called before the first frame update
        public Soldier(GameObject soldierObj, Grid grid, float mapWidth)
        {
            this.soldierTrans = soldierObj.transform;
            this.soldierMeshRenderer = soldierObj.GetComponent<MeshRenderer>();
            this.grid = grid;
            this.soldierGO = soldierObj;
            this.mapWidth = mapWidth;
            grid.Add(this);

            oldPos = soldierTrans.position;
            this.walkSpeed = 5f;
            GetNewTarget();
        }
        public bool TakeDamage()
        {
            bool isDestroyed;
            health -= baseDamage + Random.Range(0f, damageModifier);
            if (health <= 0)
            {
                KillSoldier();
                isDestroyed = true;
            }
            else
                isDestroyed = false;
            return isDestroyed; 
        }
        public void Move(Soldier closestEnemy)
        {
            soldierTrans.rotation = Quaternion.LookRotation(closestEnemy.soldierTrans.position - soldierTrans.position);
            soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
            grid.Move(this, oldPos, false);
            oldPos = soldierTrans.position;
        }
        public void Move()
        {
            soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
            grid.Move(this, oldPos, false);
            oldPos = soldierTrans.position;
            if ((soldierTrans.position - wanderTarget).magnitude < 1f)
                GetNewTarget();
        }
        // Update is called once per frame
        void GetNewTarget()
        {
            wanderTarget = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));
            soldierTrans.rotation = Quaternion.LookRotation(wanderTarget - soldierTrans.position);
        }
        bool KillSoldier()
        {
            grid.Move(this, oldPos, true);
            GameObject.Destroy(this.soldierMeshRenderer);
            return true;

        }
    }
}

