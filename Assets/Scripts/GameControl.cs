using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    private Animator dogAnimator, jackalAnimator;
    [SerializeField] private Animator menuAnimator;
    [SerializeField] private Animator bustAnimator;
    [SerializeField] private Transform dogFigure, jackalFigure;
    [SerializeField] private Transform[] dogWayPoints;
    [SerializeField] private Transform[] jackalWayPoints;
    [SerializeField] private Button trowButton;
    private AudioSource audioSource;
    [SerializeField] private AudioClip bonusClip, penaltyClip;
    private int dogFigureWayPointIndex; 
    private int jackalFigureWayPointIndex; 
    private bool whoseTurn; // false - turn dog / true - turn jackal
    public bool isMovingFigure;
    private bool bonusOrPenaltySector;
    private int stepsCount;
    private int dogTotalTurnCount;
    private int jackalTotalTurnCount;
    [SerializeField] private TextMeshProUGUI TotalTurnCountText;
    [SerializeField] private TextMeshProUGUI bestTotalTurnCountText;
    [SerializeField] private TextMeshProUGUI winnerNameText;
    [SerializeField] private TextMeshProUGUI whoseTurnText;
    [SerializeField] private GameObject winnerBoard;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("bestTotalTurnCount"))
        {
            PlayerPrefs.SetInt("bestTotalTurnCount", 0);
        }
        dogAnimator = dogFigure.GetComponent<Animator>();
        jackalAnimator = jackalFigure.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (isMovingFigure)
        {
            if (!whoseTurn)
            {
                MoveFigure(stepsCount, dogFigure,dogWayPoints,ref dogFigureWayPointIndex,dogAnimator);
            }
            else
            {
                MoveFigure(stepsCount, jackalFigure,jackalWayPoints,ref jackalFigureWayPointIndex,jackalAnimator);
            }
        }
    }
    public void SetStepsCount(int steps)
    {
        if (!whoseTurn)
        {
            dogTotalTurnCount ++;
        }
        else
        {
            jackalTotalTurnCount ++;
        }
        stepsCount = steps;                        
    }
    public void MoveFigure(int steps, Transform figureTransform, Transform[] wayPoints, ref int figureWayPointIndex,Animator animator)
    {
        animator.SetBool("move", true);
        var moveToWayPointIndex = figureWayPointIndex + steps;
        if (moveToWayPointIndex < 30)
        {
            figureTransform.position = Vector3.MoveTowards(figureTransform.position, wayPoints[moveToWayPointIndex].position, 2 * Time.deltaTime);

            if (figureTransform.position == wayPoints[moveToWayPointIndex].position)
            {
                if (!bonusOrPenaltySector)
                {
                    switch (moveToWayPointIndex)
                    {
                        case 6:
                            figureWayPointIndex += 13;
                            audioSource.PlayOneShot(bonusClip);
                            bonusOrPenaltySector = true;
                            break;
                        case 19:
                            figureWayPointIndex -= 13;
                            audioSource.PlayOneShot(penaltyClip);
                            bonusOrPenaltySector = true;
                            break;
                        case 7:
                            figureWayPointIndex += 2;
                            audioSource.PlayOneShot(bonusClip);
                            bonusOrPenaltySector = true;
                            break;
                        case 9:
                            figureWayPointIndex -= 2;
                            audioSource.PlayOneShot(penaltyClip);
                            bonusOrPenaltySector = true;
                            break;
                        default:
                            bonusOrPenaltySector = true;
                            break;
                    }
                }
                else
                {
                    trowButton.interactable = true;
                    animator.SetBool("move", false);
                    isMovingFigure = false;
                    figureWayPointIndex = moveToWayPointIndex;
                    if (figureWayPointIndex == 29)
                    {
                        Win();
                    }
                    if ((moveToWayPointIndex) != 14 && (moveToWayPointIndex) != 23)
                    {
                        EndOfTurn();
                    }
                    else
                    {
                        audioSource.PlayOneShot(bonusClip);
                    }
                }
            }
        }
        else
        {
            audioSource.PlayOneShot(penaltyClip);
            bustAnimator.Play("BustAnimation");
            trowButton.interactable = true;
            animator.SetBool("move", false);
            EndOfTurn();
        }
    }
    private void Win()
    {
        winnerBoard.SetActive(true);
        if (!whoseTurn)
        {
            if (PlayerPrefs.GetInt("bestTotalTurnCount") < dogTotalTurnCount)
            {
                PlayerPrefs.SetInt("bestTotalTurnCount", dogTotalTurnCount);
            }
            bestTotalTurnCountText.text = PlayerPrefs.GetInt("bestTotalTurnCount").ToString();
            TotalTurnCountText.text = dogTotalTurnCount.ToString();
            winnerNameText.text = "Dogs";
        }
        else
        {
            if (PlayerPrefs.GetInt("bestTotalTurnCount") < jackalTotalTurnCount)
            {
                PlayerPrefs.SetInt("bestTotalTurnCount", jackalTotalTurnCount);
            }
            bestTotalTurnCountText.text = PlayerPrefs.GetInt("bestTotalTurnCount").ToString();
            TotalTurnCountText.text = jackalTotalTurnCount.ToString();
            winnerNameText.text = "Jackals";
        }


        Debug.Log("WIN");
    }
    private void EndOfTurn()
    {
        if (!whoseTurn)
        {
            whoseTurnText.text = "Shackals Turn";
            whoseTurn = true;
        }
        else
        {
            whoseTurnText.text = "Dogs Turn";
            whoseTurn = false;
        }
        bonusOrPenaltySector = false;
        isMovingFigure = false;
    }
    public void GameMenuAnimationSwitcher()
    {
        if (menuAnimator.GetBool("Switcher"))
        {
            menuAnimator.SetBool("Switcher", false);
        }
        else
        {
            menuAnimator.SetBool("Switcher", true);
        }
    }
}
