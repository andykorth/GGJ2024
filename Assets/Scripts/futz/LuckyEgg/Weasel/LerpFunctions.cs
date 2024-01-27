namespace Weasel
{
public delegate T FnLerp<T>(T a, T b, float f);

public static class LerpFunctions
{
	public static void DefineType<TVal>(FnLerp<TVal> fnLerp)
	{
		LerpFunctions<TVal>.Lerp = fnLerp;
	}
}

public static class LerpFunctions<TVal>
{
	public static FnLerp<TVal> Lerp;
}
}