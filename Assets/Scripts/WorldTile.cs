using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keeps track of data for tale
public class TileData {
    public Vector3Int grid_pos { get; set; }

    public float movement_speed { get; set; }
    public float fire_res { get; set; }
    public float fire_size { get; set; }
    public int health { get; set; }


}
