using System;

namespace AssemblyCSharp
{
	public class CharController_Bak
	{
		/*
		private float mcCircleColliderRadius;
		private float sideLineCastRange;
		public LayerMask groundMask;

		void Awake()
		{
			mcCircleColliderRadius = GetComponent<CircleCollider2D> ().radius;
			sideLineCastRange = mcCircleColliderRadius * Mathf.Sqrt (8);

		}



		void FixedUpdate()
		{
			StateUpdate ();


		}







		void StateUpdate()
		{
			RaycastHit2D bottomLeft = Physics2D.Raycast (transform.position + Vector3.left * mcCircleColliderRadius, Vector3.down, sideLineCastRange, groundMask);
			RaycastHit2D bottomRight = Physics2D.Raycast (transform.position + Vector3.right * mcCircleColliderRadius, Vector3.down, sideLineCastRange, groundMask);
			RaycastHit2D rightBottom = Physics2D.Raycast (transform.position + Vector3.down * mcCircleColliderRadius, Vector3.right, sideLineCastRange, groundMask);
			RaycastHit2D rightTop = Physics2D.Raycast (transform.position + Vector3.up * mcCircleColliderRadius, Vector3.right, sideLineCastRange, groundMask);
			RaycastHit2D leftBottom = Physics2D.Raycast (transform.position + Vector3.down * mcCircleColliderRadius, Vector3.left, sideLineCastRange, groundMask);
			RaycastHit2D leftTop = Physics2D.Raycast (transform.position + Vector3.up * mcCircleColliderRadius, Vector3.left, sideLineCastRange, groundMask);
			RaycastHit2D topLeft = Physics2D.Raycast (transform.position + Vector3.left * mcCircleColliderRadius, Vector3.up, sideLineCastRange, groundMask);
			RaycastHit2D topRight = Physics2D.Raycast(transform.position + Vector3.right*mcCircleColliderRadius, Vector3.up, sideLineCastRange, groundMask);


			if (bottomLeft) {
				Debug.DrawLine(transform.position + Vector3.left * mcCircleColliderRadius, bottomLeft.point);
			}
			if (bottomRight) {
				Debug.DrawLine(transform.position + Vector3.right * mcCircleColliderRadius, bottomRight.point);
			}
			if (rightBottom) {
				Debug.DrawLine(transform.position + Vector3.down * mcCircleColliderRadius, rightBottom.point);
			}
			if (rightTop) {
				Debug.DrawLine(transform.position + Vector3.up * mcCircleColliderRadius, rightTop.point);
			}
			if (leftBottom) {
				Debug.DrawLine(transform.position + Vector3.down * mcCircleColliderRadius, leftBottom.point);
			}
			if (leftTop) {
				Debug.DrawLine(transform.position + Vector3.up * mcCircleColliderRadius, leftTop.point);
			}
			if (topLeft) {
				Debug.DrawLine(transform.position + Vector3.left * mcCircleColliderRadius, topLeft.point);
			}
			if (topRight) {
				Debug.DrawLine(transform.position + Vector3.right*mcCircleColliderRadius, topRight.point);
			}





		}
		*/
	}
}

