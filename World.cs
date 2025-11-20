using Godot;

public partial class World : Node2D
{
	private TileMapLayer _tileMapLayer;
	private Camera2D _camera2D;

	public override void _Ready()
	{
		_tileMapLayer = GetNode<TileMapLayer>("TileMapLayer");
		_camera2D = GetNode<Camera2D>("Player/Camera2D");

		var rect = _tileMapLayer.GetUsedRect().Grow(-1);
		var tileSize = _tileMapLayer.TileSet.TileSize;

		_camera2D.LimitTop = rect.Position.Y * tileSize.Y;
		_camera2D.LimitRight = rect.End.X * tileSize.X;
		_camera2D.LimitBottom = rect.End.Y * tileSize.Y;
		_camera2D.LimitLeft = rect.Position.X * tileSize.X;

		_camera2D.ResetSmoothing();
	}
}
