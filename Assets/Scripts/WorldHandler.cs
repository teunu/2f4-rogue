using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldHandler : MonoBehaviour {

    Vector2Int chunk_scale = new Vector2Int(16, 16);

    [SerializeField] Tilemap map;
    [SerializeField] Tilemap b_map;
    [SerializeField] TileBase ash;

    [SerializeField] Bounds world_bounds; //Should probably remove this for efficiency, readability is the same for map.localBounds
    [SerializeField] Transform player;

    Dictionary<Vector2Int, TileData> dtiles;


    private void Awake() {
        //Do the neccesary housekeeping and initialisation
        map.CompressBounds();
        world_bounds = map.localBounds;

        dtiles = new Dictionary<Vector2Int, TileData>();
        foreach(Vector3Int pos in map.cellBounds.allPositionsWithin) {
            if (!map.HasTile(pos)) {
                var tile = new TileData
                {
                    grid_pos = pos,
                    movement_speed = 0,
                    fire_res = 0.95f,
                    health = 2
                };

                dtiles.Add((Vector2Int)pos, tile);
            }
            else
            {
                var tile = new TileData
                {
                    grid_pos = pos,
                    movement_speed = 0,
                    fire_res = 0.3f,
                    health = 5
                };

                dtiles.Add((Vector2Int)pos, tile);
            }
        }

        StartCoroutine(onTileTick());
    }

    IEnumerator onTileTick () {
        yield return new WaitForSeconds(4);
        while (true) {
            foreach (Vector3Int pos in map.cellBounds.allPositionsWithin) {
                TileData dtile;

                dtiles.TryGetValue((Vector2Int)pos, out dtile);

                if (dtile.fire_size > 0) {
                    dtile.fire_size += Random.Range(-7f, 15f)/100;
                } 

                if (dtile.fire_size > dtile.fire_res) {
                    if (Random.Range(1, 20) == 1) {
                        dtile.fire_size = 0f;
                        dtile.fire_res = 1;
                        map.SetTile(pos, null);
                        b_map.SetTile(pos, ash);
                    }
                }

                if (dtile.fire_size > 0.3)
                {
                    if (Random.Range(1, 3) == 1)
                    {

                        Vector2 spread_dir = new Vector2(Random.Range(-1, 3), Random.Range(-1, 3)).normalized;

                        if (dtile.fire_size > 0.7) {
                            spread_dir *= Random.Range(1, 3);
                        }

                        TileData spread_tile;
                        Vector2 spread = new Vector2(pos.x, pos.y) + spread_dir;
                        dtiles.TryGetValue(Vector2Int.FloorToInt(new Vector3(spread.x, spread.y)), out spread_tile);

                        if (dtile == null) continue;
                        else {
                            if (dtile.fire_size >= 0 && Random.Range(0f, 1f) > spread_tile.fire_res) {
                                spread_tile.fire_size += 0.1f;
                            }
                        }
                    }

                   
                }

                dtile.fire_size = Mathf.Clamp(dtile.fire_size, 0, 1);
            }
            //TODO
            yield return new WaitForSeconds(Time.deltaTime * 10);
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mp = map.WorldToCell(mp);
            TileData dtile;
            dtiles.TryGetValue(Vector2Int.FloorToInt(mp), out dtile);
            dtile.fire_size += 0.1f;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(world_bounds.center, 0.5f);

        if(dtiles == null) return;
        foreach (Vector3Int pos in map.cellBounds.allPositionsWithin) {
            
            TileData dtile;
            dtiles.TryGetValue((Vector2Int)pos, out dtile);
            if (dtile == null) continue; 
            if (dtile.fire_size > 0) {
                float g = 10 + (dtile.fire_size * 190);
                Gizmos.color = new Color(1, (g/250), (10 / 250));


                Vector3 p = map.CellToWorld(pos);
                Gizmos.DrawCube(p + Vector3.one / 2, Vector3.one); 
            }
        }

            //UV for chunk position readability, since x, y are reserved for tilemap (unity unit) locations
            int u = 0; // chunk x
        int v = 0; // chunk y
        for (int x = 0; x < world_bounds.size.x; x += chunk_scale.x ) {
            for (int y = 0; y < world_bounds.size.y; y += chunk_scale.y) {
                if ( u == 0 || v == 0 ) {
                    Gizmos.color = Color.red;
                } else { Gizmos.color = Color.white; }
                Gizmos.DrawLine(new Vector2(world_bounds.min.x + x, world_bounds.min.y + y), new Vector2(world_bounds.min.x + x + chunk_scale.x, world_bounds.min.y + y));
                Gizmos.DrawLine(new Vector2(world_bounds.min.x + x, world_bounds.min.y + y), new Vector2(world_bounds.min.x + x, world_bounds.min.y + y + chunk_scale.y));
                v++;
            }
            v = 0;
            u++;
        }
    }
}
