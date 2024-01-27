using System.Threading;
using Cysharp.Threading.Tasks;

namespace Foundational
{
public static class UniTaskUtils
{
	/// Fire and forget (to eliminate missing "await" warning)
	public static void _(this UniTask task) => task.Forget();

	/// Fire and forget (to eliminate missing "await" warning)
	public static void _<T>(this UniTask<T> task) => task.Forget();

	/// Fire and forget (to eliminate missing "await" warning)
	public static void _(this UniTaskVoid task) => task.Forget();

	public static CancellationTokenSource Remake(this CancellationTokenSource source)
	{
		if (source != null) {
			source.Cancel();
			source.Dispose();
		}

		return new CancellationTokenSource();
	}

	public static void CancelDispose(this CancellationTokenSource source)
	{
		if (source != null) {
			source.Cancel();
			source.Dispose();
		}
	}
}
}