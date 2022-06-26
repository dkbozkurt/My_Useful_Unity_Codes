using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableGameFlowController : MonoBehaviour
{
    public enum ObstacleSets
    {
        None,
        ObstacleSet_1,
        ObstacleSet_2
    }

    public enum NumberOfShoots
    {
        NoLimit,
        One,
        Two,
        Three,
    }

    #region InGame Elements

    [SerializeField] private List<GameObject> obstacleGroups;
    [SerializeField] private PlayableGameManager playableGameManager;
    [SerializeField] private BallBehaviour mainPlayerBallBehaviour;
    [SerializeField] private GameObject leftAIPlayer;
    [SerializeField] private GameObject rightAIPlayer;
    
    
    #endregion

    #region LunaPlaygroundFields

    [LunaPlaygroundField("Obstacle set to activate", 0, "Game Flow Settings")] [SerializeField]
    private ObstacleSets obstacleSet;

    [LunaPlaygroundField("Shoot Zoom Camera Active", 1, "Game Flow Settings")] [SerializeField]
    private bool zoomCamActivation;

    [LunaPlaygroundField("Playable number of stages", 0, "Soccer Player Settings")] [SerializeField]
    private NumberOfShoots playableNumberOfShoots;
    
    [LunaPlaygroundField("Left AI Player accuracy rate", 1, "Soccer Player Settings")] 
    [Range(0, 1)] 
    [SerializeField]
    private float leftAIPlayerAccRate; 
    
    [LunaPlaygroundField("Right AI Player accuracy rate", 2, "Soccer Player Settings")] 
    [Range(0, 1)] 
    [SerializeField]
    private float rightAIPlayerAccRate;

    [LunaPlaygroundField("Player's first shoot to post", 3, "Soccer Player Settings")] [SerializeField]
    private bool isFirstShootToPost;

    #endregion
    
    private void Awake()
    {
        DecideObstacleToActivate(obstacleSet);
        DecidePlayableNumberOfShoots(playableNumberOfShoots);
        AssignAIPlayerAccuracyRates(leftAIPlayer, leftAIPlayerAccRate);
        AssignAIPlayerAccuracyRates(rightAIPlayer, rightAIPlayerAccRate);
        mainPlayerBallBehaviour.isZoomCamActive = zoomCamActivation;
        mainPlayerBallBehaviour.isFirstPlayerShootToPost = isFirstShootToPost;
    }

    private void AssignAIPlayerAccuracyRates(GameObject player,float rateValue)
    {
        player.GetComponent<PlayerBehaviour>().accuracyPercentage = rateValue;
    }

    private void DecideObstacleToActivate(ObstacleSets setValue)
    {
        switch (setValue)
        {
            case ObstacleSets.None:
                obstacleGroups[0].SetActive(false);
                obstacleGroups[1].SetActive(false);
                break;
            case ObstacleSets.ObstacleSet_1:
                obstacleGroups[0].SetActive(true);
                obstacleGroups[1].SetActive(false);
                break;
            case ObstacleSets.ObstacleSet_2:
                obstacleGroups[0].SetActive(false);
                obstacleGroups[1].SetActive(true);
                break;
            default:
                Debug.Log("There is no such a obstacle set available!");
                break;
        }
    }

    private void DecidePlayableNumberOfShoots(NumberOfShoots playableShootNumber)
    {
        switch (playableShootNumber)
        {
            case NumberOfShoots.NoLimit:
                playableGameManager.numberOfShootsLimit = 100;
                break;
            
            case NumberOfShoots.One:
                playableGameManager.numberOfShootsLimit = 1;    
                break;
            
            case NumberOfShoots.Two:
                playableGameManager.numberOfShootsLimit = 2;
                break;
            
            case NumberOfShoots.Three:
                playableGameManager.numberOfShootsLimit = 3;
                break;
                
            default:
                Debug.Log("There is no such a limit in number of shoots");
                break;
            
        }
        
    }
}
