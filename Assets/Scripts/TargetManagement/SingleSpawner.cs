using System;
using UnityEngine;

namespace Jerre
{
    public class SingleSpawner : MonoBehaviour, ITargetSpawner
    {
        [SerializeField] private bool SpawnForwards = false;

        [SerializeField] private float initialSpawnDelay;
        [SerializeField] private SpawnInfo initialSpawning;
        [SerializeField] private SpawnInfo[] spawnings;
        [SerializeField] private int repeat = -1;
        private float elapsedTime;
        private int currentIndex;

        private bool canSpawn, isInitialPhase, waitForInitialDelay, waitForInitialNextSpawnDelay;

        private MJRandom random;

        void Awake()
        {
            random = new MJRandom(1);
        }

        void Start()
        {
            for (var i = 0; i < spawnings.Length; i++)
            {
                var spawnInfo = spawnings[i];
                if (spawnInfo.nextSpawnDelay < spawnInfo.targetLifetime + 1)
                {
                    spawnInfo.nextSpawnDelay = spawnInfo.targetLifetime + 1;
                }
            }
        }

        void Update()
        {
            if (!canSpawn)
            {
                return;
            }

            elapsedTime += Time.deltaTime;
            if (isInitialPhase)
            {
                HandleInitialPhase();
            }
            else
            {
                if (elapsedTime >= spawnings[currentIndex].nextSpawnDelay)
                {
                    elapsedTime = 0f;
                    var target = SpawnTarget(spawnings[currentIndex].target, transform);
                    target.lifetime = spawnings[currentIndex].targetLifetime;
                    currentIndex = (currentIndex + 1) % spawnings.Length;
                    ReduceRepeat();
                }
            }
        }

        private void ReduceRepeat()
        {
            if (repeat == -1)
            {
                return;
            }
            if (repeat == 0 || --repeat == 0)
            {
                enabled = false;
            }
        }

        private void HandleInitialPhase()
        {
            if (waitForInitialDelay && elapsedTime >= initialSpawnDelay)
            {   //Finished waiting for initial delay, should spawn target
                waitForInitialDelay = false;
                waitForInitialNextSpawnDelay = true;
                elapsedTime = 0f;
                if (initialSpawning.target == null)
                {
                    waitForInitialNextSpawnDelay = false;
                    isInitialPhase = false;
                }
                else
                {
                    var target = SpawnTarget(initialSpawning.target, transform);
                    target.lifetime = initialSpawning.targetLifetime;
                    ReduceRepeat();
                }
            }
            else if (waitForInitialNextSpawnDelay && elapsedTime >= initialSpawning.nextSpawnDelay)
            {   //Initial target delay finished, spawn spawnings[0] immediately
                waitForInitialDelay = false;
                waitForInitialNextSpawnDelay = false;
                isInitialPhase = false;
                elapsedTime = 0f;
                var target = SpawnTarget(spawnings[currentIndex].target, transform);
                target.lifetime = spawnings[currentIndex].targetLifetime;
                currentIndex = (currentIndex + 1) % spawnings.Length;
                ReduceRepeat();
            }
        }

        private Target SpawnTarget(Target prefab, Transform spawnPoint)
        {
            var output = Instantiate(prefab, spawnPoint);
            if (!SpawnForwards)
            {
                output.FaceForward = random.Next(0, 2) > 0 ? true : false;
            }
            output.transform.parent = transform;
            return output;
        }

        private void SpawnNext()
        {
            var newTarget = SpawnTarget(spawnings[currentIndex].target, transform);
            currentIndex = (currentIndex + 1) % spawnings.Length;
        }

        public void StartSpawningProcess()
        {
            canSpawn = true;
            isInitialPhase = true;
            waitForInitialDelay = true;
        }

        public void EndSpawningProcess()
        {
            canSpawn = false;
            if (transform.childCount > 0)
            {
                transform.GetComponentInChildren<Target>().HideTarget();
            }
        }
    }
}