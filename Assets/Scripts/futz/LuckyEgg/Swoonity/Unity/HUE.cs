using Swoonity.CSharp;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Swoonity.Unity
{
public static class HueUtils { }

public static class Hue
{
	public static Color black = new Color(0.00f, 0.00f, 0.00f, 1.00f);
	public static Color blue = new Color(0.00f, 0.00f, 1.00f, 1.00f);
	public static Color blueBold = new Color(0.25f, 0.50f, 1.00f, 1.00f);
	public static Color blueDark = new Color(0.00f, 0.00f, 0.50f, 1.00f);
	public static Color blueLight = new Color(0.50f, 0.50f, 1.00f, 1.00f);
	public static Color bluePastel = new Color(0.32f, 0.42f, 0.72f, 1.00f);
	public static Color brown = new Color(0.56f, 0.31f, 0.05f);
	public static Color cyan = new Color(0.00f, 1.00f, 1.00f, 1.00f);
	public static Color cyanBold = new Color(0.25f, 1.00f, 1.00f, 1.00f);
	public static Color cyanDark = new Color(0.00f, 0.50f, 0.50f, 1.00f);
	public static Color cyanLight = new Color(0.50f, 1.00f, 1.00f, 1.00f);
	public static Color cyanPastel = new Color(0.32f, 0.72f, 0.56f, 1.00f);
	public static Color grey = new Color(0.50f, 0.50f, 0.50f, 1.00f);
	public static Color greyDark = new Color(0.35f, 0.35f, 0.35f, 1.00f);
	public static Color greyLight = new Color(0.72f, 0.72f, 0.72f);
	public static Color green = new Color(0.00f, 1.00f, 0.00f, 1.00f);
	public static Color greenBold = new Color(0.25f, 1.00f, 0.50f, 1.00f);
	public static Color greenDark = new Color(0.00f, 0.50f, 0.00f, 1.00f);
	public static Color greenLight = new Color(0.50f, 1.00f, 0.50f, 1.00f);
	public static Color greenPastel = new Color(0.32f, 0.72f, 0.42f, 1.00f);
	public static Color orange = new Color(1.00f, 0.50f, 0.00f, 1.00f);
	public static Color orangeBold = new Color(1.00f, 0.50f, 0.25f, 1.00f);
	public static Color orangeDark = new Color(0.50f, 0.25f, 0.00f, 1.00f);
	public static Color orangeLight = new Color(1.00f, 0.75f, 0.50f, 1.00f);
	public static Color orangePastel = new Color(0.72f, 0.56f, 0.32f, 1.00f);
	public static Color pinkBold = new Color(1.00f, 0.25f, 0.50f, 1.00f);
	public static Color purple = new Color(1.00f, 0.00f, 1.00f, 1.00f);
	public static Color purpleBold = new Color(0.50f, 0.25f, 1.00f, 1.00f);
	public static Color purpleDark = new Color(0.25f, 0.00f, 0.50f, 1.00f);
	public static Color purpleLight = new Color(1.00f, 0.50f, 1.00f, 1.00f);
	public static Color purplePastel = new Color(0.72f, 0.32f, 0.72f, 1.00f);
	public static Color red = new Color(1.00f, 0.00f, 0.00f, 1.00f);
	public static Color redBold = new Color(1.00f, 0.25f, 0.25f, 1.00f);
	public static Color redDark = new Color(0.50f, 0.00f, 0.00f, 1.00f);
	public static Color redLight = new Color(1.00f, 0.50f, 0.50f, 1.00f);
	public static Color redPastel = new Color(0.72f, 0.32f, 0.32f, 1.00f);
	public static Color silver = new Color(0.80f, 0.80f, 0.80f, 1.00f);
	public static Color violet = new Color(0.50f, 0.00f, 1.00f, 1.00f);
	public static Color white = new Color(1.00f, 1.00f, 1.00f, 1.00f);
	public static Color yellow = new Color(1.00f, 1.00f, 0.00f, 1.00f);
	public static Color yellowBold = new Color(1.00f, 1.00f, 0.25f, 1.00f);
	public static Color yellowDark = new Color(0.50f, 0.50f, 0.00f, 1.00f);
	public static Color yellowLight = new Color(1.00f, 1.00f, 0.50f, 1.00f);
	public static Color yellowPastel = new Color(0.72f, 0.72f, 0.32f, 1.00f);


	public static Color[] IndexColors = {
		blue, green, orange, purple, red, yellow, violet, greenDark, cyan, pinkBold
	};

	public static Color GetColor(int index) => IndexColors[index % IndexColors.Length];
}
}