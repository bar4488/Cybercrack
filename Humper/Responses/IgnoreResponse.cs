namespace Humper.Responses
{
	using Base;

	public class IgnoreResponse : ICollisionResponse
	{
		public IgnoreResponse(ICollision collision)
		{
			this.Destination = collision.Goal;
		}

		public RectangleF Destination { get; private set; }
	}
}

