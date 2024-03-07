
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedList;
import java.util.List;
import java.util.Queue;
import java.util.Set;

public class Solution {

    private int numberOfCourses;
    private List<Integer>[] graph;
    private int[] numberOfIncomingEgesPerCourse;
    private Set<Integer>[] allPrerequisitesPerCourse;

    public List<Boolean> checkIfPrerequisite(int numberOfCourses, int[][] prerequisites, int[][] queries) {
        this.numberOfCourses = numberOfCourses;
        createGraph(prerequisites);
        initializeArrayNumberOfIncomingEgesPerCourse(prerequisites);
        initializeArrayAllPrerequisitesPerCourse();
        iterateThroughGraphViaTopologicalSortAndRecordAllPrerequisitesPerCourse();

        return performAllQueries(queries);
    }

    private List<Boolean> performAllQueries(int[][] queries) {
        List<Boolean> answerToAllQueries = new ArrayList<>();

        for (int[] query : queries) {
            int prerequisiteCourse = query[0];
            int nextCourse = query[1];
            if (allPrerequisitesPerCourse[nextCourse].contains(prerequisiteCourse)) {
                answerToAllQueries.add(true);
            } else {
                answerToAllQueries.add(false);
            }
        }
        return answerToAllQueries;
    }

    private void iterateThroughGraphViaTopologicalSortAndRecordAllPrerequisitesPerCourse() {
        Queue<Integer> queue = new LinkedList<>();
        initializeQueueWithCoursesOfZeroNumberOfIncomingEges(queue);

        while (!queue.isEmpty()) {

            int currentCourseID = queue.poll();
            for (int nextCourseID : graph[currentCourseID]) {

                allPrerequisitesPerCourse[nextCourseID].addAll(allPrerequisitesPerCourse[currentCourseID]);
                allPrerequisitesPerCourse[nextCourseID].add(currentCourseID);
                if (--numberOfIncomingEgesPerCourse[nextCourseID] == 0) {
                    queue.add(nextCourseID);
                }
            }
        }
    }

    private void createGraph(int[][] prerequisites) {
        graph = new List[numberOfCourses];
        for (int i = 0; i < graph.length; ++i) {
            graph[i] = new ArrayList<>();
        }

        for (int[] courses : prerequisites) {
            int prerequisiteCourse = courses[0];
            int nextCourse = courses[1];
            graph[prerequisiteCourse].add(nextCourse);
        }
    }

    private void initializeArrayNumberOfIncomingEgesPerCourse(int[][] prerequisites) {
        numberOfIncomingEgesPerCourse = new int[numberOfCourses];
        for (int[] courses : prerequisites) {
            int nextCourse = courses[1];
            ++numberOfIncomingEgesPerCourse[nextCourse];
        }
    }

    private void initializeQueueWithCoursesOfZeroNumberOfIncomingEges(Queue<Integer> queue) {
        for (int courseID = 0; courseID < numberOfCourses; ++courseID) {
            if (numberOfIncomingEgesPerCourse[courseID] == 0) {
                queue.add(courseID);
            }
        }
    }

    private void initializeArrayAllPrerequisitesPerCourse() {
        allPrerequisitesPerCourse = new Set[numberOfCourses];
        for (int courseID = 0; courseID < numberOfCourses; ++courseID) {
            allPrerequisitesPerCourse[courseID] = new HashSet<>();
        }
    }
}
