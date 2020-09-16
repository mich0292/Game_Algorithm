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
        public static int leftBound;
        public static int rightBound;
        public static int topBound;
        public static int bottomBound;

        public static void Initialize(Vector2 start, Vector2 goal)
        {
            if (start.X > goal.X)
            {
                leftBound = (int)goal.X;
                rightBound = (int)start.X;
            }
            else
            {
                leftBound = (int)start.X;
                rightBound = (int)goal.X;
            }

            if (start.Y > goal.Y)
            {
                topBound = (int)goal.Y;
                bottomBound = (int)start.Y;
            }
            else
            {
                topBound = (int)start.Y;
                bottomBound = (int)goal.Y;
            }
        }

        public static void WalkablePosition(Vector2 start, Vector2 goalPosition)
        {
            bool set;
            Vector2 position;

            for (int i = leftBound; i <= rightBound; i++)
            {
                for (int j = topBound; j <= bottomBound; j++)
                {
                    set = false;
                    position = new Vector2(i, j);

                    if (position == goalPosition)
                    {
                        set = true;
                        walkablePosition.Add(position, true);
                    }
                    else
                    {
                        for (int k = 0; k < Game1.enemyList.Count; k++)
                        {
                            Vector2 enemyPosition = new Vector2((int)Game1.enemyList[k].position.X, (int)Game1.enemyList[k].position.Y);
                            if (enemyPosition == position) //if the i and j is equal to enemy position, set the position to not walkable
                            {
                                set = true;
                                walkablePosition.Add(position, false);
                                break;
                            }
                        }
                    }
                    if (!set)
                        walkablePosition.Add(position, true);
                }
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
            foreach (Point p in BresenhamLine((int)start.X, (int)start.Y, (int)goal.X, (int)goal.Y))
            {
                //Console.WriteLine(p);                  
                for (int i = 0; i < Game1.enemyList.Count; i++)
                {
                    Vector2 enemyPos = new Vector2((int)Game1.enemyList[i].position.X, (int)Game1.enemyList[i].position.Y);
                    if (enemyPos != goal && Game1.enemyList[i].BoundingBox.Contains(p))
                    {
                        //Console.WriteLine(Game1.enemyList[i].BoundingBox.Contains(p));
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
                System.Diagnostics.Debug.WriteLine("compute 2");
                List<Vector2> path = new List<Vector2>();
                path.Add(start);
                path.Add(goal);
                return path;
            }

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
            {
                costSoFar.Add(new KeyValuePair<Vector2, int>(node, int.MaxValue));
                comeFrom.Add(new KeyValuePair<Vector2, Vector2>(node, Vector2.Zero));
            }
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
            //Vector2 diff = end - start;
            //diff.X = (int)(diff.X / 16);
            //diff.Y = (int)(diff.Y / 16);
            Vector2 temp;
            //System.Diagnostics.Debug.WriteLine("current node: " + curr);
            if (Math.Abs(curr.X - end.X) >= constraint || Math.Abs(curr.Y - end.Y) >= constraint)
            {
                if (curr.X + constraint <= rightBound) //right neighbour
                {
                    temp = curr;
                    //temp.X += diff.X;        
                    temp.X += constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
                if (curr.X - constraint >= leftBound) //left neighbour
                {
                    temp = curr;
                    //temp.X -= diff.X;
                    temp.X -= constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
                if (curr.Y - constraint >= topBound) //top neighbour
                {
                    temp = curr;
                    //temp.Y -= diff.Y;
                    temp.Y -= constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
                if (curr.Y + constraint <= bottomBound) //bottom neighbour
                {
                    temp = curr;
                    //temp.Y += diff.Y;
                    temp.Y += constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }

                if (curr.X - constraint >= leftBound && curr.Y - constraint >= topBound) //top left neighbour
                {
                    temp = curr;
                    //temp.X -= diff.X;
                    //temp.Y -= diff.Y;
                    temp.X -= constraint;
                    temp.Y -= constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
                if (curr.X + constraint <= rightBound && curr.Y - constraint >= topBound) //top right neighbour
                {
                    temp = curr;
                    //temp.X += diff.X;
                    //temp.Y -= diff.Y;
                    temp.X += constraint;
                    temp.Y -= constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
                if (curr.Y + constraint <= bottomBound && curr.X - constraint >= leftBound) //bottom left neighbour
                {
                    temp = curr;
                    //temp.Y += diff.Y;
                    //temp.X -= diff.X;
                    temp.Y += constraint;
                    temp.X -= constraint;
                    if (validNode.Contains(temp))
                        neighbour.Add(temp);
                }
                if (curr.Y + constraint <= bottomBound && curr.X + constraint <= rightBound) //bottom right neighbour
                {
                    temp = curr;
                    //temp.Y += diff.Y;
                    //temp.X += diff.X;
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
