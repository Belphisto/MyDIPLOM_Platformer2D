using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using QuikGraph;

namespace Platformer2D.Generator
{
    public class GeneratorGraph
    { 
        private int countLocation;
        private int countStartVertix;
        public Dictionary<int, LocationType> Rooms;
        public Dictionary<int, List<int>> Transitions;
        public int ChestLocationIndex {get;private set;}
        public GeneratorGraph(int countLocation, int countStartVertix)
        {
            this.countLocation = countLocation;
            this.countStartVertix = countStartVertix;
            GenerateLocations(this.countLocation, this.countStartVertix);
            ChestLocationIndex = UnityEngine.Random.Range(0, Rooms.Count); 
            PrintGraphInfo(Rooms, Transitions);
        }

        /*https://www.nuget.org/packages/QuikGraph#supportedframeworks-body-tab*/
        private void GenerateLocations(int countLocation, int countStartVertix)
        {
            var G2 = BarabasiAlbertGraph(countLocation, countStartVertix);
            Debug.Log("[Generator] [GeneratorGraph] Vertices: " + string.Join(", ", G2.Vertices));
            Debug.Log("[Generator] [GeneratorGraph] Edges: " + string.Join(", ", G2.Edges.Select(e => $"({e.Source}, {e.Target})")));
            Rooms = AssignRoomsToNodes(G2, new List<LocationType> { LocationType.Red, LocationType.Green, LocationType.Blue, LocationType.Sky});
            GenerateRoomsAndTransitions(G2);
        }

        private static UndirectedGraph<int, Edge<int>> BarabasiAlbertGraph(int n, int m)
        {
            var G = new UndirectedGraph<int, Edge<int>>();
            var targetList = new List<int>();
            var random = new System.Random();

            // Создание полного граф с m начальными узлами
            for (int i = 0; i < m; i++)
            {
                G.AddVertex(i);
                for (int j = 0; j < i; j++)
                {
                    G.AddEdge(new Edge<int>(i, j));
                }
                targetList.AddRange(Enumerable.Repeat(i, m));
            }

            // Добавление оставшихся узлов
            for (int i = m; i < n; i++)
            {
                G.AddVertex(i);
                var targets = targetList.OrderBy(x => random.Next()).Take(m).ToList();
                foreach (var target in targets.Take(m))
                {
                    var newEdge = new Edge<int>(i, target);
                    if (!G.ContainsEdge(newEdge))
                    {
                        G.AddEdge(newEdge);
                    }
                }
                targetList.AddRange(targets.Take(m));
                targetList.AddRange(Enumerable.Repeat(i, m));
            }

            return G;
        }

        private static Dictionary<int, LocationType> AssignRoomsToNodes(UndirectedGraph<int, Edge<int>> graph, List<LocationType> roomNames)
        {
            var random = new System.Random();
            var mapping = graph.Vertices.ToDictionary(
                node => node,
                node => roomNames[random.Next(roomNames.Count)]
            );

            return mapping;
        }

        private void GenerateRoomsAndTransitions(UndirectedGraph<int, Edge<int>> graph)
        {
            var transitions = graph.Vertices.ToDictionary(room => room, room => new List<int>());
            foreach (var edge in graph.Edges)
            {
                transitions[edge.Source].Add(edge.Target);
                transitions[edge.Target].Add(edge.Source);
            }
            Transitions = transitions;
        }

        public static void PrintGraphInfo(Dictionary<int, LocationType> rooms, Dictionary<int, List<int>> transitions)
        {
            string textLog = "";
            textLog +="Levels: ";
            foreach (var room in rooms)
            {
                textLog +=$"Level {room.Key}: {room.Value}, ";
            }

            textLog +="\nPrexod:";
            foreach (var (room, transitionRooms) in transitions)
            {
                textLog +=$"\nLevel {room}: {rooms[room]} -> {string.Join(", ", transitionRooms.Select(r => $"Level {r}: {rooms[r]}"))}";
            }
            Debug.Log($" [Generator] [GeneratorGraph] {textLog}");
        }

        public static Dictionary<int, ((int, LocationType), (int, LocationType))> GraphToLocations(Dictionary<int, LocationType> rooms, Dictionary<int, List<int>> transitions)
        {
            var doorToLocations = new Dictionary<int, ((int, LocationType), (int, LocationType))>();
            int doorIndex = 0;

            foreach (var (room, transitionRooms) in transitions)
            {
                foreach (var targetRoom in transitionRooms)
                {
                    doorToLocations[doorIndex++] = ((room, rooms[room]), (targetRoom, rooms[targetRoom]));
                }
            }

            return doorToLocations;
        }

        public static Dictionary<int, (int, int)> GraphToLocations(GeneratorGraph _locationNetwork)
        {
            var doorToLocations = new Dictionary<int, (int, int)>();

            foreach (var edge in _locationNetwork.Transitions)
            {
                int sourceVertex = edge.Key;
                foreach (var targetVertex in edge.Value)
                {
                    doorToLocations[sourceVertex] = (sourceVertex, targetVertex);
                    //Debug.Log($"doorToLocations[{sourceVertex}] = ({sourceVertex}, {targetVertex})");
                }
            }
            foreach (var entry in doorToLocations)
            {
                //Debug.Log($"{{ {entry.Key}, {entry.Value} }}, // Дверь {entry.Key} связывает локации {entry.Value.Item1} и {entry.Value.Item2}");
            }
            return doorToLocations;
        }

    }

}
