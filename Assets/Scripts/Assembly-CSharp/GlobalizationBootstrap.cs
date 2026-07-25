using System.Globalization;
using System.Threading;
using UnityEngine;

public static class GlobalizationBootstrap
{
    // I couldn't be bothered to replace every reference with CultureInfo.InvariantCulture, so instead I just force the invariant culture on all threads.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void ForceInvariantCulture()
	{
		CultureInfo invariant = CultureInfo.InvariantCulture;
		CultureInfo.DefaultThreadCurrentCulture = invariant;
		CultureInfo.DefaultThreadCurrentUICulture = invariant;
		Thread.CurrentThread.CurrentCulture = invariant;
		Thread.CurrentThread.CurrentUICulture = invariant;
	}
}
