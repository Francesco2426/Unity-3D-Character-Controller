using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public CharacterController cc;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float Sensitivity;
    
    private float speed;
    [SerializeField] private float walk;
    [SerializeField] private float run;
    [SerializeField] private float crouch;

    private Vector3 crouchScale;
    private Vector3 normalScale;

    public bool isMoving;
    public bool isCrouching;
    public bool isRunning;

    private float X;
    private float Y;

    private void Start()
    {
        speed = walk;
        crouchScale = new Vector3(1, .75f, 1);
        normalScale = new Vector3(1, 1, 1);
        cc = GetComponent<CharacterController>();
        cc.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        #region Camera Limitation Calculator
        //Camera limitation variables
        const float MIN_X = 0.0f;
        const float MAX_X = 380.0f;
        const float MIN_Y = -60.0f;
        const float MAX_Y = 70.0f;

        X += Input.GetAxis("Mouse X") * (Sensitivity * Time.deltaTime);
        Y -= Input.GetAxis("Mouse Y") * (Sensitivity * Time.deltaTime);

        if (X < MIN_X)
        {
            X += MAX_X;
        }
        else if (X > MAX_X)
        {
            X -= MAX_X;
        }
        if (Y < MIN_Y)
        {
            Y = MIN_Y;
        }
        else if (Y > MAX_Y)
        {
            Y = MAX_Y;
        }
        #endregion
        transform.rotation = Quaternion.Euler(Y, X, 0.0f);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;

        cc.SimpleMove((forward + right) * speed);
        // Determines if the speed = run or walk
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = run;
            isRunning = true;
        }
        //Crouch
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            isRunning = false;
            speed = crouch;
            player.transform.localScale = crouchScale;
        }
        else
        {
            isRunning = false;
            isCrouching = false;
            speed = walk;
            player.transform.localScale = normalScale;
        }
        // Detects if the player is moving.
        // Useful if you want footstep sounds and or other features in your game.
        if (cc.velocity.sqrMagnitude > 0.0f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }
}
