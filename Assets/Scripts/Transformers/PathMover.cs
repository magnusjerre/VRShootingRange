using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class PathMover : MonoBehaviour
    {

        public Transform[] path;
        public float time;
        public Space space;
        private float elapsedTime;


        // Use this for initialization
        void Start()
        {
            if (path.Length == 0)
            {   //Assume part of a spawner setup

                var parent = transform.parent;
                var spawner = transform.GetComponentInParent<SequentialSpawner>();
                for (var c = 0; c < spawner.transform.childCount; c++)
                {
                    var spawn = spawner.transform.GetChild(c);
                    if (transform.IsChildOf(spawn))
                    {
                        path = new Transform[spawn.childCount - 1];
                        for (var i = 0; i < spawn.childCount; i++)
                        {
                            var child = spawn.GetChild(i);
							try {
								var index = int.Parse(child.name);
								path[index] = child;
							} catch (FormatException e) {
								Debug.Log("------- Couldn't int-parse the following: " + child.name);
							}
                        }
						transform.position = path[0].position;
						transform.rotation = path[0].rotation;
						break;
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / time);
            Vector3 p0 = GetPos(0);
            Vector3 p1 = GetPos(1);
            if (IsCubic())
            {
                Vector3 p2 = GetPos(2);
                Vector3 p3 = GetPos(3);
                float p = 1 - t;

                Vector3 pos = (p * p * p * p0) + (3 * p * p * t * p1) + (3 * p * t * t * p2) + (t * t * t * p3);
				Vector3 deltaPos = pos - (space == Space.World ? transform.position : transform.localPosition);

                if (space == Space.World)
                {
                    transform.position = pos;
                }
                else
                {
                    transform.localPosition = pos;
                }
				transform.LookAt(transform.position + deltaPos, transform.up);
            }
        }

        bool IsCubic()
        {
            return path.Length == 4;
        }

        Vector3 GetPos(int index)
        {
            return space == Space.World ? path[index].position : path[index].localPosition;
        }

    }
}