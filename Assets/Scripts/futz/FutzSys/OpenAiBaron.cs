using Foundational;
using Regent.Workers;
using UnityEngine;
using static UnityEngine.Debug;

namespace FutzSys
{
public class OpenAiBaron : FutzBaron
{
	static GameSysClip GameSys_ => GameSysClip.I;
	static OpenAiClip Ai_ => OpenAiClip.I;

	// TEMP
	public static void Send(string prompt)
	{
		GameSys_.MatcherActivity.OpenAiRequest
		   .SendMatcher(
				new Pk_OpenAiRequest {
					Prompt = prompt,
				}
			);
	}

	[Run.Native(SYS_PK)]
	static void Drain_OpenAi(MatcherActivity matcher)
	{
		matcher.OpenAiResult.Drain(
			static result => {
				Log($"OpenAiResult: {result.Answer.Replace(",", "\n")}");
				Ai_.Answer = result.Answer;
			}
		);
	}
}
}