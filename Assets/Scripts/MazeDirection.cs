using UnityEngine;

public enum MazeDirection {
	North,
	East,
	South,
	West
}

public static class MazeDirections {

	public static MazeDirection RandomValue {
		get {
			return (MazeDirection)Random.Range (0, 4);
		}
	}

	public static IntVector2[] vectors = {
		new IntVector2(0, 1), //Vector corresponding to the north
		new IntVector2(1, 0), //Vector corresponding to the East
		new IntVector2(0, -1), //Vector corresponding to the south
		new IntVector2(-1, 0) //Vecor corresponding to the west
	};

	public static IntVector2 ToIntVector2 (this MazeDirection direction) {
		return vectors [(int)direction];
	}
}