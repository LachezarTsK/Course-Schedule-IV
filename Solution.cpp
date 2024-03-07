
#include <span>
#include <queue>
#include <vector>
#include <unordered_set>
using namespace std;

class Solution {

    int numberOfCourses;
    vector<vector<int>> graph;
    vector<int> numberOfIncomingEgesPerCourse;
    vector<unordered_set<int>> allPrerequisitesPerCourse;

public:
    vector<bool> checkIfPrerequisite(int numberOfCourses, const vector<vector<int>>& prerequisites, const vector<vector<int>>& queries) {
        this->numberOfCourses = numberOfCourses;
        createGraph(prerequisites);
        initializeVectorNumberOfIncomingEgesPerCourse(prerequisites);
        allPrerequisitesPerCourse.resize(numberOfCourses);
        iterateThroughGraphViaTopologicalSortAndRecordAllPrerequisitesPerCourse();

        return performAllQueries(queries);
    }

private:
    vector<bool> performAllQueries(span<const vector<int>> queries) const {
        vector<bool> answerToAllQueries;

        for (const auto& query : queries) {
            int prerequisiteCourse = query[0];
            int nextCourse = query[1];
            if (allPrerequisitesPerCourse[nextCourse].contains(prerequisiteCourse)) {
                answerToAllQueries.push_back(true);
            }
            else {
                answerToAllQueries.push_back(false);
            }
        }
        return answerToAllQueries;
    }

    void iterateThroughGraphViaTopologicalSortAndRecordAllPrerequisitesPerCourse() {
        queue<int> queue;
        initializeQueueWithCoursesOfZeroNumberOfIncomingEges(queue);

        while (!queue.empty()) {

            int currentCourseID = queue.front();
            queue.pop();

            for (const auto& nextCourseID : graph[currentCourseID]) {

                allPrerequisitesPerCourse[nextCourseID]
                    .insert(
                        allPrerequisitesPerCourse[currentCourseID].begin(),
                        allPrerequisitesPerCourse[currentCourseID].end()
                    );

                allPrerequisitesPerCourse[nextCourseID].insert(currentCourseID);

                if (--numberOfIncomingEgesPerCourse[nextCourseID] == 0) {
                    queue.push(nextCourseID);
                }
            }
        }
    }

    void createGraph(span<const vector<int>> prerequisites) {
        graph.resize(numberOfCourses);

        for (const auto& courses : prerequisites) {
            int prerequisiteCourse = courses[0];
            int nextCourse = courses[1];
            graph[prerequisiteCourse].push_back(nextCourse);
        }
    }

    void initializeVectorNumberOfIncomingEgesPerCourse(span<const vector<int>> prerequisites) {
        numberOfIncomingEgesPerCourse.resize(numberOfCourses);
        for (const auto& courses : prerequisites) {
            int nextCourse = courses[1];
            ++numberOfIncomingEgesPerCourse[nextCourse];
        }
    }

    void initializeQueueWithCoursesOfZeroNumberOfIncomingEges(queue<int>& queue) const {
        for (int courseID = 0; courseID < numberOfCourses; ++courseID) {
            if (numberOfIncomingEgesPerCourse[courseID] == 0) {
                queue.push(courseID);
            }
        }
    }
};
