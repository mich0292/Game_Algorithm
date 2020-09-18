using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project
{
    public class AStar
    {
        public static IDictionary<Vector2, bool> walkablePosition = new Dictionary<Vector2, bool>();
        public static Vector2 lastPoint;
        public static int leftBound;
        public static int rightBound;
        public static int topBound;
        public static int bottomBound;

        public static void Initialize(Vector2 start, Vector2 goal)
        {
            int constraint = 0; //all enemy texture maximum height width / 2
            if (start.X > goal.X)
            {
                leftBound = (int)goal.X - constraint;
                rightBound = (int)start.X + constraint;
            }
            else
            {
                leftBound = (int)start.X - constraint;
                rightBound = (int)goal.X + constraint;
            }

            if (start.Y > goal.Y)
            {
                topBound = (int)goal.Y - constraint;
                bottomBound = (int)start.Y + constraint;
            }
            else
            {
                topBound = (int)start.Y - constraint;
                bottomBound = (int)goal.Y + constraint;
            }
        }

        public static void WalkablePosition(Vector2 start, Vector2 goalPosition)
        {
            bool set;
            List<Vector2> points = new List<Vector2>();

            for (int i = leftBound; i <= rightBound; i++)
            {
                for (int j = topBound; j <= bottomBound; j++)
                    points.Add(new Vector2(i, j));
            }

            foreach (Vector2 point in points)
            {
                set = false;
                for (int k = 0; k < Game1.enemyList.Count; k++)
                {
                    Vector2 enemyPosition = new Vector2((int)Game1.enemyList[k].position.X, (int)Game1.enemyList[k].position.Y);

                    if (enemyPosition != goalPosition && Game1.enemyList[k].BoundingBox.Contains(point)) 
                    {
                        set = true;
                        walkablePosition.Add(point, false);
                        break;
                    }
                }
                if (!set)
                    walkablePosition.Add(point, true);
            }
        }

        //https://www.youtube.com/watch?v=jNZh9wppUkY
        //https://gamedev.stackexchange.com/questions/11234/2d-ray-intersection
        public static List<Point> BresenhamLine(int x0, int y0, int x1, int y1)
        {
            List<Point> result = new List<Point>();
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (steep)
            {
                int temp = x0;
                x0 = y0;
                y0 = temp;

                temp = x1;
                x1 = y1;
                y1 = temp;
            }

            //Check whether the line go left/ right
            if (x0 > x1)
            {
                int temp = x0;
                x0 = x1;
                x1 = temp;

                temp = y0;
                y0 = y1;
                y1 = temp;
            }

            int deltax = x1 - x0;               //difference between x value
            int deltay = Math.Abs(y1 - y0);     //difference between y value
            int error = 0;                      //the increment of x value before y increase
            int ystep;
            int y = y0;
            if (y0 < y1) ystep = 1; else ystep = -1;    //increment of y value to reach endPoint
            for (int x = x0; x <= x1; x++)
            {
                if (steep) result.Add(new Point(y, x));
                else result.Add(new Point(x, y));
                error += deltay;
                if (2 * error >= deltax)
                {
                    y += ystep;
                    error -= deltax;
                }
            }

            return result;
        }

        public static bool Compute2(Vector2 start, Vector2 goal)
        {
            List<Point> bresenham = BresenhamLine((int)start.X, (int)start.Y, (int)goal.X, (int)goal.Y);

            for (int i = 0; i < bresenham.Count; i++)
            {
                for (int j = 0; j < Game1.enemyList.Count; j++)
                {
                    Vector2 enemyPos = new Vector2((int)Game1.enemyList[j].position.X, (int)Game1.enemyList[j].position.Y);
                    if (enemyPos != goal && Game1.enemyList[j].BoundingBox.Contains(bresenham[i]))
                    {
                        if (i == 0)
                            lastPoint = Vector2.Zero;
                        else
                            lastPoint = new Vector2(bresenham[i - 1].X, bresenham[i - 1].Y);
                        return false;
                    }
                }
            }
            return true;
        }

        //https://www.redblobgames.com/pathfinding/a-star/implementation.html
        //https://www.youtube.com/watch?v=FsParg61xGw
        public static List<Vector2> Compute(Vector2 start, Vector2 goal)
        {
            start = new Vector2((int)start.X, (int)start.Y);
            goal = new Vector2((int)goal.X, (int)goal.Y);
            PriorityQueue<Vector2, int> priorityQueue = new PriorityQueue<Vector2, int>();
            IDictionary<Vector2, Vector2> comeFrom = new Dictionary<Vector2, Vector2>();
            IDictionary<Vector2, int> costSoFar = new Dictionary<Vector2, int>();

            if (Compute2(start, goal))
            {
                System.Diagnostics.Debug.WriteLine("bresenham");
                List<Vector2> path = new List<Vector2>();
                path.Add(goal);
                return path;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("bresenham to reduce the node");
                if (lastPoint != Vector2.Zero)
                    start = lastPoint;
            }
            Initialize(start, goal);
            walkablePosition.Clear();
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            WalkablePosition(start, goal);
            stopWatch.Stop();
            System.Diagnostics.Debug.WriteLine("walkable position: " + stopWatch.Elapsed);

            priorityQueue.Enqueue(start, 0);
            stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            IEnumerable<Vector2> validNode = walkablePosition.Where(x => x.Value).Select(x => x.Key);
            stopWatch.Stop();
            System.Diagnostics.Debug.WriteLine("valid node: " + stopWatch.Elapsed);

            stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            foreach (Vector2 node in validNode)
                costSoFar.Add(new KeyValuePair<Vector2, int>(node, int.MaxValue));
            stopWatch.Stop();
            System.Diagnostics.Debug.WriteLine("assign valid node: " + stopWatch.Elapsed);

            comeFrom[start] = start;
            costSoFar[start] = 0;
            stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            while (priorityQueue.GetCount() > -1)
            {
                Vector2 curr = priorityQueue.Dequeue();

                if (curr == goal)
                    break;

                stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Start();
                List<Vector2> neighbour = GetNeighbour(start, goal, curr, validNode);
                stopWatch.Stop();
                System.Diagnostics.Debug.WriteLine("get neighbour: " + stopWatch.Elapsed);

                foreach (Vector2 node in neighbour)
                {
                    int newCost = costSoFar[curr] + 1; //the cost to another node always 1

                    if (costSoFar[node] == costSoFar[goal] || newCost < costSoFar[node])
                    {
                        costSoFar[node] = newCost;
                        int priority = newCost + Heuristic(node, goal);
                        priorityQueue.Enqueue(node, priority);
                        if (!comeFrom.ContainsKey(node))
                            comeFrom.Add(new KeyValuePair<Vector2, Vector2>(node, curr));
                        else
                            comeFrom[node] = curr;
                    }
                }
            }
            stopWatch.Stop();
            System.Diagnostics.Debug.WriteLine("priority queue: " + stopWatch.Elapsed);
            return ConstructPath(comeFrom, start, goal);
        }

        public static List<Vector2> ConstructPath(IDictionary<Vector2, Vector2> comeFrom, Vector2 start, Vector2 goal)
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            Vector2 curr = goal;
            List<Vector2> path = new List<Vector2>();

            while (curr != start)
            {
                path.Add(curr);
                curr = comeFrom[curr];
            }
            path.Reverse();
            stopWatch.Stop();
            System.Diagnostics.Debug.WriteLine("construct path: " + stopWatch.Elapsed);
            return path;
        }

        public static int Heuristic(Vector2 start, Vector2 end)
        {
            return (int)(Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y));
        }

        public static List<Vector2> GetNeighbour(Vector2 start, Vector2 end, Vector2 curr, IEnumerable<Vector2> validNode)
        {
            List<Vector2> neighbour = new List<Vector2>();
            int constraint = 100;
            Vector2 temp;

            if (Math.Abs(curr.X - end.X) > constraint || Math.Abs(curr.Y - end.Y) > constraint)
            {
                if (curr.X + constraint <= rightBound) //right neighbour
                {
                    temp = curr;
                    temp.X += constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
                if (curr.X - constraint >= leftBound) //left neighbour
                {
                    temp = curr;
                    temp.X -= constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
                if (curr.Y - constraint >= topBound) //top neighbour
                {
                    temp = curr;
                    temp.Y -= constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
                if (curr.Y + constraint <= bottomBound) //bottom neighbour
                {
                    temp = curr;
                    temp.Y += constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }

                if (curr.X - constraint >= leftBound && curr.Y - constraint >= topBound) //top left neighbour
                {
                    temp = curr;
                    temp.X -= constraint;
                    temp.Y -= constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
                if (curr.X + constraint <= rightBound && curr.Y - constraint >= topBound) //top right neighbour
                {
                    temp = curr;
                    temp.X += constraint;
                    temp.Y -= constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
                if (curr.Y + constraint <= bottomBound && curr.X - constraint >= leftBound) //bottom left neighbour
                {
                    temp = curr;
                    temp.Y += constraint;
                    temp.X -= constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
                if (curr.Y + constraint <= bottomBound && curr.X + constraint <= rightBound) //bottom right neighbour
                {
                    temp = curr;
                    temp.Y += constraint;
                    temp.X += constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
            }
            else
            {
                temp = curr;
                temp.Y = end.Y;
                temp.X = end.X;
                if (validNode.Contains(temp))
                    neighbour.Add(temp);
            }
            return neighbour;
        }
    }
}
