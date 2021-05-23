using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class SceneFader : MonoBehaviour
{
	public Action<int> OnCityTransition;

	public Image img;
	public AnimationCurve curve;

	void Start()
	{
		StartCoroutine(FadeIn());
	}

	public void FadeTo(string scene)
	{
		StartCoroutine(FadeOut());
	}

	public void FadeTo(int scene)
	{
		StartCoroutine(FadeOut());
	}

	public void FadeInAndOut(bool toExpedition)
    {
		//StartCoroutine(FadeInThenOut());
		StartCoroutine(FadeOutThenIn(toExpedition));
		
    }

	public IEnumerator FadeIn()
	{
		float t = 1f;

		while (t > 0f)
		{
			t -= Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color(0f, 0f, 0f, a);
			yield return 0;
		}
	}

	IEnumerator FadeInThenOut()
	{
		float t = 1f;

		while (t > 0f)
		{
			t -= Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color(0f, 0f, 0f, a);
			yield return 0;
		}

		StartCoroutine(FadeOut());

	}

	IEnumerator FadeOutThenIn(bool toExpedition)
	{
		img.raycastTarget = true;

		float t = 0.3f;

		while (t < 1f)
		{
			t += Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color(0f, 0f, 0f, a);
			yield return 0;
		}

		if (toExpedition)
			SceneManager.Instance.SwitchToExpeditionScene();
		else
			SceneManager.Instance.SwitchToCityScene();

		t = 1f;

		while (t > 0f)
		{
			t -= Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color(0f, 0f, 0f, a);
			yield return 0;
		}

		img.raycastTarget = false;
		if (TutorialController.Instance.currentSequenceIndex == 5)
			TutorialController.Instance.DisplaySequence(11);

		OnCityTransition?.Invoke(9);
	}

	public IEnumerator FadeOut()
	{
		float t = 0f;

		while (t < 1f)
		{
			t += Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color(0f, 0f, 0f, a);
			yield return 0;
		}
	}

}
