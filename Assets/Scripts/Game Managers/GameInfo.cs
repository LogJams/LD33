using System.Collections;

public static class GameInfo {

	public static int bodyCount = 0;
	public static float silencingModifier = 1;
	public static int nightNumber = 0;
	public static float spawnInterval = 20f;
	public enum LoseCondition{Insane, PoliceCalled, PoliceCaught};
	public static LoseCondition loseCondition;

	public static void resetData(){
		bodyCount = 0;
		silencingModifier = 1;
		nightNumber = 0;
		spawnInterval = 20f;
	}
}
