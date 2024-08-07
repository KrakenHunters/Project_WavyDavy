using Dan.Main;
using Dan.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderBoardSpawnBox;
    [SerializeField] private LBPlayerHolder[] scoreFields;

    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private Button submitButton;
    [SerializeField] private int minNameLength = 3;
    [SerializeField] private int maxNameLength = 15;
    [SerializeField] private GameObject loadingImage;

    [SerializeField] private TextMeshProUGUI personalEntryText;

    [SerializeField] private ScoreSO ScoreSO;

    [SerializeField] private UIAnimator UIAnimator;

    private void OnEnable()
    {
        UIAnimator.OnAnimateFinished.AddListener(() =>
        { 
            OnSubmit();
        });
    }
    private void OnDisable()
    {
        UIAnimator.OnAnimateFinished.RemoveListener(() =>
        {
            OnSubmit();
        });
    }

    private void Start()
    {
        LeaderboardCreator.ResetPlayer();
    }

    public void OnSubmit() => SendLeaderBoardEntry(userNameInput.text, ScoreSO.Score);

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

    private void ErrorCallback(string error) => Debug.LogError(error);

    private void OnPersonalEntryLoaded(Entry entry) => personalEntryText.text = $" Rank: {entry.RankSuffix()} \n Score: {entry.Score}";
    #endregion  
}
