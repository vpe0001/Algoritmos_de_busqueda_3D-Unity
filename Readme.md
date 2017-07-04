# Algoritmos de búsqueda 3D en Unity

Este proyecto consiste la implementación de varios algoritmos de búsqueda de rutas para vehículos autónomos, así como el siguimiento de las rutas generadas desde el inicio hasta la meta.

Está desarrollado usando el motor [Unity](https://unity3d.com/es/)  5.3.6f usando *C#* en [xUbuntu](https://xubuntu.org/)  16.04, pero deberia funcionar en cualquier sistema que soporte esa versión de [Unity](https://unity3d.com/es/) y posteriores.

## Instalación en Unity

Para instalarlo, hay dos maneras:

- Clonando el repositorio o mediante el archivo zip, y abriendo desde [Unity](https://unity3d.com/es/) el proyecto que se encuentra en el directorio *Codigo/Algoritmos_de_busqueda_3D/*.

- Creando un proyecto nuevo en [Unity](https://unity3d.com/es/), y desde *Assets->Import Package*.

Una vez instalado, se puede abrir alguna de las escenas disponibles para probar los algoritmos, y modifcarla como se desee. 

## Ejecución

El proyecto se puede ejecutar desde [Unity](https://unity3d.com/es/), lo que proporciona ventajas ya que se dispone de varias vista para observar el desarrollo de los algoritmos, y el acceso a sus parámetros, las distintas opciones y la modificacion de la escena.

También hay un ejecutable que permite observar en escenas prefijadas a los distintos algooritmo y compararles, que se encuentra en *release*. En el [archivo de zip](https://github.com/vpe0001/Algoritmos_de_busqueda_3D-Unity/releases/download/1.0/release10_mystic-chicken.zip) se encuentran todos los archivos necesarios para las versiones de Linux y Windows.

## Contenidos del proyecto

Los algoritmos implementados han sido:

- ** _A*_ **: [The A* Algorithm](http://theory.stanford.edu/~amitp/GameProgramming/AStarComparison.html#the-a-star-algorithm) El algoritmo *A\** original.

- ** _Theta*_ **: [Theta*: any angle paths](http://aigamedev.com/open/tutorials/theta-star-any-angle-paths/) Es una variación del *A\** que mejora los caminos encontrados.

- ** _A* con vértices_ **: Una versión del *A\** que usa los vértices del [*Nav Mesh*](https://docs.unity3d.com/ScriptReference/AI.NavMesh.html) para mejorar su rendimiento.

- ** _Hybrid A*_ **: [Practical Search Techniques in Path Planning for Autonomous Driving](https://ai.stanford.edu/~ddolgov/papers/dolgov_gpp_stair08.pdf) Es una evolución del *A\** que tiene en cuenta las limitaciones de los vehículos no holonómicos, calculando los estados continuos a los que se pueden desplazar.

Para el desplazamiento autónomo del vehículo se ha utilizado un [*PID Controller*](https://en.wikipedia.org/wiki/PID_controller).

Para el suavizado de las rutas se han usado [*curvas Bézier*](http://devmag.org.za/2011/04/05/bzier-curves-a-tutorial/) y el [*descenso gradiente*](https://en.wikipedia.org/wiki/Gradient_descent).


# ![Licencia GPLv3](gplv3-127x51.png  "Licencia GPLv3")