using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public static Simulation sim;

    [HideInInspector]
    public float nrOfPeople;

    private void Awake() {
        sim = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSimulation(){ //Called when 'Start' button is pressed
        SetPeople();

        //TO-DO: Actual Simulation stuff
    }

    public void StopSimulation(){ //Called when 'Stop' button is pressed
        ResetPeople();
    }

    private void ResetPeople(){
        foreach (var tile in MapGrid_Flex.mg.currentTiles)
        {
            if (tile.tileType == tileType.People)
                tile.SetSpriteFromTileType(tileType.Empty);
        }
    }

    private void SetPeople(){
        nrOfPeople = Settings.st.nrOfPeople_Slider.value;
        float nrOfTilesForPeople = nrOfPeople / 10;
        PlacePeopleOnEmptyTiles(nrOfTilesForPeople);
    }

    private void PlacePeopleOnEmptyTiles(float nrOfPeopleTiles){
        bool done = false;
        int i = 0;
        while (!done){
            int randomEmptyTile = Random.Range(0, Settings.st.currentEmptyTiles.Count);
            Tile currentRandomTile = Settings.st.currentEmptyTiles[randomEmptyTile];

            if (currentRandomTile.tileType != tileType.People){
                currentRandomTile.SetSpriteFromTileType(tileType.People);
                currentRandomTile.SetTileTypeFromCurrentSprite();
                i++;
            }
            
            if (i == nrOfPeopleTiles){
                done = true;
            }
        }
    }
}
