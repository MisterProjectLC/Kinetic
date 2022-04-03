// SMARTTYPE Vector3
// SMARTTEMPLATE SmartSetTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SmartData.SmartVector3.Data;
using SmartData.Abstract;
using SmartData.Interfaces;
using Sigtrap.Relays;

namespace SmartData.SmartVector3.Data {
	/// <summary>
	/// ScriptableObject data set which fires a Relay on data addition/removal.
	/// <summary>
	[CreateAssetMenu(menuName="SmartData/Vector3/Vector3 Set", order=2)]
	public class Vector3Set : SmartSet<Vector3>, ISmartDataSet<Vector3> {
		#if UNITY_EDITOR
		const string VALUETYPE = "Vector3";
		const string DISPLAYTYPE = "Vector3 Set";
		#endif
	}
}

namespace SmartData.SmartVector3 {
	/// <summary>
	/// Read-only access to Vector3Set or List<0>, with built-in UnityEvent.
	/// For write access make a Vector3SetWriter reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class Vector3SetReader : SmartSetRefBase<Vector3, Vector3Set>, ISmartSetRefReader<Vector3> {
		[SerializeField]
		Data.Vector3Var.Vector3Event _onAdd = null;
		[SerializeField]
		Data.Vector3Var.Vector3Event _onRemove = null;
		[SerializeField]
		Data.Vector3Var.Vector3Event _onChange = null;
		
		protected override System.Action<SetEventData<Vector3>> GetUnityEventInvoke(){
			return (d)=>{
				switch (d.operation){
					case SetOperation.ADDED:
						_onAdd.Invoke(d.value);
						break;
					case SetOperation.REMOVED:
						_onRemove.Invoke(d.value);
						break;
					case SetOperation.CHANGED:
						_onChange.Invoke(d.value);
						break;
				}
			};
		}
	}
	/// <summary>
	/// Write access to Vector3Set or List<Vector3>, with built-in UnityEvent.
	/// For read-only access make a Vector3SetRef reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class Vector3SetWriter : SmartSetRefWriterBase<Vector3, Vector3Set>, ISmartSetRefReader<Vector3> {
		[SerializeField]
		Data.Vector3Var.Vector3Event _onAdd = null;
		[SerializeField]
		Data.Vector3Var.Vector3Event _onRemove = null;
		[SerializeField]
		Data.Vector3Var.Vector3Event _onChange = null;
		
		protected override System.Action<SetEventData<Vector3>> GetUnityEventInvoke(){
			return InvokeUnityEvent;
		}
		
		protected sealed override void InvokeUnityEvent(SetEventData<Vector3> d){
			switch (d.operation){
				case SetOperation.ADDED:
					_onAdd.Invoke(d.value);
					break;
				case SetOperation.REMOVED:
					_onRemove.Invoke(d.value);
					break;
				case SetOperation.CHANGED:
					_onChange.Invoke(d.value);
					break;
			}
		}
		
	}
}