using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    Animator animator;

    [SerializeField] float carSpeed, rotationSpeed;

    Vector2 touchPos;

    int travelingDirection, carDirection;

    bool outOfPark;

    private void Start()
    {
        animator = GetComponent<Animator>();

        //Determining the direction of car for further use
        float angle90 = transform.eulerAngles.y - 90f;
        float angle180 = transform.eulerAngles.y - 180f;
        float angle270 = transform.eulerAngles.y - 270f;

        if (Mathf.Abs(angle90) < 1e-3f)
            carDirection = 1;
        else if (Mathf.Abs(angle180) < 1e-3f)
            carDirection = 2;
        else if (Mathf.Abs(angle270) < 1e-3f)
            carDirection = 3;
    }

    void Update()
    {
        //If car does not require to move, return
        if (travelingDirection == 0)
            return;

        transform.Translate(Vector3.forward * carSpeed * Time.deltaTime * travelingDirection);
    }

    private void FixedUpdate()
    {
        //If car does not require to move or out of the parking spot already, return
        if (travelingDirection == 0 || outOfPark)
            return;

        //Check if there is obstacle to stop the car
        if (CheckForObstacle(travelingDirection))
        {
            travelingDirection = 0;
            animator.SetTrigger("Stop");
        }
    }

    bool CheckForObstacle(int direction)
    {
        //Used raycast to check if there is an obstacle in the way
        int layerMask = 1 << 6;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward * direction), out hit, 1.3f, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * direction) * hit.distance, Color.yellow);      
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * direction) * 100, Color.yellow);
            return false;
        }
    }

    private void OnMouseDown()
    {
        //Saved the initial touch position in a variable for swipe mechanic
        touchPos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (travelingDirection != 0 || outOfPark)
            return;

        Vector2 currentPos = Input.mousePosition;

        if (carDirection == 0 || carDirection == 2) 
        {
            if (currentPos.y < touchPos.y - Screen.height / 15)
            {
                Drive(-1, currentPos);
            }
            else if (currentPos.y > touchPos.y + Screen.height / 15)
            {
                Drive(1, currentPos);
            }
        }
        else
        {
            if (currentPos.x < touchPos.x - Screen.width / 20)
            {
                Drive(-1, currentPos);
            }
            else if (currentPos.x > touchPos.x + Screen.width / 20)
            {
                Drive(1, currentPos);
            }
        }
    }

    void Drive(int direction, Vector2 currentPos)
    {
        //Drive command is received, let's equalize the initial touch pos to current touch position to avoid recurring commands
        touchPos = currentPos;

        //If the angle of the car is 180 or 270, swipe-drive direction is reversed
        if (carDirection > 1)
            direction *= -1;

        //Check if there is an obstacle in the way to start moving
        if (!CheckForObstacle(direction))
        {
            travelingDirection = direction;
            animator.SetTrigger("Drive");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If collided with the road colliders, it means the car is out of the jam
        if (other.CompareTag("Director"))
        {
            outOfPark = true;

            //Cars will turn right when out of the parking spot, however if car goes out reversed, then it will turn left first to go straight
            StartCoroutine(PerformTurn(travelingDirection * -1));
        }
    }

    IEnumerator PerformTurn(int turnState)
    {
        //New direction is added to current direction
        carDirection += turnState;

        float newEuler = carDirection * 90;

        while (true)
        {
            yield return null;

            //Car is turning to desired rotation
            Vector3 to = new Vector3(0, newEuler, 0);
            if (Vector3.Distance(transform.eulerAngles, to) > 0.2f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, newEuler, 0), rotationSpeed * carSpeed * Time.deltaTime * 120f);
            }
            else
            {
                transform.eulerAngles = to;
                break;
            }
        }

        //If car goes out reversed and finished turning left, it will start to go straight
        if(turnState == -1)
            travelingDirection *= -1;

    }

}
