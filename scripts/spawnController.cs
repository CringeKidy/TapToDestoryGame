using Godot;
using System;

public partial class spawnController : Sprite2D
{
	[Export]
	public Label waveCounterLabel;

	[Export]
	public Label touchActivatedLabel;

	[Export]
	private PackedScene enemy = ResourceLoader.Load<PackedScene>("res://scenes/enemy.tscn");
	private PackedScene shopScene = ResourceLoader.Load<PackedScene>("res://scenes/upgradeShop.tscn");

	private storeController upgradeShopInstance;

	private Node2D spawnPointA;
	private Node2D spawnPointB;
	private Vector2 spawnPosition;



	//Boolean to stop multiple calls. 
	private bool waveSpawning = true;
	///Bool for when the user is in the upgrade shop.
	private bool inStore = false;

	[Export]
	private int maxEnemyCount = 5;
	[Export]
	private int waveCounter = 1;

	//Delays between waves or enemies spawning.
	[Export]
	private float enemyDelay = 0.5f;
	[Export]
	private float waveDelay = 1f;

	public override void _Ready()
	{
		//Get the spawnPoints From the tree.
		spawnPointA = GetNode<Node2D>("spawnPointA");
		spawnPointB = GetNode<Node2D>("spawnPointB");
	}


	// public override void _Input(InputEvent @event)
	// {
	// 	if (@event is InputEventScreenTouch touchEvent)
	// 	{
	// 		if (touchEvent.IsPressed())
	// 		{
	// 			touchActivatedLabel.Text = "Touch Pressed";
	// 			touchActivatedLabel.AddThemeColorOverride("font_color", Colors.Green);
	// 		}
	// 		else
	// 		{
	// 			touchActivatedLabel.Text = "Touch UP!";
	// 			touchActivatedLabel.AddThemeColorOverride("font_color", Colors.Red);
	// 		}
	// 	}
	// }


	//Count how many enemies there is, if there is less than 2 spawn the next wave.
	public override void _Process(double delta)
	{
		int enemiesLeft = GetTree().GetNodesInGroup("enemy").Count;
		// waveCounterLabel.Text = "Wave Counter: " + waveCounter;

		if (enemiesLeft == 0 && waveCounter % 2 == 0 && !inStore)
		{
			OpenUpgradeShop();
			inStore = true;
			return;
		}
		if (enemiesLeft == 0 && !waveSpawning)
		{
			waveSpawning = true;
			maxEnemyCount *= 2;
			enemyDelay -= 0.1f;
			GD.Print("Updating wave counter normally.");
			if (!inStore) waveCounter++;
			SpawnWave();
			return;
		}


	}


	private void onInitialSpawnTimeout()
	{
		SpawnWave();
	}

	private async void SpawnWave()
	{
		if (inStore)
		{
			waveCounter++;
			inStore = false;
		}

		for (int i = 0; i < maxEnemyCount; i++)
		{
			float randomPositionX = (float)GD.RandRange(spawnPointA.GlobalPosition.X, spawnPointB.GlobalPosition.X) + GD.RandRange(-10, 10);

			Vector2 randomPosition = new Vector2(randomPositionX, spawnPointA.GlobalPosition.Y);

			enemyController enemyToSpawn = (enemyController)enemy.Instantiate();
			GetParent().AddChild(enemyToSpawn);
			enemyToSpawn.GlobalPosition = randomPosition;
			enemyToSpawn.Init();

			//Timer between enemy spawn
			await ToSignal(GetTree().CreateTimer(enemyDelay), "timeout");
		}

		waveSpawning = false;
	}

	private void OpenUpgradeShop()
	{
		if (upgradeShopInstance == null)
		{
			upgradeShopInstance = (storeController)shopScene.Instantiate();
			GetParent().AddChild(upgradeShopInstance);
			upgradeShopInstance.ShopFinished += OnShopFinished;
		}
		GetTree().Paused = true;
		upgradeShopInstance.updateGoldLabel();
		upgradeShopInstance.Visible = true;
	}

	private void OnShopFinished()
	{
		waveSpawning = true;
		SpawnWave();
		return;
	}
}
