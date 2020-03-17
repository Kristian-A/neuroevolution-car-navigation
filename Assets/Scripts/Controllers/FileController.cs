using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FileController : MonoBehaviour {

	public GameObject loadButton; 
	private static string nnDir;
	private static List<GameObject> buttons = new List<GameObject>();

	void Start() {
		nnDir = Application.dataPath + "/";
	}

	public void ExploreFiles() {
		DirectoryInfo directory = new DirectoryInfo(nnDir);
		FileInfo[] files = directory.GetFiles("*.txt", SearchOption.AllDirectories);
	
		Vector3 loadButtonPos = loadButton.transform.position;

		var yOffset = -60;

		foreach (var file in files) {
			var buttonGO = Instantiate(loadButton, loadButtonPos + new Vector3(0, yOffset, 0), Quaternion.identity);
			yOffset -= 45;
			var button = buttonGO.GetComponent<Button>();
			
			button.onClick.AddListener ( delegate { LoadNN(nnDir + file.Name); });
			
			buttonGO.transform.Find("Text").GetComponent<Text>().text = file.Name;
			buttonGO.transform.SetParent(loadButton.transform.parent, true);

			buttons.Add(buttonGO);
		}
	}

	private static string CreateFile() {
		string folder = nnDir + "NN-" + AIController.GenerationCount() + "-";
		int i = 0;

		string path;
		
		do {
			path = folder + (i++) + ".txt";	
		} while (File.Exists(path));

		File.WriteAllText(path, "");
		return path;
	}

	public void SaveBestNN() {
		string path = CreateFile();

		var DNA = AIController.GetBestEntry().GetDna();

		foreach (var genome in DNA) {
			File.AppendAllText(path, genome + "\n");
		}

	}

	public static void LoadNN(string path) {
		DestroyButtons();

		var DNA = new List<double>();

		print(path);

		using (StreamReader sr = new StreamReader(path)) {
			string text = sr.ReadToEnd();
			string[] lines = text.Split('\n');

			foreach (string line in lines) {
				double genome;

				if (double.TryParse(line, out genome)) {
					DNA.Add(genome);
				}
			}
		}

		CarController.Demonstrate(DNA);
	}

	private static void DestroyButtons() {
		foreach (var button in buttons) {
			Destroy(button);
		}

		buttons = new List<GameObject>();
	}
}
