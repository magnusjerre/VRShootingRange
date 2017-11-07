using UnityEngine;

namespace Jerre {
    public class SequentialSpawner : MonoBehaviour, ITargetSpawner
    {
        [SerializeField] private bool SpawnForwards = true;
        [SerializeField] private float initialSpawnDelay;
        [SerializeField] private SpawnInfo initialSpawning;
        [SerializeField] private SpawnInfo[] spawnings;
        [SerializeField] private int repeat = -1;

        private float elapsedTime;
        private int currentSpawnIndex, currentChildIndex;

        private bool canSpawn, isInitialPhase, waitForInitialDelay, waitForInitialNextSpawnDelay;

        private MJRandom random;

        void Awake()  {
            random = new MJRandom(1);
        }

        void Update ()
        {
            if (!canSpawn) {
                return;
            }

            elapsedTime += Time.deltaTime;
            if (isInitialPhase)
            {
                HandleInitialPhase();
            }
            else {
                if (elapsedTime >= spawnings[currentSpawnIndex].nextSpawnDelay) {
                    elapsedTime = 0f;
                    var target = SpawnTarget(spawnings[currentSpawnIndex].target, transform.GetChild(currentChildIndex));
                    target.lifetime = spawnings[currentSpawnIndex].targetLifetime; 
                    currentSpawnIndex = (currentSpawnIndex + 1) % spawnings.Length;                   
                    NextChild();
                    ReduceRepeat();
                }
            }
        }

        private void HandleInitialPhase()
        {
            if (waitForInitialDelay && elapsedTime >= initialSpawnDelay)
            {   //Finished waiting for initial delay, should spawn target
                waitForInitialDelay = false;
                waitForInitialNextSpawnDelay = true;
                elapsedTime = 0f;
                if (initialSpawning.target == null) {
                    waitForInitialNextSpawnDelay = false;
                    isInitialPhase = false;
                } else {
                    var target = SpawnTarget(initialSpawning.target, transform.GetChild(0));
                    target.lifetime = initialSpawning.targetLifetime;
                    NextChild();
                    ReduceRepeat();
                }
            }
            else if (waitForInitialNextSpawnDelay && elapsedTime >= initialSpawning.nextSpawnDelay)
            {   //Initial target delay finished, spawn spawnings[0] immediately
                waitForInitialDelay = false;
                waitForInitialNextSpawnDelay = false;
                isInitialPhase = false;
                elapsedTime = 0f;
                var target = SpawnTarget(spawnings[currentSpawnIndex].target, transform.GetChild(currentChildIndex));
                target.lifetime = spawnings[currentSpawnIndex].targetLifetime;
                currentSpawnIndex = (currentSpawnIndex + 1) % spawnings.Length;
                NextChild();
                ReduceRepeat();
            }
        }

        private void ReduceRepeat() {
            if (repeat == -1) {
                return;
            }
            if (repeat == 0 || --repeat == 0) {
                enabled = false;
            }
        }

        void NextChild() {
            currentChildIndex = (currentChildIndex + 1) % transform.childCount;
        }

        private Target SpawnTarget(Target prefab, Transform spawnPoint)
        {
            var output = Instantiate(prefab, spawnPoint);
            if (!SpawnForwards) {
                output.FaceForward = random.Next(0, 2) > 0 ? true : false;
            }
            output.transform.parent = transform.GetChild(currentChildIndex);
            return output;
        }

        private void SpawnNext() {
            var newTarget = SpawnTarget(spawnings[currentSpawnIndex].target, transform);
            currentSpawnIndex = (currentSpawnIndex + 1) % spawnings.Length;
        }

        public void StartSpawningProcess()
        {
            canSpawn = true;
            isInitialPhase = true;
            waitForInitialDelay = true;
            enabled = true;
        }

        public void EndSpawningProcess()
        {
            canSpawn = false;
            for (var i = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i);
                if (child.childCount == 0) {
                    continue;
                }
                var childTarget = child.GetComponentInChildren<Target>();
                if (childTarget == null) {
                    continue;
                }

                childTarget.HideTarget();
            }
            if (transform.childCount > 0) {
                transform.GetComponentInChildren<Target>().HideTarget();
            }
            enabled = false;
        }
    }
}