using CHS.BLL.InstructorDtos;
using CHS.BLL.Interfaces;
using Credit_Hours_System.Api.Dtos.InstructorDtos;
using Microsoft.AspNetCore.Mvc;
namespace Credit_Hours_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorRepository instructorRepository;
        public InstructorController(IInstructorRepository instructorRepository)
        {
            this.instructorRepository = instructorRepository;
        }
        #region Get Semesters

        [HttpGet("GetSemesters")]
        public async Task<ActionResult<SemesterTitlesDto>> GetSemesters()
        {
            var semesters = await instructorRepository.GetSemesters();
            //select ==>> same as foreach
            var semesterTitles = semesters.Select(semester => new SemesterTitlesDto
            {
                Title = semester.SemesterTitle
            });
            return Ok(semesterTitles);
          
        }
        #endregion
        #region Get Courses
        [HttpGet("GetInstructorCourses/{id}")]
        public async Task<ActionResult<CourseCodeAndCourseNameDto>> GetInstructorCourses(int id)
        {
            var courses = await instructorRepository.GetInstructorCourses(id);
            var courseCodeAndCourseName = courses.Select(course => new CourseCodeAndCourseNameDto
            {
                CourseCode = course.CourseCode,
                CourseTitle = course.CourseTitle
            });
            return Ok(courseCodeAndCourseName);
           
        }
        #endregion
        #region get groups
        [HttpGet("GetGroups")]
        public async Task<ActionResult<CourseGroupDto>> GetGroups()
        {
            var groups = await instructorRepository.GetGroups();
            var courseGroupDtos = groups.Select(group => new CourseGroupDto
            {
                Group = group
            });
            return Ok(courseGroupDtos);
        }
        #endregion
        #region Get Instructor Time Table
        [HttpGet("GetInstructorTimeTable/{id}")]
        public async Task<ActionResult<TimeTableDto>> GetInstructorTimeTable(int id)
        {
            var timeTable = await instructorRepository.GetInstructorTimeTable(id);
            return Ok(timeTable);
        }
        #endregion
        #region Get Grades
        [HttpGet("Grades")]
        public async Task<ActionResult<EvaluateDto>> Grades(string courseCode, string semesterTitle, string group)
        {
            var grades = await instructorRepository.Grades(courseCode, semesterTitle, group);
            return Ok(grades);
        }
        #endregion
        #region Post Grades
        [HttpPost("PostGrades")]
        public async Task<ActionResult<EvaluatePostDto>> PostGrades(IEnumerable<EvaluatePostDto> evaluatePostDtos, string courseCode, string semesterTitle, string group)
        {
            var result = await instructorRepository.PostGrades(evaluatePostDtos, courseCode, semesterTitle, group);
            return Ok(result);
        }
        #endregion
        #region get absences
        [HttpGet("GetAbsences")]
        public async Task<ActionResult<AbsenceDto>> GetAbsences(string courseCode, string semesterTitle, string group)
        {
            var absences = await instructorRepository.GetAbsences(courseCode, semesterTitle, group);
            return Ok(absences);
        }
        #endregion
        #region Post Absences
        [HttpPost("PostAbsences")]
        public async Task<ActionResult<AbsencePostDto>> PostAbsences(IEnumerable<AbsencePostDto> absencePostDtos, string courseCode, string semesterTitle, string group)
        {
            if (ModelState.IsValid)
            {
                var result = await instructorRepository.PostAbsences(absencePostDtos, courseCode, semesterTitle, group);
                return Ok(result);
            }
            else
            {
                return BadRequest("Model is not valid");
            }
        }
        #endregion
        #region get evaluation sheet
        [HttpGet("GetEvaluationSheet")]
        public async Task<ActionResult<EvaluateDto>> GetEvaluationSheet(string courseCode, string semesterTitle, string group)
        {
            var evaluationSheet = await instructorRepository.Grades(courseCode, semesterTitle, group);
            return Ok(evaluationSheet);
        }

                    


        #endregion
        #region Get Students In Course Group
        [HttpGet("GetStudentsInCourseGroup")]
        public async Task<ActionResult<StudentDto>> GetStudentsInCourseGroup(string courseCode, string semesterTitle, string group)
        {
            var students = await instructorRepository.GetStudentsInCourseGroup(courseCode, semesterTitle, group);
            var studentDtos = students.Select(student => new StudentDto
            {
                Id = student.Id,
                StudentName = student.Name,
                StudentEmail = student.Email,              
            });
            return Ok(studentDtos);
        }
        #endregion
    }
}
