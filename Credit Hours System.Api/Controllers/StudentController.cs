using CHS.BLL.Interfaces;
using CHS.BLL.StudentDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Credit_Hours_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        public StudentController(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }
        #region Get Student Time Table
        [HttpGet("GetStudentTimeTable/{id}")]
        public async Task<ActionResult<TimeTableStudentDto>> GetStudentTimeTable(int id)
        {
            var timeTable = await studentRepository.GetStudentTimeTable(id);
            return Ok(timeTable);
        }
        #endregion
        #region get the courses that are available for the student to enroll
        [HttpGet("GetCoursesBySemesterTitle/{semesterTitle}/{stuId}")]
        public async Task<ActionResult<CourseDataDto>> GetCoursesBySemesterTitle(string semesterTitle, int stuId)
        {
            var courses = await studentRepository.GetCoursesBySemesterTitle(semesterTitle, stuId);
            return Ok(courses);
        }
        #endregion
        #region Enroll in a course
        [HttpPost("EnrollInCourse")]
        public async Task<ActionResult<string>> EnrollInCourse(EnrollmentRequestDto enrollmentRequest)
        {
            var result = await studentRepository.EnrollStudentInCourseGroup(enrollmentRequest);
            return Ok(result);
        }
        #endregion
        #region get the Student's Tracker
        [HttpGet("GetStudentTracker/{studentId}/{semesterTitle}")]
        public async Task<ActionResult<StudentTrackerDto>> GetStudentTracker(int studentId, string semesterTitle)
        {
            var tracker = await studentRepository.GetStudentTracker(studentId, semesterTitle);
            return Ok(tracker);
        }

        #endregion
        #region get enrolled courses by the student 
        [HttpGet("GetEnrolledCourses/{studentId}/{semesterTitle}")]
        public async Task<ActionResult<EnrolledCoursesDto>> GetEnrolledCourses(int studentId, string semesterTitle)
        {
            var courses = await studentRepository.GetEnrolledCourses(studentId, semesterTitle);
            return Ok(courses);
        }

        #endregion
        #region withdraw a course that the student is enrolled in
        [HttpDelete("WithdrawCourse/{studentId}/{courseCode}/{semesterTitle}")]
        public async Task<ActionResult<string>> WithdrawCourse(int studentId, string courseCode, string semesterTitle)
        {
            var result = await studentRepository.WithdrawCourse(studentId, courseCode, semesterTitle);
            return Ok(result);
        }
        #endregion
        #region get the student's results
        [HttpGet("GetStudentResults/{studentId}/{semesterTitle}")]
        public async Task<ActionResult<StudentResultsDto>> GetStudentResults(int studentId, string semesterTitle)
        {
            var results = await studentRepository.GetStudentResults(studentId, semesterTitle);
            return Ok(results);
        }
        #endregion
        #region Get The student Finished Courses and Unfinished courses for a specific student
        [HttpGet("GetStudentFinishedCoursesAndUnfinishedCourses/{studentId}")]
        public async Task<ActionResult<StudentFinishedCoursesDto>> GetStudentFinishedCoursesAndUnfinishedCourses(int studentId)
        {
            var courses = await studentRepository.GetStudentFinishedCoursesAndUnfinishedCourses(studentId);
            return Ok(courses);
        }
        #endregion
        #region get the data of the specific course to re-enroll in it
        [HttpGet("GetCourseDataForReEnrollement/{courseCode}/{semesterTitle}/{studentId}")]
        public async Task<ActionResult<CourseDataDto>> GetCourseDataForReEnrollement(string courseCode, string semesterTitle, int studentId)
        {
            var courseData = await studentRepository.GetCourseDataForReEnrollement(courseCode, semesterTitle, studentId);
            if (courseData.IsNullOrEmpty())
            {
                return NotFound("Course Not Availiable In This Semester");
            }
            else
                return Ok(courseData);
        }
        #endregion
        #region re-enroll in a course
        [HttpPost("ReEnrollInCourse")]
        public async Task<ActionResult<string>> ReEnrollInCourse(EnrollmentRequestDto enrollmentRequestDto)
        {
            var result = await studentRepository.ReEnrollInCourse(enrollmentRequestDto);
            return Ok(result);
        }
        
        #endregion
    }
}
