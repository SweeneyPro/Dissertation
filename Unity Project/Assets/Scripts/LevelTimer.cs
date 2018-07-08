using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelTimer : Level {

	public int timeInSeconds;
	public int targetScore;

	[SerializeField]
	private Slider timeBar;

	[SerializeField]
	private Text CoinAmount;

	private float timer;
	private bool timeOut = false;

	// Use this for initialization
	void Start () {
		type = LevelType.TIMER;

		hud.SetLevelType (type);
		hud.SetScore (currentScore);
		hud.SetTarget (targetScore);
		hud.SetRemaining (string.Format ("{0}:{1:00}", timeInSeconds / 60, timeInSeconds % 60));


	}

	// Update is called once per frame
	void Update () {
		if (!timeOut) {
			timer += Time.deltaTime;
			hud.SetRemaining (string.Format ("{0}:{1:00}", (int)Mathf.Max((timeInSeconds - timer) / 60, 0), (int)Mathf.Max((timeInSeconds - timer) % 60, 0)));
			CoinAmount.text = CurrencySystem.CoinAmount.ToString();
			timeBar.value = 90-timer;
			if (timeInSeconds - timer <= 0) {
				if (currentScore >= targetScore) {
					GameWin ();
				} else {
					GameLose ();
				}

				timeOut = true;
			}
		}

		if (Input.GetKeyDown (KeyCode.K))
			IncreaseTimer (10);
	}

	public void IncreaseTimer(float Time)
	{
		timer -= Time;
	}
}
