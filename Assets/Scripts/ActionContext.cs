using UnityEngine;
using System.Collections;

public class ActionContext {

	public static bool carryingBody;
	public static bool canDumpBody;
	public static bool canGrabBody;


	public static string getContext() {
		string context = " ";
		if (canDumpBody && carryingBody) {
			context = "SPACE to store body";
		} else if (carryingBody) {
			context = "SPACE to drop body";
		} else if (canGrabBody) {
			context = "SPACE to grab victim";
		}
		return context;
	}
}
