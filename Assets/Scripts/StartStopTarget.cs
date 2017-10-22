﻿using UnityEngine;

public class StartStopTarget : MonoBehaviour, IListener, IHittable
{
    private GameController gameController;
    [SerializeField] private Transform actualTarget;
    private ParticleSystem hitParticles;
    public string startTriggerName, hitTriggerName, initTriggerName, waitTriggerName;
    private ITriggerable startTrigger = new EmptyTrigger(), hitTrigger = new EmptyTrigger(), initTrigger = new EmptyTrigger(), waitTrigger = new EmptyTrigger();
    private bool canReceiveHit;
    
    public Texture2D ScoreTexture;
    private Texture2D actualTargetTexture;

    private float rotationAmount = 1f;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameController>();
    }

    void Start()
    {
        if (gameController.IsPlaying)
        {
            FlipRotation();
        }
        var triggers = GetComponents<ITriggerable>();
        for (var i = 0; i < triggers.Length; i++)
        {
            var trigger = triggers[i];
            if (trigger.Name().Equals(startTriggerName))
            {
                startTrigger = trigger;
                trigger.AddListener(this);
            } else if (trigger.Name().Equals(hitTriggerName))
            {
                hitTrigger = trigger;
                trigger.AddListener(this);
            } else if (trigger.Name().Equals(initTriggerName))
            {
                initTrigger = trigger;
                trigger.AddListener(this);
            }
            else if (trigger.Name().Equals(waitTriggerName))
            {
                waitTrigger = trigger;
                trigger.AddListener(this);
            }
        }
        
        initTrigger.Trigger();
    }

    public Hit RegisterHit(RaycastHit hit)
    {
        if (!canReceiveHit)
        {
            return Hit.Miss();
        }

        ShowHitParticles(hit);
        Trigger();
        hitTrigger.Trigger();
        canReceiveHit = false;
        return Hit.Miss();
    }
    
    private void ShowHitParticles(RaycastHit hit) {
        if (hitParticles == null) {
            return;
        }

        int x = (int) (hit.textureCoord.x * actualTargetTexture.width);
        int y = (int) (hit.textureCoord.y * actualTargetTexture.height);
        Color particleHitColor = actualTargetTexture.GetPixel(x, y);
        var colorOverLifetime = hitParticles.colorOverLifetime;
        var instanceGradient = new Gradient();
        instanceGradient.SetKeys(
            new GradientColorKey[]{
                new GradientColorKey(particleHitColor, 0f),
                new GradientColorKey(particleHitColor, 1f)
            }, new GradientAlphaKey[]{
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0.3f, 0.6f),
                new GradientAlphaKey(0f, 0f)
            });
        colorOverLifetime.color = instanceGradient;
        hitParticles.transform.position = hit.point;
        hitParticles.transform.LookAt(hit.point + hit.normal * 10);
        hitParticles.Play();
    }

    public void Trigger()
    {
        gameController.StartButtonClicked();
    }

    private void FlipRotation()
    {
        actualTarget.Rotate(Vector3.up * 180);
    }

    public void Notify(object notifier)
    {
        if (notifier == initTrigger)
        {
            startTrigger.Trigger();
        }
        if (notifier == startTrigger)
        {
            Debug.Log("Start trigger finished");
            canReceiveHit = true;
        }
        else if (notifier == hitTrigger)
        {            
            Debug.Log("Hit trigger finished");
            FlipRotation();
            waitTrigger.Trigger();
        }
        else if (notifier == waitTrigger)
        {
            startTrigger.Trigger();
        }
    }
}