﻿using System;
using UnityEngine;

public class SimpleTargetSpawner : MonoBehaviour
{

	[SerializeField] private Transform[] spawnPoints;
	[SerializeField] private float[] spawnTimes;
	[SerializeField] private Target[] targetPrefabs;
	[SerializeField] private Order order = Order.ROUND_ROBIN;

	private MJRandom random;
	private float elapsedTime;

	private int currentSpawnPointIndex, currentSpawnTimeIndex, currentTargetIndex;

	void Awake()
	{
		random = new MJRandom(1);
	}

	// Use this for initialization
	void Start () {
		if (spawnPoints.Length == 0)
		{
			throw new IndexOutOfRangeException("The must be at least one spawn point");
		}
		
		if (targetPrefabs.Length == 0)
		{
			throw new IndexOutOfRangeException("The must be at least one target prefab");
		}
		
		if (spawnTimes.Length == 0)
		{
			throw new IndexOutOfRangeException("The must be at least one spawn time");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= spawnTimes[currentSpawnTimeIndex])
		{
			elapsedTime = 0f;
			var spawnPoint = GetFreeSpawnPoint();
			if (spawnPoint != null)
			{
				SpawnTarget(currentTargetIndex, spawnPoint);
				currentTargetIndex = GetNextTargetIndex();
				currentSpawnTimeIndex = GetNextSpawnTimeIndex();
				currentSpawnPointIndex = GetNextSpawnPointIndex();
			}
		}
	}

	private int GetNextSpawnPointIndex()
	{
		if (order == Order.ROUND_ROBIN)
		{
			return (currentSpawnPointIndex + 1) % spawnPoints.Length;
		}
		return random.Next(0, spawnPoints.Length);
	}

	private int GetNextSpawnTimeIndex()
	{
		if (order == Order.ROUND_ROBIN)
		{
			return (currentSpawnTimeIndex + 1) % spawnTimes.Length;
		}
		return random.Next(0, spawnTimes.Length);
	}

	private int GetNextTargetIndex()
	{
		if (order == Order.ROUND_ROBIN)
		{
			return (currentTargetIndex + 1) % targetPrefabs.Length;
		}
		return random.Next(0, targetPrefabs.Length);
	}

	private Transform GetFreeSpawnPoint()
	{
		foreach (var spawnPoint in spawnPoints)
		{
			if (spawnPoint.childCount == 0)
			{
				return spawnPoint;
			}
		}
		return null;
	}

	private Target SpawnTarget(int targetIndex, Transform spawnPoint)
	{
		return Instantiate(targetPrefabs[targetIndex], spawnPoint);
	}
	
}