using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class IO
{
 
    //Complexidade O(n.m)
    public static Graph GenerateGraph()
    {
        try
        {
            string fileLocation = Application.dataPath + "/Parametros Grafo.txt";
            StreamReader streanReader = new StreamReader(fileLocation);
            int currentLine = 0;


            //L� a quantidade de n�s
            currentLine++;
            string line = streanReader.ReadLine();
            int totalNodes = int.Parse(line);

            //L� a quantidade de n�s
            currentLine++;
            line = streanReader.ReadLine();
            bool directed = line.ToLower() == "d";

            Graph graph = new Graph(totalNodes, directed);

            //L� as arestas
            while (!streanReader.EndOfStream)
            {
                currentLine++;
                //N� Origem - N� Destino - Cor - Distancia
                line = streanReader.ReadLine();

                //Pega os parametros, usando o espa�o como separador
                string[] parameters = line.Split(' ');
                
                if(parameters.Length != 4)
                {
                    throw new Exception("Wrong number of parameters on line: " + currentLine);
                }

                //Procura se o n� de origem j� est� adicionado
                int nodeA = graph.GetNodeIdByName(parameters[0]);

                //Caso n�o esteja, � criado
                if(nodeA == -1)
                {
                    nodeA = graph.AddNode(parameters[0], 1);
                }

                //Procura se o n� de destino j� est� adicionado
                int nodeB = graph.GetNodeIdByName(parameters[1]);

                //Caso n�o esteja, � criado
                if (nodeB == -1)
                {
                    nodeB = graph.AddNode(parameters[1], 1);
                }

                //Pega a cor da aresta
                Color edgeColor = Color.black;
                switch(parameters[2])
                {
                    case "Black":
                        edgeColor = Color.black;
                        break;

                    case "Yellow":
                        edgeColor = Color.yellow;
                        break;

                    case "Blue":
                        edgeColor = Color.blue;
                        break;

                    case "White":
                        edgeColor = Color.white;
                        break;
                }

                //Pega o peso da aresta
                float weight = float.Parse(parameters[3]);

                //Cria a aresta
                GraphEdge graphEdge = new GraphEdge(nodeA, nodeB, edgeColor, weight);

                //Adiciona a aresta no grafo
                if(directed)
                {
                    graph.AddDirectionalEdge(graphEdge);
                }
                else
                {
                    graph.AddBidirectionalGraphEdge(graphEdge);
                }
            }

            streanReader.Close();

            return graph;
        }
        catch(Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public static void PrintAdjacencyMatrix(Graph graph)
    {
        string output = " ";

        GraphEdge[,] matrix = graph.GetAdjacencyMatrix();

        //Adiciona o �ndice de cada aresta
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            output += "   " + graph.GetNodeIdById(i).GetName();
        }

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            output += "\n";
            output += "\n";

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (j == 0)
                {
                    output += graph.GetNodeIdById(i).GetName();
                }

                if(matrix[i, j] != null)
                {
                    output += "   " + 1;
                }
                else
                {
                    output += "   " + 0;
                }
            }
        }

        string fileLocation = Application.dataPath + "/Matriz Adjacencia.txt";

        WriteString(output, fileLocation);
    }

    public static void PrintIncidenceMatrix(Graph graph)
    {
        string output = " ";

        int[,] matrix = graph.GetIncidenceMatrix();

        //Adiciona o �ndice de cada aresta
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            output += "   " + i;
        }

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            output += "\n";
            output += "\n";

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if(j == 0)
                {
                    output += graph.GetNodeIdById(i).GetName(); 
                }

                output += "   " + matrix[i, j];
            }
        }

        string fileLocation = Application.dataPath + "/Matriz Incidencia.txt";

        WriteString(output, fileLocation);
    }

    private static void WriteString(string text, string filePath)
    {
        StreamWriter writer = new StreamWriter(filePath, false);
        writer.WriteLine(text);
        writer.Close();
    }
}
