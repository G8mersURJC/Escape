using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarPathGenerator
{
    private List<PathNode> open;
    private List<PathNode> closed;
    private int[,] map;

    public List<Room> GeneratePath(Room start, Room finish)
    {

        if (start.GetId() == finish.GetId())
        {
            return new List<Room>();
        }

        open = new List<PathNode>();   //Lista de nodos por revisar
        closed = new List<PathNode>(); //Lista de nodos ya revisados

        open.Add(new PathNode(start));    //Añadimos nodo Start a Open
        PathNode nfinale = new PathNode(finish);

        //Mientras Open tenga elementos.
        while (open.Count > 0)
        {
            SortListByCostArrayList(open);      //Ordenar la lista de F menor a mayor.
            PathNode q = open[0];               //Comprobar el nodo con menor F.
            open.Remove(q);                     //Sacamos "q" de la lista Open
            closed.Add(q);

            if (q.getOriginal().GetId() == nfinale.getOriginal().GetId())
                return ObtainPath(q);     //Encontrado el camino

            List<PathNode> sucesores = q.getNeighborNodes();   //Buscar los nodos vecinos de "q" y actualizar sus atributos Parent
            foreach (PathNode sucesor in sucesores)
            {
                if (ChildIsAlreadyInClosedList(sucesor))
                    continue;

                sucesor.setG(q.getG() + GetDistanceBetweenNodePaths(sucesor, q));   //g = Coste para llegar aquí
                sucesor.setH(GetDistanceBetweenNodePaths(sucesor, nfinale));    //h = Coste aproximado para llegar al nodo final
                sucesor.setF(sucesor.getG() + sucesor.getH());  //f = Coste total de nodo

                if (ChildIsAlreadyInOpenList(sucesor))
                    continue;

                open.Add(sucesor);
            }
        }
    

        return null;    //No se han encontrado caminos válidos
    }

    private bool ChildIsAlreadyInClosedList(PathNode sucesor)
    {
        foreach (PathNode n in closed)
        {
            if (n.getOriginal().GetId() == sucesor.getOriginal().GetId())
                return true;
        }

        return false;
    }

    private bool ChildIsAlreadyInOpenList(PathNode sucesor)
    {
        foreach (PathNode n in open)
        {
            if (n.getOriginal().GetId() == sucesor.getOriginal().GetId() &&
                sucesor.getG() > n.getG())
                return true;
        }

        return false;
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
   
    private int GetDistanceBetweenNodePaths(PathNode a, PathNode b)
    {
        //return Vector2Int.Distance(a.getOriginal().GetIndexCenter(), b.getOriginal().GetIndexCenter());
        return (Mathf.Abs(a.getOriginal().GetIndexCenter().x - b.getOriginal().GetIndexCenter().x) +
            Mathf.Abs(a.getOriginal().GetIndexCenter().y - b.getOriginal().GetIndexCenter().y));
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
            if (n.getOriginal().GetId() == sucesor.getOriginal().GetId() && n.getF() < sucesor.getF())
                return true;
        }

        //Buscar si hay otro nodo en la misma posicion que Sucesor en la lista Closed con menor F.
        

        return false;
    }
}