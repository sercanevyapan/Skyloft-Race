using System;
using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public  float speed;
        public float maxSpeed, crashSpeed;
        float distanceTravelled;

        public bool RotationIgnore, isObstacle,isSpeedIncease;

        void Start() {
            speed = maxSpeed;
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null && !isObstacle)
            {

                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

                if (RotationIgnore)
                {
                    Quaternion newRot = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                    newRot.x = 0;
                    newRot.z = 0;

                    transform.rotation = newRot;
                }
                else
                {
                    transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                }


                
            }else if(pathCreator != null && isObstacle)
            {
                distanceTravelled -= crashSpeed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                
            }

            if (isSpeedIncease)
            {
                SpeedIncrease();
            }
        }

    



        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

         public void  SpeedIncrease()
        {
            if(speed<=maxSpeed)
                speed += 10f* Time.deltaTime;

            print(speed);
        }

    }
}