using CHS.BLL.InstructorDtos;
using CHS.DAL.Entites;
using CHS.DAL.Entities;
namespace CHS.BLL.Interfaces
{
    public interface IInstructorRepository
    {
        #region Choices of Instructor
        //returns all semesters
        Task<IEnumerable<Semester>> GetSemesters();
        //returns all courses
        Task<IEnumerable<Course>> GetInstructorCourses(int id);
        //return all groups
        Task<List<string>> GetGroups();
        #endregion
        #region  Instructor TimeTable
        //use the TimeTableDto to return the type of lecture in table lecture and the interval of the lecture and course name and the group of the course of specific instructor
        Task<List<TimeTableDto>> GetInstructorTimeTable(int Insid);
        #endregion
        #region Get grades
        Task<IEnumerable<EvaluateDto>> Grades(string courseCode, string semesterTitle, string group);
        #endregion
        #region post grades
        Task<string> PostGrades(IEnumerable<EvaluatePostDto> evaluatePostDtos, string courseCode, string semesterTitle, string group);
        #endregion
        #region Class List
        Task<IEnumerable<Student>> GetStudentsInCourseGroup(string courseCode, string semesterTitle, string group);
        #endregion
        #region get absences
        Task<IEnumerable<AbsenceDto>> GetAbsences(string courseCode, string semesterTitle, string group);
        #endregion
        #region Post Absences
        Task<string> PostAbsences(IEnumerable<AbsencePostDto> absencePostDtos, string courseCode, string semesterTitle, string group);
        #endregion
       



















        //Task<IEnumerable<Instructor>> GetInstructorsTeachingCourse(string CourseCode);
        // Task<Instructor> GetInstructor(int id);
        // Task<Instructor> AddInstructor(Instructor instructor);
        // Task<Instructor> UpdateInstructor(Instructor instructor);
        // Task<Instructor> DeleteInstructor(int id);
        //list of courses that the instructor is teaching
        // Task<IEnumerable<Course>> GetInstructorCourses(int id);
    }
}
