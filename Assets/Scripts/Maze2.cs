using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze2 : MonoBehaviour {

	public IntVector2 size;
	public Cell cellPrefab;
	public Enemy enemyPrefab;
	public Key keyPrefab;
	public int nbRooms = 3;

	private Cell[,] cells;
	private List<Cell> keyPositions = new List<Cell>();
	private List<Enemy> enemies = new List<Enemy>();

	//Generate trees for each enemy animation
	public void GenerateTree(){
		foreach (var enemyPosition in keyPositions) {
			Node root = new Node(enemyPosition, null);
			//Add a deterministic path to the door
			IntVector2 nextCoordinates = enemyPosition.coordinates + new IntVector2(-1, -1);
			Node nextNode = new Node (GetCell(nextCoordinates), root);
			root.addChild (nextNode);
			addChildren (nextNode, MazeDirection.West);

			CreateEnemy(root);
		}

		//Add an entrance to the maze
		cells[0,0].destroyWestWall();

	}

	public void addChildren(Node node, MazeDirection comingFrom){
		foreach (var direction in node.value.availableDirections(comingFrom)) {
			addAChild (node, direction);
		}
	}

	private void addAChild(Node node, MazeDirection direction){
		IntVector2 newCoordinates = node.value.coordinates + direction.ToIntVector2 ();
		Cell nextCell = GetCell (newCoordinates);
		if (!nextCell.isARoom) {
			Node newNode = new Node (nextCell, node);
			node.addChild (newNode);
			addChildren (newNode, direction);
		} else {
			//Visit the room (deterministic path)
			Node newNode = new Node (nextCell, node);
			node.addChild (newNode);
		}
	}

	//Generate a maze
	public void Generate(){
		cells = new Cell[size.x, size.z];
		List<Cell> activeCells = new List<Cell> ();
		// Init all cells
		for (int x = 0; x < size.x; x++) {
			for (int z = 0; z < size.z; z++) {
				CreateCell (new IntVector2(x,z));
			}
		}
		//Chose the entrance cell
		activeCells.Add(cells[0,0]);
		cells [0, 0].GetComponent<Renderer> ().material.color = Color.blue;

		//Create rooms
		for (int i = 0; i < nbRooms; i++) {
			CreateRoom (i);
		}

		//Wilson's algorithm
		Cell currentCell, startPoint, nextCell = null;

		//Chose a random cell to start a new path (not active)
		currentCell = startPoint = GetRandomNonActiveCell(activeCells);
		while (activeCells.Count < (size.x * size.z - 4*nbRooms -1)) {
			while(true) {
				MazeDirection randomDirection = GetValidRandomDirection(currentCell);
				currentCell.direction = randomDirection;
				IntVector2 nextCoordinates = currentCell.coordinates + randomDirection.ToIntVector2 ();
				nextCell = GetCell (nextCoordinates);

				if (activeCells.Contains (nextCell)) {
					activeCells = CreateNewPath (startPoint, nextCell, activeCells);
					currentCell = startPoint = GetRandomNonActiveCell(activeCells);
					break;
				} else {
					currentCell = nextCell;
				}
			}
		}

	}

	public void OpenExit(){
		cells [size.x - 1, size.z - 1].destroyEastWall ();
	}

	private List<Cell> CreateNewPath(Cell startPoint, Cell lastCell, List<Cell> activeCells){
		while (startPoint != lastCell) {
			activeCells.Add (startPoint);
			startPoint.GetComponent<Renderer> ().material.color = Color.blue;
			Cell nextCell = GetCell (startPoint.coordinates + startPoint.direction.ToIntVector2 ());
			if (startPoint.direction == MazeDirection.East) {
				startPoint.destroyEastWall ();
				nextCell.destroyWestWall ();
			} else if (startPoint.direction == MazeDirection.West) {
				startPoint.destroyWestWall ();
				nextCell.destroyEastWall ();
			} else if (startPoint.direction == MazeDirection.North) {
				startPoint.destroyNorthWall ();
				nextCell.destroySouthWall ();
			} else if (startPoint.direction == MazeDirection.South) {
				startPoint.destroySouthWall ();
				nextCell.destroyNorthWall ();
			}
			startPoint = nextCell;
		}
		return activeCells;
	}

	private Cell GetRandomNonActiveCell(List<Cell> activeCells){
		List<Cell> nonActiveCells = new List<Cell> ();
		for (int x = 0; x < size.x; x++) {
			for (int z = 0; z < size.z; z++) {
				if (!activeCells.Contains(cells[x, z])) {
					nonActiveCells.Add(cells[x, z]);
				}
			}
		}

		Cell result;
		do {
			result = nonActiveCells[Random.Range (0, nonActiveCells.Count)];
		} while(result.isARoom);
		return result;
	}

	private MazeDirection GetValidRandomDirection(Cell currentCell){
		MazeDirection direction;
		IntVector2 nextCoordinates;

		do {
			do {
				direction = MazeDirections.RandomValue;
				nextCoordinates = currentCell.coordinates + direction.ToIntVector2 ();
			} while(!ContainsCoordinates (nextCoordinates));
		} while(GetCell (nextCoordinates).isARoom);
		return direction;
	}

	private Cell CreateCell(IntVector2 coordinates) {
		Cell cell = Instantiate (cellPrefab) as Cell;
		cells [coordinates.x, coordinates.z] = cell;
		cell.coordinates = coordinates;
		cell.name = "Maze Cell " + coordinates.x + "-" + coordinates.z;
		cell.transform.parent = transform;
		cell.transform.localPosition = new Vector3 (coordinates.x, 0f, coordinates.z);
		return cell;
	}

	private void CreateEnemy(Node path){
		Enemy enemy = Instantiate (enemyPrefab) as Enemy;
		enemy.name = "Enemy";
		enemy.transform.parent = transform;
		enemy.transform.localPosition = new Vector3 (path.value.coordinates.x, 1.0f, path.value.coordinates.z);
		enemy.path = path;
		enemies.Add (enemy);
	}

	private void CreateRoom(int index){
		int randomX, randomZ;
		do {
			randomX = Random.Range (2, size.x - 4);
			randomZ = Random.Range (2, size.z - 2);
		}while(AjacentToARoom(new IntVector2(randomX, randomZ)));

		cells [randomX, randomZ].isARoom = true;
		cells [randomX, randomZ].name = index.ToString();
		cells [randomX, randomZ].GetComponent<Renderer> ().material.color = Color.green;
		cells [randomX, randomZ].destroyEastWall ();
		cells [randomX, randomZ].destroyNorthWall ();

		cells [randomX + 1, randomZ].isARoom = true;
		cells [randomX + 1, randomZ].destroyWestWall ();
		cells [randomX + 1, randomZ].destroyNorthWall ();


		cells [randomX, randomZ + 1].isARoom = true;
		cells [randomX, randomZ + 1].destroyEastWall ();
		cells [randomX, randomZ + 1].destroySouthWall ();

		cells [randomX + 1, randomZ + 1].isARoom = true;
		cells [randomX + 1, randomZ + 1].destroyWestWall ();
		cells [randomX + 1, randomZ + 1].destroySouthWall ();
		keyPositions.Add (cells [randomX + 1, randomZ + 1]);

		//Create a key
		Key key = Instantiate (keyPrefab) as Key;
		key.name = "Key";
		key.transform.parent = transform;
		key.transform.localPosition = new Vector3 (randomX + 1, 0.5f, randomZ + 1);

		//Make a door
		if (ContainsCoordinates (new IntVector2 (randomX - 1, randomZ))) {
			cells [randomX - 1, randomZ].destroyEastWall ();
			cells [randomX, randomZ].destroyWestWall ();
		} else {
			cells [randomX + 2, randomZ].destroyWestWall ();
			cells [randomX + 1, randomZ].destroyEastWall ();
		}
	}

	private Cell GetCell (IntVector2 coordinates) {
		return cells [coordinates.x, coordinates.z];
	}

	private IntVector2 RandomCoordinates {
		get {
			return new IntVector2 (Random.Range (0, size.x), Random.Range (0, size.x));
		}
	}

	private bool ContainsCoordinates (IntVector2 coordinates) {
		return coordinates.x >= 0 && coordinates.x < size.x && coordinates.z >= 0 && coordinates.z < size.x;
	}

	private bool AjacentToARoom(IntVector2 room){
		return GetCell (room).isARoom || GetCell (room + new IntVector2(-1, 0)).isARoom
			||  GetCell (room + new IntVector2(2, 1)).isARoom;
	}

	public void MakeEnemyMove(int index){
		if (enemies [index] != null && !enemies [index].isMoving) {
			enemies [index].startMovement ();
		}
	}

}
