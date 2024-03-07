
using System;
using System.Collections.Generic;

public class Solution
{
    private int numberOfCourses;
    private IList<int>[]? graph;
    private int[]? numberOfIncomingEgesPerCourse;
    private HashSet<int>[]? allPrerequisitesPerCourse;

    public IList<bool> CheckIfPrerequisite(int numberOfCourses, int[][] prerequisites, int[][] queries)
    {
        this.numberOfCourses = numberOfCourses;
        CreateGraph(prerequisites);
        InitializeArrayNumberOfIncomingEgesPerCourse(prerequisites);
        InitializeArrayAllPrerequisitesPerCourse();
        IterateThroughGraphViaTopologicalSortAndRecordAllPrerequisitesPerCourse();

        return performAllQueries(queries);
    }

    private IList<Boolean> performAllQueries(int[][] queries)
    {
        IList<bool> answerToAllQueries = new List<bool>();

        foreach (int[] query in queries)
        {
            int prerequisiteCourse = query[0];
            int nextCourse = query[1];
            if (allPrerequisitesPerCourse[nextCourse].Contains(prerequisiteCourse))
            {
                answerToAllQueries.Add(true);
            }
            else
            {
                answerToAllQueries.Add(false);
            }
        }
        return answerToAllQueries;
    }

    private void IterateThroughGraphViaTopologicalSortAndRecordAllPrerequisitesPerCourse()
    {
        Queue<int> queue = new Queue<int>();
        InitializeQueueWithCoursesOfZeroNumberOfIncomingEges(queue);

        while (queue.Count > 0)
        {
            int currentCourseID = queue.Dequeue();
            foreach (int nextCourseID in graph[currentCourseID])
            {
                allPrerequisitesPerCourse[nextCourseID]
                    .UnionWith(allPrerequisitesPerCourse[currentCourseID]);

                allPrerequisitesPerCourse[nextCourseID].Add(currentCourseID);

                if (--numberOfIncomingEgesPerCourse[nextCourseID] == 0)
                {
                    queue.Enqueue(nextCourseID);
                }
            }
        }
    }

    private void CreateGraph(int[][] prerequisites)
    {
        graph = new List<int>[numberOfCourses];
        for (int i = 0; i < graph.Count(); ++i)
        {
            graph[i] = new List<int>();
        }

        foreach (int[] courses in prerequisites)
        {
            int prerequisiteCourse = courses[0];
            int nextCourse = courses[1];
            graph[prerequisiteCourse].Add(nextCourse);
        }
    }

    private void InitializeArrayNumberOfIncomingEgesPerCourse(int[][] prerequisites)
    {
        numberOfIncomingEgesPerCourse = new int[numberOfCourses];
        foreach (int[] courses in prerequisites)
        {
            int nextCourse = courses[1];
            ++numberOfIncomingEgesPerCourse[nextCourse];
        }
    }

    private void InitializeQueueWithCoursesOfZeroNumberOfIncomingEges(Queue<int> queue)
    {
        for (int courseID = 0; courseID < numberOfCourses; ++courseID)
        {
            if (numberOfIncomingEgesPerCourse[courseID] == 0)
            {
                queue.Enqueue(courseID);
            }
        }
    }

    private void InitializeArrayAllPrerequisitesPerCourse()
    {
        allPrerequisitesPerCourse = new HashSet<int>[numberOfCourses];
        for (int courseID = 0; courseID < numberOfCourses; ++courseID)
        {
            allPrerequisitesPerCourse[courseID] = new HashSet<int>();
        }
    }
}
