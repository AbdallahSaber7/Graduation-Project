using CHS.BLL.StudentDtos;
namespace CHS.BLL.Interfaces
{
    public interface IStudentRepository
    {
        #region get TimeTable
        Task<List<TimeTableStudentDto>> GetStudentTimeTable(int StuId);
        #endregion
        #region get the courses that are available for the student to enroll
        Task<List<CourseDataDto>> GetCoursesBySemesterTitle(string semesterTitle, int studentId);

        #endregion
        #region function void to save existing tracker info and call it in the function GetCoursesBySemesterTitle to declare maximum hours for the student according to his accumulative GPA
        Task SaveExistingTrackerInfo(int studentId, string semesterTitle);
        #endregion
        #region Enroll in a course
        Task<string> EnrollStudentInCourseGroup(EnrollmentRequestDto enrollmentRequest);
        #endregion
        #region get the Student's Tracker
        Task<StudentTrackerDto> GetStudentTracker(int studentId , string semesterTitle);
        #endregion
        #region get enrolled courses by the student 
        Task<List<EnrolledCoursesDto>> GetEnrolledCourses(int studentId,string semesterTitle);
        #endregion
        #region withdraw a course that the student is enrolled in
        Task<string> WithdrawCourse(int studentId, string courseCode, string SemesterTitle);
        #endregion
        #region get the student's results
        Task<StudentResultsDto> GetStudentResults(int studentId, string semesterTitle);
        #endregion
        #region function return accumulative GPA
        Task<decimal> CalculateAccumulativeGPA(int studentId);
        #endregion
        #region Get The student Finished Courses and Unfinished courses for a specific student
        Task<StudentFinishedCoursesDto> GetStudentFinishedCoursesAndUnfinishedCourses(int studentId);
        #endregion
        #region get the data of the specific course to re-enroll in it
        Task<List<CourseDataDto>> GetCourseDataForReEnrollement(string courseCode, string semesterTitle, int studentId);
        #endregion
        #region post re-enroll in a course
        Task<string> ReEnrollInCourse(EnrollmentRequestDto enrollmentRequestDto);
        #endregion
    }
}
