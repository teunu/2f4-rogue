using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraTargetManager : MonoBehaviour {
    [SerializeField] Vector2Int chunk_scale = new Vector2Int(16, 16);
    [SerializeField] Tilemap map;

    [SerializeField] Transform player;

    private void LateUpdate() {
        Vector2 start = map.localBounds.min;

        Vector2 diff = start - (Vector2)player.position;
        diff = -Vector2Int.FloorToInt(diff/16);

        Vector2 camera_pos = start + (diff * 16 + chunk_scale/2);

        transform.position = start + diff * 16 - chunk_scale / 2;
        transform.position += Vector3.back; //Sidestep to make sure we don't clip
    }
}
