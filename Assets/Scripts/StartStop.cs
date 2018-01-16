using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jerre
{
    public class StartStop : MonoBehaviour, IHittable, IListener
    {

        private bool canReciveHit;
        private GameController gameController;
        [SerializeField] private BaseTriggerable initTrigger, showDoor, hideDoor;
        public Renderer doorRenderer;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private PeriodicEmitter emitter;

        // Use this for initialization
        void Start()
        {
            canReciveHit = true;
			gameController = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameController>();
            gameController.setStartStopInteractor(this);
            initTrigger.AddListener(this);
            showDoor.AddListener(this);
            hideDoor.AddListener(this);
            emitter.StartEmission();
        }

        // Update is called once per frame
        void Update()
        {

        }


		public Hit RegisterHit(RaycastHit hit)
        {
            Debug.Log("registered hit on startstopv2");
            if (!canReciveHit) {
                return Hit.Miss();
            }
            canReciveHit = false;
            gameController.StartButtonClicked();
            hideDoor.Trigger();
            return Hit.Miss();
        }

        public void Notify(object notifier)
        {
            if (notifier == hideDoor || notifier == initTrigger) {
                SetCorrectMaterial();
                showDoor.Trigger();
            } else if (notifier == showDoor) {
                canReciveHit = true;
            }
        }

        private void SetCorrectMaterial() {
            if (gameController.IsPlaying) {
                targetTransform.localRotation = Quaternion.Euler(Vector3.up * 180f);
                emitter.StopEmission();
            } else {
                targetTransform.localRotation = Quaternion.identity;
                emitter.StartEmission();
            }
        }

        public void NotifyGameOver() {
            canReciveHit = false;
            hideDoor.Trigger();
        }
    }
}
