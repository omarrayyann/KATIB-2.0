using UnityEngine;

public class FollowMouse : MonoBehaviour
{
	public bool follow = false;
	Vector3 mousePosition;
	public float moveSpeed = 0.1f;
	Rigidbody2D rb;
	Vector2 position;
	public bool lockOnPath = false;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		position = Vector2.Lerp(transform.position, mousePosition, moveSpeed); // Speed can be adjusted for trailing effect which is why linear interpolation is needed
	}

	private void FixedUpdate()
	{
		if (follow) // Object only follows mouse if in the follow state
		{
			rb.MovePosition(position);
		}
	}


	/// <summary>
	/// When the left mouse button is pressed above the object, the object is switched to the follow state
	/// </summary>
    private void OnMouseDown()
    {
		follow = true;
    }

	/// <summary>
	/// When the left mouse button is released, the object is no longer in the follow state 
	/// </summary>
    private void OnMouseUp()
    {
		follow = false;
    }

	/// <summary>
	/// Any collision must stop the motion
	/// </summary>
	/// <param name="collision">Collision2D representing the collision</param>
	private void OnCollisionEnter2D(Collision2D collision)
    {
		follow = false;        
    }

	/// <summary>
	/// Upon colliding with a trigger, if it is the goal, end the game and destroy this object
	/// </summary>
	/// <param name="other">Collider2D of the objects with which the collision occured</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
			Debug.Log("You Win!");
			follow = false;
			GetComponentInParent<MazeRenderer>().Win();
			this.enabled = false;
        }
    }
}