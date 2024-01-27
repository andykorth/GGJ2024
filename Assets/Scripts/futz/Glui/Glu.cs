// using System;
// using Lumberjack;
// using Swoonity.Unity;
// using UnityEngine;
//
// namespace Glui
// {
// /// TODO: hook up with Regent Track?
// /// maybe separate types for readonly vs UI controls
// [Serializable]
// public class Glu<TVal> : BaseGlu
// {
// 	[Btn(nameof(PushChange))] // TODO: custom property drawer instead
// 	[SerializeField] TVal _value;
//
// 	public TVal Value {
// 		get => _value;
// 		set {
// 			Debug.Log($"{this} value change: {_value} --> {value}".LgPink());
// 			_value = value;
// 			OnValueChanged?.Invoke(_value);
// 		}
// 	}
//
// 	public event Action<TVal> OnValueChanged;
//
// 	public void PushChange() => Value = _value;
// }
//
// [Serializable]
// public abstract class BaseGlu { }
// }