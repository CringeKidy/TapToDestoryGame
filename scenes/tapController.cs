using Godot;
using System;

public partial class tapController : Node2D
{

	[Export]
	public Camera2D mainCamera;

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventScreenTouch touch && touch.Pressed)
		{
			GD.Print("Touch Pressed at: " + touch.Position);
			CheckTap(touch.Position);
		}
	}

	private void CheckTap(Vector2 screenPosition)
	{
		// Convert to world coordinates
		Vector2 worldPos = GetViewport().CanvasTransform.AffineInverse() * screenPosition;

		var spaceState = GetViewport().World2D.DirectSpaceState;

		var query = new PhysicsPointQueryParameters2D
		{
			Position = worldPos,
			CollideWithBodies = true,
			CollisionMask = uint.MaxValue
		};

		var results = spaceState.IntersectPoint(query);

		foreach (var hit in results)
		{
			Node collider = hit["collider"].As<Node>();
			if (collider is enemyController enemy)
			{
				enemy.TakeDamage();
				GD.Print("Hit enemy: " + collider.Name);
			}
		}
	}

}
