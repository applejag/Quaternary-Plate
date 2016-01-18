using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ExtensionMethods {
	
	public static class VectorExtension {
		/// <summary>
		/// Converts a vec2 into a vec3. The Z value of the output gets set to 0.
		/// </summary>
		public static Vector3 ToVector3(this Vector2 vec) {
			// Z = 0
			return new Vector3 (vec.x, vec.y);
		}
		
		/// <summary>
		/// Converts a vec3 into a vec2. The Z value of the input gets lost.
		/// </summary>
		public static Vector2 ToVector2(this Vector3 vec) {
			// Z gets lost
			return new Vector2 (vec.x, vec.y);
		}
	}
	
	public static class TransformExitensions {
		/// <summary>
		/// Get the full hierarchy path of a transfrom.
		/// Recursive.
		/// </summary>
		public static string GetPath(this Transform current) {
			// Recursive

			if (current.parent == null) 
				return current.name;

			return current.parent.GetPath () + "/" + current.name;
		}
	}
}