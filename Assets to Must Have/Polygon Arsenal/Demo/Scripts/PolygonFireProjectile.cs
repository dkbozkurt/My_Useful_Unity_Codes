using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PolygonArsenal
{
    public class PolygonFireProjectile : MonoBehaviour
    {
        RaycastHit hit;
        public GameObject[] projectiles;
        public Transform spawnPosition;
        [HideInInspector]
        public int currentProjectile = 0;
        public float speed = 1000;

        //    MyGUI _GUI;
        PolygonButtonScript selectedProjectileButton;

        void Start()
        {
            selectedProjectileButton = GameObject.Find("Button").GetComponent<PolygonButtonScript>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                nextEffect();
            }

            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                previousEffect();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0)) //On left mouse down-click
            {
                if (!EventSystem.current.IsPointerOverGameObject()) //Checks if the mouse is not over a UI part
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f)) //Finds the point where you click with the mouse
                    {
                        GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject; //Spawns the selected projectile
                        projectile.transform.LookAt(hit.point); //Sets the projectiles rotation to look at the point clicked
                        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed); //Set the speed of the projectile by applying force to the rigidbody
                    }
                }
            }
            Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100, Color.yellow);
        }

        public void nextEffect() //Changes the selected projectile to the next. Used by UI
        {
            if (currentProjectile < projectiles.Length - 1)
                currentProjectile++;
            else
                currentProjectile = 0;
			selectedProjectileButton.getProjectileNames();
        }

        public void previousEffect() //Changes the selected projectile to the previous. Used by UI
        {
            if (currentProjectile > 0)
                currentProjectile--;
            else
                currentProjectile = projectiles.Length - 1;
			selectedProjectileButton.getProjectileNames();
        }

        public void AdjustSpeed(float newSpeed) //Used by UI to set projectile speed
        {
            speed = newSpeed;
        }
    }
}