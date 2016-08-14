using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tuple<T1, T2>
{
	public T1 First { get; private set; }
	public T2 Second { get; private set; }
	internal Tuple(T1 first, T2 second)
	{
		First = first;
		Second = second;
	}
}

public class TileObject
{
	public GameObject tile;
	public int x, y;
}
public class GameManager : MonoBehaviour {
	public Transform lostscreen;
	public Transform winscreen;
	public TextMesh scoreLabel;
	public TextMesh scoreBest;
	int score, bestscore, originalbest;
	public GameObject Tile;
	public bool keep = false;
	List<Tuple<int,int>> free = new List<Tuple<int,int>>();
	int [,] grid = new int[4,4];
	int [,] auxgrid = new int[4,4];
	Queue<TileObject> tilegrid = new Queue<TileObject>();
	bool win, lose;
	bool checking = false;
	string str = "";
	Animator anim;
	// Use this for initialization
	void Start () {
		bestscore = PlayerPrefs.GetInt("bestScore", 0);
		originalbest = bestscore;
		win = lose = false;
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				grid[i,j] = 0;
			}
		}
//		grid[0,0] = 8;
//		grid[1,0] = 64;
//		grid[2,0] = 512;
//		grid[3,0] = 256;
//		grid[3,1] = 1024;
//		grid[3,2] = 2048;
//		grid[3,3] = 4096;
//		grid[2,2] = 8192;
		addRandomTile();
		addRandomTile();
		UpdateTiles();
		checking = false;
		score = 0;
		scoreLabel.text = "Score: " + score.ToString();
		scoreBest.text = "Best: " + bestscore.ToString();
	}
	// Update is called once per frame
	void Update () {
		if((!win || keep) && !lose){
			if(Input.GetKeyDown(KeyCode.Space)){
				printStatus();
			}
			if(Input.GetKeyDown(KeyCode.UpArrow)){
				makeMoveUp();
				makeCheck();
			}else if(Input.GetKeyDown(KeyCode.DownArrow)){
				makeMoveDown();
				makeCheck();
			}else if(Input.GetKeyDown(KeyCode.RightArrow)){
				makeMoveRight();
				makeCheck();
			}else if(Input.GetKeyDown(KeyCode.LeftArrow)){
				makeMoveLeft();
				makeCheck();
			}
		}
	}
	void SavePrevGrid(){
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				auxgrid[i,j] = grid[i,j];
			}
		}
	}
	bool checkchanged(){
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				if(auxgrid[i,j] != grid[i,j])
					return true;
			}
		}
		return false;
	}
	void makeCheck(){
		if(score>bestscore)
			bestscore=score;
		scoreLabel.text = "Score: " + score.ToString();
		scoreBest.text = "Best: " + bestscore.ToString();
		checking = true;
		if(!keep)checkWin();
		if(!win || keep){
			if(checkchanged())
				addRandomTile();
			printgrid();
			checkLose();
		}
		UpdateTiles();
		checking = false;
	}
	void makeMoveUp(){
		SavePrevGrid();
		pileUp();
		for(int i = 0; i<3; i++){
			for(int j = 0; j<4; j++){
				if(grid[i,j] == grid[i+1,j]){
					grid[i,j] *= 2;
					grid[i+1,j] = 0;
					if(!checking){
						score += grid[i,j];
						DeleteTile(i+1,j);
						AminateTile(i,j);
					}
					//if(!checking)tilegrid[i,j].GetComponent<TileController>().Fusion();
				}
			}
		}
		pileUp();
	}
	void pileUp(){
		Queue<int> aux = new Queue<int>();
		Queue<TileObject> aux2 = new Queue<TileObject>();
		TileObject t;
		for(int j = 0; j<4; j++){
			aux.Clear();
			aux2.Clear();
			for(int i = 0; i<4; i++){
				if(grid[i,j] != 0){
					aux.Enqueue(grid[i,j]);
					if(!checking){
						t = FindTile(i,j);
						if(t!=null)
							aux2.Enqueue(t);
					}
				}
			}
			for(int i = 0; i<4; i++){
				grid[i,j]=0;
				if(aux.Count>0){
					grid[i,j] = aux.Dequeue();
					if(!checking){
						t = aux2.Dequeue();
						MoveTile(i, j, t);
					}
				}
			}
		}
	}

	void makeMoveDown(){
		SavePrevGrid();
		pileDown();
		for(int i = 3; i>0; i--){
			for(int j = 0; j<4; j++){
				if(grid[i,j] == grid[i-1,j]){
					grid[i,j] *= 2;
					grid[i-1,j] = 0;
					if(!checking){
						score += grid[i,j];
						DeleteTile(i-1,j);
						AminateTile(i,j);
					}
					//if(!checking) tilegrid[i,j].GetComponent<TileController>().Fusion();
				}
			}
		}
		pileDown();
	}
	void pileDown(){
		Queue<int> aux = new Queue<int>();
		Queue<TileObject> aux2 = new Queue<TileObject>();
		TileObject t;
		for(int j = 0; j<4; j++){
			aux.Clear();
			aux2.Clear();
			for(int i = 3; i>=0; i--){
				if(grid[i,j] != 0){
					aux.Enqueue(grid[i,j]);
					if(!checking){
						t = FindTile(i,j);
						if(t!=null)
							aux2.Enqueue(t);
					}
				}
			}
			for(int i = 3; i>=0; i--){
				grid[i,j]=0;
				if(aux.Count>0){
					grid[i,j] = aux.Dequeue();
					if(!checking){
						t = aux2.Dequeue();
						MoveTile(i, j, t);
					}
				}
			}
		}
	}

	void makeMoveRight(){
		SavePrevGrid();
		pileRight();
		for(int j = 3; j>0; j--){
			for(int i = 0; i<4; i++){
				if(grid[i,j] == grid[i,j-1]){
					grid[i,j] *= 2;
					grid[i,j-1] = 0;
					if(!checking){
						score += grid[i,j];
						DeleteTile(i,j-1);
						AminateTile(i,j);
					}
					//if(!checking) tilegrid[i,j].GetComponent<TileController>().Fusion();
				}
			}
		}
		pileRight();
	}
	void pileRight(){
		Queue<int> aux = new Queue<int>();
		Queue<TileObject> aux2 = new Queue<TileObject>();
		TileObject t;
		for(int i = 0; i<4; i++){
			aux.Clear();
			aux2.Clear();
			for(int j = 3; j>=0; j--){
				if(grid[i,j] != 0){
					aux.Enqueue(grid[i,j]);
					if(!checking){
						t = FindTile(i,j);
						if(t!=null)
							aux2.Enqueue(t);
					}
				}
			}
			for(int j = 3; j>=0; j--){
				grid[i,j]=0;
				if(aux.Count>0){
					grid[i,j] = aux.Dequeue();
					if(!checking){
						t = aux2.Dequeue();
						MoveTile(i, j, t);
					}
				}
			}
		}
	}
	void makeMoveLeft(){
		SavePrevGrid();
		pileLeft();
		for(int j = 0; j<3; j++){
			for(int i = 0; i<4; i++){
				if(grid[i,j] == grid[i,j+1]){
					grid[i,j] *= 2;
					grid[i,j+1] = 0;
					if(!checking){
						score += grid[i,j];
						DeleteTile(i,j+1);
						AminateTile(i,j);
					}
					//if(!checking) tilegrid[i,j].GetComponent<TileController>().Fusion();
				}
			}
		}
		pileLeft();
	}
	void pileLeft(){
		Queue<int> aux = new Queue<int>();
		Queue<TileObject> aux2 = new Queue<TileObject>();
		TileObject t;
		for(int i = 0; i<4; i++){
			aux.Clear();
			aux2.Clear();
			for(int j = 0; j<4; j++){
				if(grid[i,j] != 0){
					aux.Enqueue(grid[i,j]);
					if(!checking){
						t = FindTile(i,j);
						if(t!=null)
							aux2.Enqueue(t);
					}
				}
			}
			for(int j = 0; j<4; j++){
				grid[i,j]=0;
				if(aux.Count>0){
					grid[i,j] = aux.Dequeue();
					if(!checking){
						t = aux2.Dequeue();
						MoveTile(i, j, t);
					}
				}
			}
		}
	}
	void checkWin(){
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				if(grid[i,j] == 2048){
					win = true;
					Invoke("goWin", 0.5f);
					Debug.Log("WIN!!");
				}
			}
		}
	}

	void freeCells(){
		free.Clear();
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				if(grid[i,j] == 0)
					free.Add(new Tuple<int, int>(i,j));
			}
		}
	}
	void addRandomTile(){
		freeCells();
		if(free.Count==0){
			return;
		}
		int r = Random.Range(0, free.Count);
		int i, j;
		i = free[r].First;
		j = free[r].Second;
		if(Random.value < 0.9f){
			grid[i,j] = 2;
		}else{
			grid[i,j] = 4;
		}
		AddTile(i,j);
	}

	void checkLose(){
		int[,] auxgrid = new int[4,4];
		bool done = false;
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				auxgrid[i,j] = grid[i,j];
			}
		}
		makeMoveUp();
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				if(auxgrid[i,j] != grid[i,j]){
					done = true;
					goto EndIt;
				}
			}
		}
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				grid[i,j] = auxgrid[i,j];
			}
		}
		makeMoveDown();
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				if(auxgrid[i,j] != grid[i,j]){
					done = true;
					goto EndIt;
				}
			}
		}
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				grid[i,j] = auxgrid[i,j];
			}
		}
		makeMoveRight();
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				if(auxgrid[i,j] != grid[i,j]){
					done = true;
					goto EndIt;
				}
			}
		}
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				grid[i,j] = auxgrid[i,j];
			}
		}
		makeMoveLeft();
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				if(auxgrid[i,j] != grid[i,j]){
					done = true;
					goto EndIt;
				}
			}
		}

	EndIt:
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				grid[i,j] = auxgrid[i,j];
			}
		}
		if(!done){
			lose = true;
			Invoke("goLost", 1.0f);
			Debug.Log("LOST!");
		}
	}

	void printgrid(){
		str = "";
		for(int i = 0; i<4; i++){
			for(int j = 0; j<4; j++){
				str+= "" + grid[i,j] + ",\t\t";
			}
			str+="\n\n";
		}
		Debug.Log(str);
	}
	void UpdateTiles(){
		foreach(TileObject tile in tilegrid){
			int val = grid[tile.x, tile.y];
			tile.tile.GetComponent<TileController>().SetValue(val);
		}
	}
	void AddTile(int i, int j){
		TileObject _tile = new TileObject();
		_tile.tile = (GameObject)Instantiate(Tile);
		_tile.tile.transform.position = new Vector3(j, -i, 0);
		_tile.tile.transform.parent = transform;
		_tile.x = i;
		_tile.y = j;
		tilegrid.Enqueue(_tile);
		Debug.Log("nuevo tile en: " +_tile.x.ToString() + " , " + _tile.y.ToString() + " = " + grid[i,j].ToString());
	}
	void MoveTile(int i, int j, TileObject t){
		if(t!=null){
			t.tile.GetComponent<translateScript>().MoveTo(new Vector3(j, -i, 0));
			t.x=i;
			t.y=j;
			//t.tile.transform.position = new Vector3(j, -i, 0);
			tilegrid.Enqueue(t);
		}
	}
	TileObject FindTile(int i, int j){
		TileObject ret;
		for(int k=0; k<tilegrid.Count; k++){
			ret = tilegrid.Dequeue();
			if(ret.x==i && ret.y==j)
				return ret;
			tilegrid.Enqueue(ret);
		}
		return null;
	}
	void DeleteTile(int i, int j){
		TileObject _tile;
		for(int k=0; k<tilegrid.Count; k++){
			_tile = tilegrid.Dequeue();
			if(_tile.x==i && _tile.y==j){
				Destroy(_tile.tile);
				return;
			}
			tilegrid.Enqueue(_tile);
		}
	}
	void printStatus(){
		foreach(TileObject tile in tilegrid){
			Debug.Log("hay un tile en: " + tile.x.ToString() + ", " + tile.y.ToString() + " = " + grid[tile.x, tile.y].ToString());
		}
	}
	void AminateTile(int i, int j){
		foreach(TileObject tile in tilegrid){
			if(tile.x==i && tile.y==j)
				tile.tile.GetComponent<TileController>().Fusion();
		}
	}
	void goLost(){
		Instantiate(lostscreen);
		if(bestscore>originalbest)
			PlayerPrefs.SetInt("bestScore", bestscore);
	}
	void goWin(){
		Instantiate(winscreen);
		if(bestscore>originalbest)
			PlayerPrefs.SetInt("bestScore", bestscore);
	}
}
