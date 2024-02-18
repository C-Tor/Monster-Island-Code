using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSettingsEnforcer : MonoBehaviour
{
	public float baseMapDistance = 2000f;

	void Start() {
		ApplyTerrainSettings();
	}

	void ApplyTerrainSettings() {
		Terrain[] terrainTiles = Terrain.activeTerrains;

		foreach (Terrain terrain in terrainTiles) {
			// Set the base map distance
			terrain.basemapDistance = baseMapDistance;

			// Other settings can be adjusted here if needed
		}
	}

}
