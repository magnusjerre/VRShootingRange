using System;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDestroyedListener
{
	public Texture2D ScoreTexture;
	public int MaxScore = 100;

	public bool IsPlainScoreTarget;

	public bool HasSubTargets, CanOutliveSubTargets;
	public int BonusScore, BonusTargetsDestroyed;
	public Target[] subTargets;
	public bool ReceivedBonus { get { return subTargets.Length != 0 && BonusTargetsDestroyed == subTargets.Length;  } }

	private List<IDestroyedListener> destroyedLiisteners;

	private int totalDestructionScore;
	public int TotalDestructionScore { get { return totalDestructionScore;  } }

	private bool isDestroyed;

	private IDestructor destructor;

	void Awake()
	{
		destroyedLiisteners = new List<IDestroyedListener>();
		destructor = GetComponent<IDestructor>();
		if (destructor == null)
		{
			throw new NullReferenceException("Target requires an IDestructor");
		}
	}
	
	// Use this for initialization
	void Start () {
		if (subTargets.Length > 0)
		{
			foreach (var bonusTarget in subTargets)
			{
				bonusTarget.RegisterDestroyedListener(this);
			}
		}
	}

	public int GetScore(float posX, float posY)
	{
		if (IsPlainScoreTarget)
		{
			return MaxScore;
		}
		
		int x = (int) (posX * ScoreTexture.width);
		int y = (int) (posY * ScoreTexture.height);
		return (int)(Mathf.Abs(ScoreTexture.GetPixel(x, y).r - 1)* MaxScore);
	}

	public Hit RegisterHit(float uvX, float uvY)
	{
		if (isDestroyed)
		{
			return Hit.Miss();
		}
		
		totalDestructionScore = GetScore(uvX, uvY) + (ReceivedBonus ? BonusScore : 0);
		for (var i = 0; i < subTargets.Length; i++)
		{
			subTargets[i].NotifyParentTargetDestroyed(this);
		}
		
		for (var i = 0; i < destroyedLiisteners.Count; i++)
		{
			destroyedLiisteners[i].NotifyDestroyed(this);
		}
		
		ShowScore();

		DestroyTarget();
		
		return new Hit(true, totalDestructionScore);
	}

	private void ShowScore()
	{
		var scorePool = GameObject.FindGameObjectWithTag(Tags.SCORE_CANVAS_POOL).GetComponent<Pool>();
		var score = scorePool.Get<ScoreViewer>();
		if (score != null)
		{
			if (ReceivedBonus)
			{
				score.Show(TotalDestructionScore - BonusScore, false, transform.position + Vector3.up * 0.5f);
				var bonusScore = scorePool.Get<ScoreViewer>();
				bonusScore.Show(BonusScore, true, transform.position);
			}
			else
			{
				score.Show(TotalDestructionScore, false, transform.position);
			}
		}
		Debug.Log("score: " + totalDestructionScore);
	}

	public void RegisterDestroyedListener(Target listener)
	{
		destroyedLiisteners.Add(listener);
	}

	public void NotifyDestroyed(Target target)
	{
		BonusTargetsDestroyed++;
		if (ReceivedBonus && !CanOutliveSubTargets)
		{
			DestroyTarget();
		}
	}

	public void NotifyParentTargetDestroyed(Target parent)
	{
		destructor.DestroyAsSubTarget();
	}

	private void DestroyTarget()
	{
		if (isDestroyed)
		{
			return;
		}
		isDestroyed = true;
		destructor.DestroyTarget();
	}
}
