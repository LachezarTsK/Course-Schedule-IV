
/**
 * @param {number} numberOfCourses
 * @param {number[][]} prerequisites
 * @param {number[][]} queries
 * @return {boolean[]}
 */
var checkIfPrerequisite = function (numberOfCourses, prerequisites, queries) {
    this.numberOfCourses = numberOfCourses;
    this.graph = Array.from(new Array(numberOfCourses), () => new Array());
    this.numberOfIncomingEgesPerCourse = new Array(numberOfCourses).fill(0);
    this.allPrerequisitesPerCourse = Array.from(new Array(numberOfCourses), () => new Set());

    createGraph(prerequisites);
    initializeArrayNumberOfIncomingEgesPerCourse(prerequisites);
    iterateThroughGraphViaTopologicalSortAndRecordAllPrerequisitesPerCourse();

    return performAllQueries(queries);
};

/**
 * @param {number[][]} queries
 * @return {void}
 */
function performAllQueries(queries) {
    const answerToAllQueries = new Array();

    for (let [prerequisiteCourse, nextCourse]of queries) {
        if (this.allPrerequisitesPerCourse[nextCourse].has(prerequisiteCourse)) {
            answerToAllQueries.push(true);
        } else {
            answerToAllQueries.push(false);
        }
    }
    return answerToAllQueries;
}

/**
 * @param {void} 
 * @return {void}
 */
function iterateThroughGraphViaTopologicalSortAndRecordAllPrerequisitesPerCourse() {
    // const {Queue} = require('@datastructures-js/queue');
    // Queue<Integer>
    const queue = new Queue();
    initializeQueueWithCoursesOfZeroNumberOfIncomingEges(queue);

    while (!queue.isEmpty()) {

        let currentCourseID = queue.dequeue();
        for (let nextCourseID of this.graph[currentCourseID]) {

            this.allPrerequisitesPerCourse[currentCourseID]
                    .forEach(
                            this.allPrerequisitesPerCourse[nextCourseID].add,
                            this.allPrerequisitesPerCourse[nextCourseID]
                            );
                    
            allPrerequisitesPerCourse[nextCourseID].add(currentCourseID);

            if (--this.numberOfIncomingEgesPerCourse[nextCourseID] === 0) {
                queue.enqueue(nextCourseID);
            }
        }
    }
}

/**
 * @param {number[][]} prerequisites
 * @return {void}
 */
function createGraph(prerequisites) {
    for (let [prerequisiteCourse, nextCourse] of prerequisites) {
        this.graph[prerequisiteCourse].push(nextCourse);
    }
}

/**
 * @param {number[][]} prerequisites
 * @return {void}
 */
function initializeArrayNumberOfIncomingEgesPerCourse(prerequisites) {
    for (let [prerequisiteCourse, nextCourse] of prerequisites) {
        ++this.numberOfIncomingEgesPerCourse[nextCourse];
    }
}

/**
 * @param {Queue<number>} queue
 * @return {void}
 */
function initializeQueueWithCoursesOfZeroNumberOfIncomingEges(queue) {
    for (let courseID = 0; courseID < this.numberOfCourses; ++courseID) {
        if (this.numberOfIncomingEgesPerCourse[courseID] === 0) {
            queue.enqueue(courseID);
        }
    }
}
