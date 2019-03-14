using Scrips.Spells;
using UnityEngine;

namespace Scrips.Controls
{
    [RequireComponent(typeof(Collider2D))]
    public class LevelMouseControls : MonoBehaviour
    {
        public Level Level;

        public Camera Camera;

        public CircleRenderer SpellCircle;

        public SpellSpawner SpellSpawner;

        private Collider2D _collider;

        private TdTile _lastTile;

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnMouseOver()
        {
            var option = SelectedTowerOption.Option;

            var rayHit = GetRaycastHit();
            if (!rayHit.HasValue) return;
            var hit = rayHit.Value;

            if (option.SelectedTowerPrefab != null)
            {
                var tile = GetTile(hit);

                if (_lastTile != null && _lastTile != tile)
                {
                    _lastTile.StopHighlightTile();
                }

                tile.HighlightTile();
                _lastTile = tile;
            }
            else
            {
                if (_lastTile != null)
                {
                    _lastTile.StopHighlightTile();
                }
                _lastTile = null;
            }

            if (option.SelectedSpell != null)
            {
                SpellCircle.gameObject.SetActive(true);
                SpellCircle.transform.position = new Vector3(hit.point.x, hit.point.y, -1);
                SpellCircle.UpdateCircle(SelectedTowerOption.Option.SelectedSpell.Spell.Range);
            }
            else
            {
                SpellCircle.gameObject.SetActive(false);
            }
        }

        private void OnMouseDown()
        {
            var option = SelectedTowerOption.Option;

            var rayHit = GetRaycastHit();
            if (!rayHit.HasValue) return;
            var hit = rayHit.Value;

            if (option.SelectedTowerPrefab != null)
            {
                var tile = GetTile(hit);
                if (_lastTile != null && _lastTile != tile)
                {
                    _lastTile.StopHighlightTile();
                }

                _lastTile = tile;
                tile.ReadyToBuild();
            }
            else
            {
                if (_lastTile != null)
                {
                    _lastTile.StopHighlightTile();
                    _lastTile = null;
                }
            }

            SpellSpawner.IsReady = option.SelectedSpell != null;
        }

        private void OnMouseUp()
        {
            var option = SelectedTowerOption.Option;

            var rayHit = GetRaycastHit();
            if (!rayHit.HasValue) return;
            var hit = rayHit.Value;

            if (option.SelectedTowerPrefab != null)
            {
                var tile = GetTile(hit);
                if (tile == _lastTile && _lastTile != null) tile.Build(option.SelectedTowerPrefab.Tower);
                else if (_lastTile != null) _lastTile.StopHighlightTile();
            }

            _lastTile = null;

            if (option.SelectedSpell != null)
            {
                if (SpellSpawner.IsReady)
                {
                    SpellSpawner.IsReady = false;
                    SelectedTowerOption.Option.SelectedSpell.TryInstantiateSpell(SpellSpawner);
                }
            }
            else
            {

            }

        }

        private TdTile GetTile(RaycastHit hit)
        {
            float minX = - (Level.Width - 1) / 2f - 0.5f;
            float minY = - (Level.Height - 1) / 2f - 0.5f;

            int actualX = Mathf.FloorToInt(hit.point.x - minX);
            int actualY = Mathf.FloorToInt(hit.point.y - minY);

            return Level[actualX, actualY].GetComponent<TdTile>();
        }

        private RaycastHit? GetRaycastHit()
        {
            RaycastHit hit;
            var mousePosition = Input.mousePosition;
            var rayMouse = Camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit)) return null;

            return hit;
        }
    }
}