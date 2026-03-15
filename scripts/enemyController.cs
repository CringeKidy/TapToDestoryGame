using Godot;
using System;

public partial class enemyController : CharacterBody2D
{
	[Export]
	public float movementSpeed = 30;

	[Export]
	public int health = 10;

	[Export]
	public int goldWorth = 1;


	private Node2D player;

	private Vector2 direction;

	public override void _Ready()
	{
		AddToGroup("enemy");
	}

	public void Init()
	{
		player = GetParent().GetNode<Node2D>("Player");

		if (player == null)
		{
			GD.PrintErr("Player node not found in enemyController.");
			return;
		}
		direction = (player.GlobalPosition - GlobalPosition).Normalized();
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = direction * movementSpeed;
		MoveAndSlide();
	}


	public void TakeDamage()
	{
		health -= GameManager.clickDamage;

		if (health <= 0)
		{
			GameManager.Gold += goldWorth;
			Die();
		}
	}

	private void Die()
	{
		QueueFree();
	}

}
