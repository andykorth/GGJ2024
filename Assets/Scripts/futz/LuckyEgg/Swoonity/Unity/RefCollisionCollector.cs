// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Wabi.Locomotion
// {
// public class CollisionCollector : MonoBehaviour
// {
// 	public int InitialSize = 20;
// 	public bool DrawGizmos;
//
// 	public event Action<CollisionContact> OnImpact;
//
// 	[Header("State")]
// 	[SerializeField] int _lastStayPhysicsFrame;
// 	[SerializeField] List<ContactPoint> _contactsRaw;
// 	[SerializeField] List<CollisionContact> _allContacts;
// 	[SerializeField] List<Vector3> _allNormals;
//
// 	[SerializeField] int _lastImpactPhysicsFrame;
// 	[SerializeField] List<CollisionContact> _impacts;
//
// 	void Awake()
// 	{
// 		_contactsRaw = new List<ContactPoint>(InitialSize);
// 		_allContacts = new List<CollisionContact>(InitialSize);
// 		_impacts = new List<CollisionContact>(InitialSize);
// 		_allNormals = new List<Vector3>(InitialSize);
// 	}
//
// 	void OnDrawGizmosSelected()
// 	{
// 		if (!DrawGizmos) return;
//
// 		Gizmos.color = Color.red;
//
// 		foreach (var impact in _impacts) {
// 			Gizmos.DrawSphere(impact.Point, .05f);
// 			Gizmos.DrawRay(impact.Point, impact.Force * .1f);
// 		}
// 	}
//
//
// 	void OnCollisionStay(Collision collision)
// 	{
// 		var currentFrame = PhysicsFrame();
//
// 		if (currentFrame != _lastStayPhysicsFrame) {
// 			_allContacts.Clear();
// 			_allNormals.Clear();
// 			_lastStayPhysicsFrame = currentFrame;
// 		}
//
// 		var count = collision.GetContacts(_contactsRaw);
//
// 		for (var dex = 0; dex < count; dex++) {
// 			var contact = CollisionContact.From(collision, _contactsRaw[dex]);
// 			_allContacts.Add(contact);
// 			_allNormals.Add(contact.Normal);
// 		}
// 	}
//
// 	/// physics events happen at the end of the physics frame, therefore this gets collisions from the PREVIOUS frame
// 	public List<CollisionContact> GetContacts()
// 	{
// 		if (_lastStayPhysicsFrame != PhysicsFrame() - 1) {
// 			_allContacts.Clear();
// 		}
//
// 		return _allContacts;
// 	}
//
// 	public List<Vector3> GetNormals()
// 	{
// 		if (_lastStayPhysicsFrame != PhysicsFrame() - 1) {
// 			_allNormals.Clear();
// 		}
//
// 		return _allNormals;
// 	}
//
//
// 	// TODO: decide whether to keep 'impacts' or not:
//
// 	void OnCollisionEnter(Collision collision)
// 	{
// 		var currentFrame = PhysicsFrame();
//
// 		if (currentFrame != _lastImpactPhysicsFrame) {
// 			_impacts.Clear();
// 			_lastImpactPhysicsFrame = currentFrame;
// 		}
//
// 		var count = collision.GetContacts(_contactsRaw);
//
// 		for (var dex = 0; dex < count; dex++) {
// 			var contact = CollisionContact.From(collision, _contactsRaw[dex]);
// 			_impacts.Add(contact);
// 			OnImpact?.Invoke(contact);
// 		}
// 	}
//
// 	public List<CollisionContact> GetImpacts()
// 	{
// 		if (_lastImpactPhysicsFrame != PhysicsFrame() - 1) {
// 			_impacts.Clear();
// 		}
//
// 		return _impacts;
// 	}
//
// 	static int PhysicsFrame() => Mathf.RoundToInt(Time.fixedTime / Time.fixedDeltaTime);
// }
//
// [Serializable]
// public struct CollisionContact
// {
// 	public Vector3 Force;
// 	public Vector3 Point;
// 	public Vector3 Normal;
// 	public Collider OurCollider;
// 	public Collider OtherCollider;
// 	public Rigidbody OtherRigid;
//
// 	public static CollisionContact From(Collision collision, ContactPoint contact)
// 	{
// 		return new CollisionContact {
// 			Force = collision.relativeVelocity,
// 			Point = contact.point,
// 			Normal = contact.normal,
// 			OurCollider = contact.thisCollider,
// 			OtherCollider = collision.collider,
// 			OtherRigid = collision.rigidbody,
// 		};
// 	}
// }
//
// public static class CollisionCollectorUtils
// {
// 	public static (bool hasAny, CollisionContact contact, float angle) ClosestNormalTo(
// 		this List<CollisionContact> contacts,
// 		Vector3 direction
// 	)
// 	{
// 		var count = contacts.Count;
// 		if (count == 0) return (false, default, default);
//
// 		var contactToBeat = contacts[0];
// 		var angleToBeat = Vector3.Angle(direction, contactToBeat.Normal);
//
// 		for (var i = 1; i < count; i++) {
// 			var contact = contacts[i];
// 			var angle = Vector3.Angle(direction, contact.Normal);
//
// 			if (angle < angleToBeat) {
// 				contactToBeat = contact;
// 				angleToBeat = angle;
// 			}
// 		}
//
// 		return (true, contactToBeat, angleToBeat);
// 	}
//
// 	public static (bool hasAny, CollisionContact contact, float angle) FarthestNormalTo(
// 		this List<CollisionContact> contacts,
// 		Vector3 direction
// 	)
// 	{
// 		var count = contacts.Count;
// 		if (count == 0) return (false, default, default);
//
// 		var contactToBeat = contacts[0];
// 		var angleToBeat = Vector3.Angle(direction, contactToBeat.Normal);
//
// 		for (var i = 1; i < count; i++) {
// 			var contact = contacts[i];
// 			var angle = Vector3.Angle(direction, contact.Normal);
//
// 			if (angle > angleToBeat) {
// 				contactToBeat = contact;
// 				angleToBeat = angle;
// 			}
// 		}
//
// 		return (true, contactToBeat, angleToBeat);
// 	}
// }
// }