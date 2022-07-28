using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Flags]
public enum WallState
{
    // 0000 -> NO WALLS
    // 1111 -> LEFT,RIGHT,UP,DOWN
    LEFT = 1, // 0001
    RIGHT = 2, // 0010
    UP = 4, // 0100
    DOWN = 8, // 1000

    VISITED = 128, // 1000 0000 to avoid interference with the previous wall states
}

public struct Cell
{
    public int x;
    public int y;
}

public struct Neighbour
{
    public Cell Cell;
    public WallState SharedWall;
}

public static class MazeGenerator
{
    private static List<Cell> solutionList = new List<Cell>();
    public static int seed = -1;


    /// <summary>
    /// Returns the opposite wall that should be removed from the neighboring cell based on the wall that is to be removed from the current cell
    /// </summary>
    /// <param name="wall">Wall of the current cell that needs to be removed</param>
    /// <returns>Wall of the neighboring cell that needs to be removed</returns>
    private static WallState GetOppositeWall(WallState wall)
    {
        switch (wall)
        {
            case WallState.RIGHT: return WallState.LEFT;
            case WallState.LEFT: return WallState.RIGHT;
            case WallState.UP: return WallState.DOWN;
            case WallState.DOWN: return WallState.UP;
            default: return WallState.LEFT;
        }
    }

    /// <summary>
    /// Applies the Recursive Backtracer algorithm to produce the maze
    /// </summary>
    /// <param name="maze">A 2D array of WallState objects that corresponds to the cells of the maze</param>
    /// <param name="width">The width of the maze</param>
    /// <param name="height">The height of the maze</param>
    /// /// <param name="randomStartEnd">Whether the maze task starts at the top right or at a random cell in the maze</param>
    /// <returns>2D array of Wallstate objects representing the maze after the recursive backtracker algorithm has been initiated</returns>
    private static WallState[,] ApplyRecursiveBacktracker(WallState[,] maze, int width, int height, bool randomStartEnd , int s)
    {
        List<Cell> cellList = new List<Cell>();
        Cell cell = new Cell { x = 0, y = 0 }; // Top right starting point
        if (s == -1)
            seed = (int)DateTime.Now.Ticks;
        else
            seed = s;
        Cell target = new Cell { x = width - 1, y = height - 1 };
        System.Random rng = new System.Random(seed);
        if (randomStartEnd)
        {
            do
            {
                cell = new Cell { x = rng.Next(0, width), y = rng.Next(0, height) }; // Random starting point
                target = new Cell { x = rng.Next(0, width), y = rng.Next(0, height) }; // Random starting point
            } while (cell.x == target.x && cell.y == target.y);
        }
          
        maze[cell.x, cell.y] |= WallState.VISITED;  // 1000 1111
        cellList.Add(cell);
        Cell current = cellList[cellList.Count - 1];
        while (cellList.Count > 0)
        {
            List<Neighbour> neighbours = GetUnvisitedNeighbours(current, maze, width, height); // Get the current cell's unvisited neighbors
            if (neighbours.Count > 0)
            {
                int randIndex = rng.Next(0, neighbours.Count);
                Neighbour randomNeighbour = neighbours[randIndex]; // Get a random neighbor from the list of unvisited neighbors

                var neighborCell = randomNeighbour.Cell;
                maze[current.x, current.y] &= ~randomNeighbour.SharedWall; // Removes the shared wall from the current cell's side
                maze[neighborCell.x, neighborCell.y] &= ~GetOppositeWall(randomNeighbour.SharedWall); // Removes the shared wall from the neighbor's side
                maze[neighborCell.x, neighborCell.y] |= WallState.VISITED; // Make the neighbor visited

                cellList.Add(neighborCell); // Add the neighbor to the solution stack
                current = cellList[cellList.Count - 1]; // Move to the neighbor cell

                // Once the target cell is arrived at, start backtracking in the stack
                if (current.x == target.x && current.y == target.y)
                {
                    FindSolution(cellList); // At this point, the stack holds the solution just with some possible redundancy
                    if (cellList.Count != 1)
                        current = cellList[cellList.Count - 2];
                    cellList.RemoveAt(cellList.Count - 1);
                }

            }
            // If the stack is not empty and there are no unvisited neighbors, backtrack
            else if (cellList.Count != 0)
            {
                if (cellList.Count != 1)
                    current = cellList[cellList.Count - 2];
                cellList.RemoveAt(cellList.Count - 1);

            }
            // At this point, the maze is complete
            else
            {
                // Do whatever upon completion
            }
        }
        return maze;
    }

    /// <summary>
    /// Returns a list of unvisited neighbors of the current cell
    /// </summary>
    /// <param name="c">Current cell</param>
    /// <param name="maze">Current Maze Wallstate 2D array</param>
    /// <param name="width">Width of current maze</param>
    /// <param name="height">Height of current maze</param>
    /// <returns>List of unvisited neighbors of the current cell</returns>
    private static List<Neighbour> GetUnvisitedNeighbours(Cell c, WallState[,] maze, int width, int height)
    {
       List<Neighbour> list = new List<Neighbour>();

        if (c.x > 0) // Checking the LEFT neighbor => the current cell should not be at the far left
        {
            if (!maze[c.x - 1, c.y].HasFlag(WallState.VISITED)) // If the left neighbor is not visited
            {
                list.Add(new Neighbour
                {
                    Cell = new Cell
                    {
                        x = c.x - 1,
                        y = c.y
                    },
                    SharedWall = WallState.LEFT
                });
            }
        }

        if (c.y > 0) // Checking the BELLOW neighbor => the current cell should not be at the far bottom
        {
            if (!maze[c.x, c.y - 1].HasFlag(WallState.VISITED)) // If the bellow neighbor is not visited
            {
                list.Add(new Neighbour
                {
                    Cell = new Cell
                    {
                        x = c.x,
                        y = c.y - 1
                    },
                    SharedWall = WallState.DOWN
                });
            }
        }

        if (c.y < height - 1) // Checking the ABOVE neighbor => the current cell should not be at the far top
        {
            if (!maze[c.x, c.y + 1].HasFlag(WallState.VISITED)) // If the above neighbor is not visited
            {
                list.Add(new Neighbour
                {
                    Cell = new Cell
                    {
                        x = c.x,
                        y = c.y + 1
                    },
                    SharedWall = WallState.UP
                });
            }
        }

        if (c.x < width - 1) // Checking the RIGHT neighbor => the current cell should not be at the far right
        {
            if (!maze[c.x + 1, c.y].HasFlag(WallState.VISITED)) // If the right neighbor is not visited
            {
                list.Add(new Neighbour
                {
                    Cell = new Cell
                    {
                        x = c.x + 1,
                        y = c.y
                    },
                    SharedWall = WallState.RIGHT
                });
            }
        }

        return list;
    }

    /// <summary>
    /// Generates a maze and returns the Wallstate 2D array representing it
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="randomStartEnd">Whether the first cell should be a random cell or the top right cell</param>
    /// <returns>Wallstate 2D array representing maze</returns>
    public static WallState[,] Generate(int width, int height, bool randomStartEnd, int s)
    {
        WallState[,] maze = new WallState[width, height];
        WallState initial = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                maze[i, j] = initial;  // 1111
            }
        }

        return ApplyRecursiveBacktracker(maze, width, height, randomStartEnd, s);
    }


    /// <summary>
    /// Returns solution of the maze
    /// </summary>
    /// <returns>Cell List containing the solution to the maze</returns>
    public static List<Cell> GetSolution()
    {
        return solutionList;
    }

    /// <summary>
    /// Trims the solution by removing redundancies
    /// </summary>
    /// <param name="cellList">The untrimmed solution</param>
    public static void FindSolution(List<Cell> cellList)
    {
        solutionList = cellList.Distinct().ToList();
    }
}