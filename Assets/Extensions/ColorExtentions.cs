using UnityEngine;
using System.Collections;

/// <summary>
/// Contains additional functions that extend Unity's Color class.
/// </summary>
static public class ColorExtentions
{
	/// <summary>
	/// Returns a copy of the Color with the given alpha.
	/// </summary>
	/// <param name="target">The Color to copy.</param>
	/// <param name="alpha">The new alpha value.</param>
	/// <returns>A copy of the Color with the given alpha.</returns>
	static public Color WithAlpha(this Color target, float alpha)
	{
		return new Color(target.r, target.g, target.b, alpha);
	}

	/// <summary>
	/// Returns a copy of the Color with an alpha value that has been multiplied by the given alphaFactor.
	/// </summary>
	/// <param name="target">The Color to copy.</param>
	/// <param name="alphaFactor">Multiply the alpha by this factor.</param>
	/// <returns>A copy of the Color with the new alpha.</returns>
	static public Color WithFactoredAlpha(this Color target, float alphaFactor)
	{
		return new Color(target.r, target.g, target.b, target.a * alphaFactor);
	}

	/// <summary>
	/// Returns a copy of the Color with all components multiplied by a given factor.
	/// </summary>
	/// <param name="c">The Color to copy.</param>
	/// <param name="a">Multiply each component by this value.</param>
	/// <returns>A copy of the Color with the new component values.</returns>
	static public Color WithPremultipliedAlpha(this Color c, float a) => new Color(c.r*a, c.g*a, c.b*a, c.a*a);

	static public Color MoveTowards(Color current, Color target, float maxDelta)
	{
		Vector4 vCurrent = new Vector4(current.r, current.g, current.b, current.a);
		Vector4 vTarget = new Vector4(target.r, target.g, target.b, target.a);

		Vector4 vValue = Vector4.MoveTowards(vCurrent, vTarget, maxDelta);

		return new Color(vValue.x, vValue.y, vValue.z, vValue.w);

	}

	/// <summary>
	/// Returns a Vector3 where the components are the Color converted to HSV.  (This does not maintain the alpha value.)
	/// Adapted from:  http://www.cs.rit.edu/~ncs/color/t_convert.html
	/// </summary>
	/// <param name="target">The Color to convert to HSV.</param>
	/// <returns>A Vector3 where the components are the Color converted to HSV.</returns>
	static public Vector3 ToHSV(this Color target)
	{
		float h, s, v;
		float min, max, delta;

		min = Mathf.Min( target.r, target.g, target.b );
		max = Mathf.Max( target.r, target.g, target.b );
		delta = max - min;

		// calculate V
		v = max;

		// calculate S
		if( max != 0 )
			s = delta / max;
		// s = 0, v is undefined
		else
			return new Vector3(-1f, 0f, v);

		// calculate H
		if( target.r == max )
			h = ( target.g - target.b ) / delta;		// between yellow & magenta
		else if( target.g == max )
			h = 2 + ( target.b - target.r ) / delta;	// between cyan & yellow
		else
			h = 4 + ( target.r - target.g ) / delta;	// between magenta & cyan
		h *= 60;				// degrees
		if( h < 0 )
			h += 360;

		return new Vector3(h, s, v);
	}

	// adapted from:  http://www.cs.rit.edu/~ncs/color/t_convert.html
	/// <summary>
	/// Returns a Color that has been converted from the given HSV value.
	/// </summary>
	/// <param name="target">Unused.  (Here just to make it appear as an extension of the Color class.)</param>
	/// <param name="hsv">A Vector3 where the components are the Color converted to HSV.</param>
	/// <returns>A Color converted from the given HSV.</returns>
	static public Color FromHSV(this Color target, Vector3 hsv)
	{
		float r, g, b;
		float h = hsv[0];
		float s = hsv[1];
		float v = hsv[2];

		int i;
		float f, p, q, t;

		// achromatic (grey)
		if( s == 0 )
			return target = new Color(v, v, v);

		h /= 60;			// sector 0 to 5
		i = (int)Mathf.Floor( h );
		f = h - i;			// factorial part of h
		p = v * ( 1 - s );
		q = v * ( 1 - s * f );
		t = v * ( 1 - s * ( 1 - f ) );

		switch( i ) {
		case 0:
			r = v;
			g = t;
			b = p;
			break;

		case 1:
			r = q;
			g = v;
			b = p;
			break;

		case 2:
			r = p;
			g = v;
			b = t;
			break;

		case 3:
			r = p;
			g = q;
			b = v;
			break;

		case 4:
			r = t;
			g = p;
			b = v;
			break;

		default:		// case 5:
			r = v;
			g = p;
			b = q;
			break;
		}

		return target = new Color(r, g, b);
	}
}
