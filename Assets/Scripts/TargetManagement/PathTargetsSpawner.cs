using UnityEngine;

namespace Jerre {
	public class PathTargetsSpawner : MonoBehaviour, ITargetSpawner {

		[SerializeField] Transform[] pathParents;
		[SerializeField] float initialSpawnDelay;
		[SerializeField] SpawnPathInfo[] targetSpawns;

		SpawnPathInfo[] spawnings;
		bool allowedToSpawnByGameController;
		float timeSinceLastSpawn;
		int currentSpawningIndex, pathParentsIndex;

		void Awake() {
			SetupSpawningInfos ();
		}

		void SetupSpawningInfos () {
			int startIndex = 0;
			if (initialSpawnDelay > 0) {
				spawnings = new SpawnPathInfo[targetSpawns.Length + 1];
				spawnings [0] = SpawnPathInfo.Empty (initialSpawnDelay);
				startIndex = 1;
			}
			else {
				spawnings = new SpawnPathInfo[targetSpawns.Length];
			}
			for (var i = 0; i < targetSpawns.Length; i++) {
				spawnings [i + startIndex] = targetSpawns [i];
			}
		}

		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
			if (!allowedToSpawnByGameController) {
				return;
			}

			timeSinceLastSpawn += Time.deltaTime;
			var currentSpawn = spawnings [currentSpawningIndex];
			if (timeSinceLastSpawn > currentSpawn.timeUntilNextSpawn) {
				timeSinceLastSpawn = 0f;
				currentSpawningIndex = (currentSpawningIndex + 1) % spawnings.Length;
				pathParentsIndex = (pathParentsIndex + 1) % pathParents.Length;
				var nextSpawn = spawnings[currentSpawningIndex];
				if (nextSpawn.target != null) {
					SetupTarget (nextSpawn, pathParents[pathParentsIndex]);
				}
			}
		}

		void SetupTarget(SpawnPathInfo spawnInfo, Transform pathParent) {
			var targetParent = Instantiate (spawnInfo.target, transform);
			targetParent.lifetime = spawnInfo.targetLifetime;
			SetupInOutScalerTriggers (targetParent);

			var pathMover = targetParent.gameObject.AddComponent <PathMover>();

			var actualTarget = targetParent.GetComponentInChildren <TargetCollider> ().gameObject;
			actualTarget.transform.localScale = spawnInfo.scale;

			if (spawnInfo.facePlayerAlways) {
				var lookAt = actualTarget.AddComponent <LookAt> ();
				lookAt.target = Vector3.zero;
				lookAt.onlyXZDirection = true;
				lookAt.continuous = spawnInfo.facePlayerAlways;
			}
			var path = new Transform[pathParent.childCount];
			for (var i = 0; i < path.Length; i++) {
				path [i] = pathParent.GetChild (i);
			}
			pathMover.path = path;
			pathMover.time = spawnInfo.targetLifetime;
			pathMover.space = Space.World;
		}

		void SetupInOutScalerTriggers(Target target) {
			target.transform.localScale = Vector3.zero;

			var scaleInTrigger = target.gameObject.AddComponent <ScalerTrigger> ();
			var waitTrigger = target.gameObject.AddComponent <WaitTrigger> ();
			var scaleOutTrigger = target.gameObject.AddComponent <ScalerTrigger>();

			float scaleTime = 0.5f;

			scaleInTrigger.scaleTo = Vector3.one;
			scaleInTrigger.time = scaleTime;
			scaleInTrigger.AddProgrammatically(waitTrigger);

			waitTrigger.waitTime = target.lifetime - 2 * scaleTime;
			waitTrigger.AddProgrammatically(scaleOutTrigger);

			scaleOutTrigger.scaleTo = Vector3.zero;
			scaleOutTrigger.time = scaleTime;

			scaleInTrigger.Trigger ();
		}

		#region ITargetSpawner implementation

		public void StartSpawningProcess () {
			allowedToSpawnByGameController = true;
			timeSinceLastSpawn = 0f;
			currentSpawningIndex = 0;
		}

		public void EndSpawningProcess () {
			allowedToSpawnByGameController = false;
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
		}

		#endregion
	}
}