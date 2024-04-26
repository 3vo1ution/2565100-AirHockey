using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownAndStart : MonoBehaviour
{
    public GameObject countdownPanel;
    public Text countdownText;
    public Button startButton;
    private int countdownValue = 3;
    private bool countdownFinished = false;

    void Start()
    {
        // Set up the start button listener
        startButton.onClick.AddListener(StartGame);

        // Start the countdown coroutine
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        // Loop until countdown finishes
        while (countdownValue > 0)
        {
            countdownText.text = countdownValue.ToString(); // Update the countdown text
            yield return new WaitForSeconds(1f); // Wait for 1 second
            countdownValue--; // Decrement the countdown value
        }

        // Countdown finished
        countdownFinished = true;
        countdownText.text = "Go!"; // Display "Go!" when countdown finishes

        // Deactivate the countdown panel
        countdownPanel.SetActive(false);
    }

    void StartGame()
    {
        // Check if countdown has finished before starting the game
        if (countdownFinished)
        {
            Debug.Log("Game started!"); // Placeholder for starting the game logic
        }
        else
        {
            Debug.Log("Countdown still in progress."); // Placeholder for feedback if the countdown is still running
        }
    }
}
