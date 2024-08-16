using Dan.Main;
using Dan.Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderBoardSpawnBox;
    [SerializeField] private LBPlayerHolder[] scoreFields;

    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button backButton;
    [SerializeField] private int minNameLength = 3;
    [SerializeField] private int maxNameLength = 15;
    [SerializeField] private GameObject loadingImage;

    [SerializeField] private TextMeshProUGUI personalRank;
    [SerializeField] private TextMeshProUGUI personalScore;

    [SerializeField] private GameDataSO ScoreSO;

    [SerializeField] private UIAnimator uIAnimator;

    [SerializeField] private EventSystem eventSystem;

    private void OnEnable()
    {
        if (ScoreSO.PlayedGame)
        {
            uIAnimator.OnAnimateFinished.AddListener(() =>
            {
                OnSubmit();
            });
        }
    }

    private GameObject GetMenusButton() 
    {
        if (ScoreSO.PlayedGame)
        {
            return userNameInput.gameObject;
        }
        else
        {
            return backButton.gameObject;
        }
    }

    private void OnDisable()
    {
        if (ScoreSO.PlayedGame)
        {
            uIAnimator.OnAnimateFinished.RemoveListener(() =>
        {
            OnSubmit();
        });
        }
    }

    private void Start()
    {
        if (!ScoreSO.PlayedGame)
            LoadLeaderBoard();
        else
            LeaderboardCreator.ResetPlayer();


        eventSystem.SetSelectedGameObject(GetMenusButton());
    }

    public void OnSubmit() => SendLeaderBoardEntry(userNameInput.text, ScoreSO.Score);

    public void LoadLeaderBoard()
    {
        uIAnimator.MoveAnimate();
        GetLeaderBoard();
        SetEmptyPersonalScore();
    }

    private void Update() => submitButton.interactable = userNameInput.text.Length > minNameLength && userNameInput.text.Length < maxNameLength;

    #region LEADER BOARD 

    private readonly string publicLeaderBoardKey = "a92682080da1ed9f3b0f0b46e5fd441d0a00d8edffbf4a403f23c0f233f30180";

    public void GetLeaderBoard()
    {
        ToggleLoadingPanel(false);
        foreach (LBPlayerHolder scoreFields in scoreFields)
        {
            scoreFields.ClearText();
        }
        LeaderboardCreator.GetLeaderboard(publicLeaderBoardKey, ((msg) =>
        {
            int loopLength = (msg.Length < scoreFields.Length) ? msg.Length : scoreFields.Length;
            for (int i = 0; i < loopLength; i++)
            {
                scoreFields[i].SetPlayerData(msg[i].Username, score: msg[i].Score, rank: (i + 1));
            }
        }));

        ToggleLoadingPanel(true);
    }

    private void ToggleLoadingPanel(bool state)
    {
        loadingImage.SetActive(!state);
        leaderBoardSpawnBox.SetActive(state);
    }

    public void SendLeaderBoardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderBoardKey, username, score, ((msg) =>
        {
            GetLeaderBoard();
            GetPersonalEntry();
        }));

    }
    private void GetPersonalEntry() => LeaderboardCreator.GetPersonalEntry(publicLeaderBoardKey, OnPersonalEntryLoaded, ErrorCallback);

    private void SetEmptyPersonalScore()
    {
        personalRank.text = "Rank: -";
        personalScore.text = "Score: -";
    }

    private void ErrorCallback(string error) => Debug.LogError(error);


    private void OnPersonalEntryLoaded(Entry entry)
    {
        personalRank.text = $" Rank: {entry.RankSuffix()}";
        personalScore.text = $" Score: {entry.Score}";
    }
    #endregion  
}
