using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarPathGenerator
{
    private List<PathNode> open;
    private List<PathNode> closed;
    private int[,] map;

    /*
    public List<Vector2Int> GeneratePath(Vector2Int start, Vector2Int finish, int[,] map, bool ignoreTerrain)
    {
        if (Vector2Int.Equals(start, finish))
            return new List<Vector2Int>();

        open = new List<PathNode>();         //Lista de nodos por revisar
        closed = new List<PathNode>();       //Lista de nodos ya revisados
        this.map = map;

        open.Add(new PathNode(start));

        PathNode nfinale = new PathNode(finish);

        //Mientras Open tenga elementos.
        while (open.Count > 0)
        {
            SortListByCostArrayList(open);  //Ordenar la lista de F menor a mayor.

            PathNode q = open[0];   //Comprobar el nodo con menor F.
            open.Remove(q);         //Sacamos "q" de la lista Open

            //Buscar los nodos vecinos de "q" y asignar a Q como su predecesor
            List<PathNode> sucesores = GetNeighborNodes(q, ignoreTerrain); 

            //Por cada nodo vecino
            foreach (PathNode sucesor in sucesores)
            {
                if (Vector2Int.Equals(sucesor.getIndexPosition(), nfinale.getIndexPosition()))
                    return ObtainPath(sucesor);     //Encontrado el camino

                sucesor.setG(q.getG() + 1);
                //sucesor.setG(q.getG() + getDistanceBetweenNodePaths(sucesor, q));   //g = Coste para llegar aquí

                int realDistance = Mathf.Abs(sucesor.getIndexPosition().x - nfinale.getIndexPosition().x) +
                    Mathf.Abs(sucesor.getIndexPosition().y - nfinale.getIndexPosition().y);

                sucesor.setH(Vector2Int.Distance(sucesor.getIndexPosition(), nfinale.getIndexPosition()));
                //sucesor.setH(getDistanceBetweenNodePaths(sucesor, nfinale));    //h = Coste aproximado para llegar al nodo final
                sucesor.setF(sucesor.getG() + sucesor.getH());  //f = Coste total de nodo

                if (SkipSuccesor(open, closed, sucesor))
                    continue;

                open.Add(sucesor);
            }

            closed.Add(q);
        }

        return null;    //No se han encontrado caminos válidos
    }
    */

    public List<Room> GeneratePath(Room start, Room finish)
    {

        if (start.GetId() == finish.GetId())
        {
            return new List<Room>();
        }

        open = new List<PathNode>();   //Lista de nodos por revisar
        closed = new List<PathNode>(); //Lista de nodos ya revisados

        open.Add(new PathNode(start, start.GetIndexPosition()));    //Añadimos nodo Start a Open
        PathNode nfinale = new PathNode(finish, finish.GetIndexPosition());

        //Mientras Open tenga elementos.
        while (open.Count > 0)
        {
            SortListByCostArrayList(open);  //Ordenar la lista de F menor a mayor.
            PathNode q = open[0];   //Comprobar el nodo con menor F.
            open.Remove(q); //Sacamos "q" de la lista Open

            List<PathNode> sucesores = q.getNeighborNodes();   //Buscar los nodos vecinos de "q" y actualizar sus atributos Parent

            //Por cada nodo vecino
            foreach (PathNode sucesor in sucesores)
            {
                if (sucesor.getOriginal().Equals(nfinale.getOriginal()))
                    return ObtainPath(sucesor); //Encontrado el camino

                sucesor.setG(q.getG() + GetDistanceBetweenNodePaths(sucesor, q));   //g = Coste para llegar aquí
                sucesor.setH(GetDistanceBetweenNodePaths(sucesor, nfinale));    //h = Coste aproximado para llegar al nodo final
                sucesor.setF(sucesor.getG() + sucesor.getH());  //f = Coste total de nodo

                if (SkipSuccesor(sucesor))
                    continue;

                open.Add(sucesor);
            }

            closed.Add(q);
        }

        return null;    //No se han encontrado caminos válidos
    }



    private List<PathNode> SortListByCostArrayList(List<PathNode> lista)
    {
        int pos = 0;
        PathNode sel;

        while (pos < lista.Count)
        {
            sel = lista[pos];

            for (int i = pos; i < lista.Count; i++)
            {	//Buscar el Nodo con menor coste hacia N. desde pos hasta el final de la lista.
                PathNode num = lista[i];
                if (num.getF() <= sel.getF())
                    sel = num;
            }

            lista.Remove(sel);  //Intercambiamos los nodos de posición
            lista.Insert(pos, sel);

            pos++;
        }

        return lista;
    }
   
    private float GetDistanceBetweenNodePaths(PathNode a, PathNode b)
    {
        return Vector2Int.Distance(a.getOriginal().GetIndexCenter(), b.getOriginal().GetIndexCenter());
    }

    private List<Room> ObtainPath(PathNode result)
    {
        List<Room> path = new List<Room>();

        //Hay que tener en cuenta que se obtiene la ruta desde Final hasta Inicio.
        while (!(result == null))
        {
            path.Insert(0, result.getOriginal());   
            result = result.getParent();
        }

        //Eliminamos el primer objeto porque es el inicio.
        path.RemoveAt(0);

        return path;
    }

    private bool SkipSuccesor(PathNode sucesor)
    {
        //Buscar si hay otro nodo en la misma posicion que Sucesor dentro de la lista Open y con una F menor.
        foreach (PathNode n in open)
        {
            if (Vector2Int.Equals(n.getIndexPosition(), sucesor.getIndexPosition()))
                if (n.getF() < sucesor.getF())
                    return true;
        }
        //Buscar si hay otro nodo en la misma posicion que Sucesor en la lista Closed con menor F.
        foreach (PathNode n in closed)
        {
            if (Vector2Int.Equals(n.getIndexPosition(), sucesor.getIndexPosition()))
                if (n.getF() < sucesor.getF())
                    return true;
        }
        return false;
    }


}
