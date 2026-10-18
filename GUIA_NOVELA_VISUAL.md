# Guía: novela visual sencilla en Unity

**Proyecto:** WGJ2026  
**Unity:** 6000.3.18f1  
**Render:** URP 17.3.0

Esta guía propone una primera versión pequeña: una escena ilustrada, un cuadro de diálogo abajo, un nombre de hablante, su retrato opcional y dos opciones para que el jugador elija. Es un plan de trabajo; todavía no contiene código ni cambios de implementación.

## 1. Alcance de la primera versión

Para que el prototipo sea fácil de terminar, empieza con:

- Una escena de fondo.
- Uno o dos personajes.
- Un diálogo breve de unas 8–12 líneas.
- Una decisión con dos opciones.
- Un resultado diferente para cada opción.
- Un botón para avanzar cada línea.
- Un botón para comenzar de nuevo.

No hace falta incluir todavía inventario, guardado, mapa, animaciones, relaciones, música dinámica ni muchas rutas. Se pueden añadir cuando el diálogo básico ya funcione.

## 2. Estructura de la pantalla

Usa una sola pantalla de juego con estos elementos:

1. **Fondo:** ocupa toda la vista y muestra el lugar donde ocurre la escena.
2. **Personaje (opcional):** retrato o sprite colocado a un lado, delante del fondo.
3. **Cuadro de diálogo:** panel ancho cerca del borde inferior, con un fondo suficientemente opaco para leer el texto.
4. **Nombre:** etiqueta pequeña en la parte superior del cuadro; se puede ocultar en narración.
5. **Texto:** área principal para una o varias frases.
6. **Continuar:** botón o indicador en una esquina del cuadro.
7. **Opciones:** dos botones que aparecen en el lugar del texto cuando llega la decisión.

En Unity, estos elementos pueden organizarse bajo un **Canvas** con componentes de UI. Configura el Canvas para adaptarse a distintas resoluciones y usa anclas para que el cuadro permanezca abajo y los botones no se desplacen fuera de la pantalla. El proyecto ya tiene una escena `Assets/Scenes/SampleScene.unity` que puede servir como punto de partida.

## 3. Diseño del cuadro de diálogo

- Mantén el cuadro dentro de la zona inferior, dejando visible la ilustración importante.
- Usa contraste claro entre el panel y el texto. Si el fondo cambia mucho, un panel oscuro semitransparente suele ser legible.
- Reserva una línea para el nombre y deja el cuerpo del diálogo con espacio amplio.
- Elige un tamaño de letra que se lea a resolución de juego y no llenes el cuadro con párrafos largos.
- Deja espacio alrededor de los botones y evita que queden pegados al borde.
- Comprueba que el texto largo pase a una segunda o tercera línea sin tapar el nombre ni el indicador de avanzar.

## 4. Retrato del hablante

El retrato es opcional. La primera versión puede funcionar solo con fondo, nombre y diálogo.

Si se añade:

- Colócalo a la izquierda o derecha del cuadro, según la composición.
- Cambia el retrato cuando cambia quien habla.
- Ocúltalo durante narración o cuando no corresponda mostrar un personaje.
- Usa imágenes con dimensiones y estilo coherentes.
- Asegúrate de que no cubra el texto, las opciones ni partes importantes del fondo.

Para una prueba temprana, se pueden usar ilustraciones provisionales; conviene dejar claro qué recursos son temporales para reemplazarlos después.

## 5. Cómo escribir la escena

Antes de construir la interfaz, escribe la historia en una tabla o documento con estas columnas:

| Paso | Hablante | Texto | Siguiente paso |
|---|---|---|---|
| 1 | Narrador | Describe brevemente el lugar. | 2 |
| 2 | Personaje A | Presenta la situación. | 3 |
| 3 | Personaje B | Hace una pregunta o plantea un problema. | Decisión |
| 4 | Jugador | Opción A / Opción B. | Ruta A / Ruta B |
| 5A | Personaje A | Reacción a la opción A. | Cierre |
| 5B | Personaje B | Reacción a la opción B. | Cierre |
| 6 | Narrador | Cierra la escena. | Fin |

Escribe frases cortas para el prototipo. Cada opción debe expresar una decisión comprensible y llevar a una reacción distinta, aunque ambas rutas puedan volver a un cierre común.

## 6. Flujo de la interacción

El comportamiento esperado, expresado sin código, es:

1. Al iniciar, se muestra el fondo y la primera línea.
2. El jugador pulsa **Continuar** para ver la línea siguiente.
3. Al llegar a la decisión, se oculta **Continuar** y aparecen las dos opciones.
4. El jugador selecciona una opción.
5. Se muestra la respuesta correspondiente y luego continúa el diálogo.
6. Al terminar, se presenta un cierre y una forma de volver al inicio.

En la guía de implementación futura convendrá definir cada línea y su siguiente paso antes de escribir la lógica. Así las dos opciones no se mezclan con el avance normal del diálogo.

## 7. Pasos de trabajo en Unity

1. **Preparar la escena:** usar `SampleScene` o crear una escena dedicada para la novela visual.
2. **Añadir el fondo:** colocar una imagen de prueba y verificar que cubra la pantalla sin deformarse.
3. **Crear la interfaz:** añadir Canvas, panel inferior, nombre, texto y botón de continuar.
4. **Probar la composición:** revisar la pantalla en la resolución objetivo y una resolución diferente.
5. **Añadir el retrato si se desea:** probarlo junto al cuadro y confirmar que no tape elementos.
6. **Diseñar la decisión:** preparar dos botones con textos breves, claros y fáciles de pulsar.
7. **Definir las rutas:** escribir qué línea sigue a cada opción y dónde se reúnen o terminan.
8. **Conectar la interacción:** en una fase posterior, implementar el avance, la actualización de hablante/retrato y la selección de ruta.
9. **Revisar la experiencia:** hacer una partida completa por cada opción y corregir texto, composición o navegación.

## 8. Lista de comprobación

- ¿Se entiende quién está hablando?
- ¿El texto se lee sobre todos los fondos usados?
- ¿Se puede avanzar sin pulsar accidentalmente una opción?
- ¿Las dos opciones tienen textos distintos y se distinguen bien?
- ¿Cada opción lleva a la respuesta correcta?
- ¿El retrato cambia o se oculta cuando corresponde?
- ¿Todo sigue visible en una resolución distinta?
- ¿Hay una manera clara de terminar o reiniciar la escena?

## 9. Orden recomendado

Primero conviene terminar una escena corta con fondo, cuadro, texto y dos opciones. Después se puede añadir el retrato del hablante. Cuando esa versión sea clara y estable, se pueden valorar varias escenas, más rutas, música, transiciones, guardado y ajustes de accesibilidad.
