
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MazeColliderGenerator : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float alphaMinima = 0.1f;

    private void Start()
    {
        GenerarColliders();
    }

    private void GenerarColliders()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = spriteRenderer.sprite;

        if (sprite == null)
        {
            Debug.LogError("No hay un Sprite asignado.");
            return;
        }

        Texture2D textura = sprite.texture;
        Rect rect = sprite.rect;

        int ancho = Mathf.RoundToInt(rect.width);
        int alto = Mathf.RoundToInt(rect.height);
        int inicioX = Mathf.RoundToInt(rect.x);
        int inicioY = Mathf.RoundToInt(rect.y);

        Color[] pixeles;

        try
        {
            pixeles = textura.GetPixels(inicioX, inicioY, ancho, alto);
        }
        catch
        {
            Debug.LogError(
                "Activa Read/Write en la importación del PNG."
            );
            return;
        }

        float ppu = sprite.pixelsPerUnit;
        Vector2 pivote = sprite.pivot;

        // Guarda los segmentos de la fila anterior.
        Dictionary<Vector2Int, BoxCollider2D> anterior =
            new Dictionary<Vector2Int, BoxCollider2D>();

        for (int y = 0; y < alto; y++)
        {
            Dictionary<Vector2Int, BoxCollider2D> actual =
                new Dictionary<Vector2Int, BoxCollider2D>();

            int x = 0;

            while (x < ancho)
            {
                // Ignora los píxeles transparentes.
                while (x < ancho &&
                    pixeles[y * ancho + x].a < alphaMinima)
                {
                    x++;
                }

                if (x >= ancho)
                    break;

                int inicio = x;

                // Busca el final de la línea visible.
                while (x < ancho &&
                    pixeles[y * ancho + x].a >= alphaMinima)
                {
                    x++;
                }

                int fin = x;
                Vector2Int clave = new Vector2Int(inicio, fin);

                float centroX = ((inicio + fin) * 0.5f - pivote.x) / ppu;
                float centroY = (y + 0.5f - pivote.y) / ppu;

                BoxCollider2D collider;

                if (anterior.TryGetValue(clave, out collider))
                {
                    // Une este segmento con el de la fila anterior.
                    collider.size = new Vector2(
                        (fin - inicio) / ppu,
                        collider.size.y + 1f / ppu
                    );

                    collider.transform.localPosition +=
                        Vector3.up * (0.5f / ppu);
                }
                else
                {
                    GameObject pared = new GameObject("Collider");
                    pared.transform.SetParent(transform, false);
                    pared.transform.localPosition =
                        new Vector3(centroX, centroY, 0f);

                    collider = pared.AddComponent<BoxCollider2D>();

                    collider.size = new Vector2(
                        (fin - inicio) / ppu,
                        1f / ppu
                    );
                }

                actual[clave] = collider;
            }

            anterior = actual;
        }

        Debug.Log("¡Colliders del laberinto generados!");
    }
}
