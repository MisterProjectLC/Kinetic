﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;
using Sigtrap.Relays;

namespace SmartData.SmartFloat.Decorators {
	[DecoratorDescription("Clamps a FloatVariable when its value is set.")]
	public class SmartFloatRangeDecorator : SmartDataDecoratorBase<float> {
		[SerializeField]
		float _min = 0;
		[SerializeField]
		float _max = 0;

		/// <summary>
		/// SmartFloat value cannot be lower than this.
		/// </summary>
		public float min {get {return _min;}}
		/// <summary>
		/// SmartFloat value cannot exceed this.
		/// </summary>
		public float max {get {return _max;}}

		/// <summary>
		/// Raised when value exceeds stated range and is clamped.
		/// Passes (this, clamped value, input value).
		/// </summary>
		public IRelayLink<SmartFloatRangeDecorator, float, float> onRangeClamped {get {return _onRangeClamped;}}
		Relay<SmartFloatRangeDecorator, float, float> _onRangeClamped = new Relay<SmartFloatRangeDecorator, float, float>();

		public override float OnUpdated(float oldValue, float newValue, RestoreMode restoreMode, ref BlockFlags block){
			float result = Mathf.Clamp(newValue, _min, _max);
			if (result != newValue){
				_onRangeClamped.Dispatch(this, result, newValue);
			}
			return result;
		}
	}
}
