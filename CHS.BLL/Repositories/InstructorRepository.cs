using CHS.BLL.InstructorDtos;
using CHS.BLL.Interfaces;
using CHS.DAL;
using CHS.DAL.Entites;
using CHS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CHS.BLL.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly CreditHoursSystemContext context;

        public InstructorRepository(CreditHoursSystemContext context)
        {
            this.context = context;
        }
        #region Choices of Instructor
        public async Task<IEnumerable<Semester>> GetSemesters()
        {
            //get all semesters
            return await context.Set<Semester>().ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetInstructorCourses(int InsId)
        {
            //get the distinct courses of the instructor

            var query = (from Course in context.Courses
                        join courseGroup in context.CourseGroups on Course.CourseCode equals courseGroup.CourseCode
                         join lecture in context.Lectures on  courseGroup.Id equals lecture.CourseGroupId
                        where lecture.InstructorId == InsId
                        select new Course
                        {
                            CourseCode = Course.CourseCode,
                            CourseTitle = Course.CourseTitle,
                        }).Distinct();
            return query.ToList();
        }


        public async Task<List<string>> GetGroups()
        {
            // Get distinct groups
            var uniqueGroups = await context.Set<CourseGroup>().Select(group => group.Group).Distinct().ToListAsync();
            return uniqueGroups;
        }
        #endregion
        #region  Instructor TimeTable
        public async Task<List<TimeTableDto>> GetInstructorTimeTable(int Insid)
        {
            var timeTable = await context.Lectures
                .Where(t => t.InstructorId == Insid)
                .Join(
                    context.CourseGroups,
                    t => t.CourseGroupId,
                    cg => cg.Id,
                    (t, cg) => new { t, cg }
                )
                .Join(
                    context.Courses,
                    j => j.cg.CourseCode,
                    c => c.CourseCode,
                    (j, c) => new { j.t, j.cg, c }
                )
                .Join(
                    context.Lectures,
                    j => j.cg.Id,
                    l => l.CourseGroupId,
                    (j, l) => new { j.t, j.cg, j.c, l }
                )
                .Join(
                    context.ClassRooms,
                    j => j.l.ClassRoomId,
                    cr => cr.Id,
                    (j, cr) => new { j.t, j.cg, j.c, j.l, cr }
                )
                .Join(
                    context.Intervals,
                    j => j.l.IntervalIdFk,
                    i => i.Id,
                    (j, i) => new { j.t, j.cg, j.c, j.l, j.cr, i }
                )
                .Join(
                    context.Instructors,
                    j => j.l.InstructorId,
                    ins => ins.Id,
                    (j, ins) => new TimeTableDto
                    {
                        Type = j.l.Type,
                        CourseTitle = j.c.CourseTitle,
                        Group = j.cg.Group,
                        RoomName = j.cr.RoomName,
                        Day = j.l.Day,
                        StartInterval = j.i.StartInterval,
                        EndInterval = j.i.EndInterval,
                        InsName = ins.InstructorName // Add the instructor name to the TimeTableDto
                    }
                )
                .ToListAsync();
            return timeTable;
        }
        #endregion
        #region Get grades
        public async Task<IEnumerable<EvaluateDto>> Grades(string courseCode, string semesterTitle, string group)
        {
            var courseGroupId = context.CourseGroups
              .Where(cg => cg.CourseCode == courseCode && cg.SemesterTitle == semesterTitle && cg.Group == group)
              .Select(cg => cg.Id).FirstOrDefault();
            //get the students grades in the course group
                var grades = await (
                from student in context.Students
                join evaluate in context.Evaluates on student.Id equals evaluate.StudentId
                join course in context.Courses on evaluate.CourseGroupId equals courseGroupId
                join courseGroup in context.CourseGroups on course.CourseCode equals courseGroup.CourseCode
                where courseGroup.Id == courseGroupId
                select new EvaluateDto
                {
                    StudentId = student.Id,
                    Name = student.Name,
                    MidtermGrade = evaluate.MidtermGrade,
                    PreFinalGrade = evaluate.PreFinalGrade,
                    FinalGrade = evaluate.FinalGrade,
                    AbsencePercent = evaluate.AbsencePercent,
                    TotalGrade = evaluate.TotalGrade,
                    WeekTwelve = evaluate.WeekTwelve,
                    LetterGrade = evaluate.LetterGrade,
                    PointsGrade = evaluate.PointsGrade
                }
             ).ToListAsync();
             return grades;
        }
        #endregion
        #region Add grades
        public async Task<string> PostGrades(IEnumerable<EvaluatePostDto> evaluatePostDtos, string courseCode, string semesterTitle, string group)
        {
            try
            {
                // Retrieve course group ID based on provided parameters
                var courseGroupId = context.CourseGroups
                    .Where(cg => cg.CourseCode == courseCode && cg.SemesterTitle == semesterTitle && cg.Group == group)
                    .Select(cg => cg.Id)
                    .FirstOrDefault();

                // Check if the course group exists
                if (courseGroupId == 0)
                {
                    // Course group not found, handle accordingly
                    return "Course Group Not Found";
                }

                foreach (var dto in evaluatePostDtos)
                {
                    // Check if the student with the given ID exists
                    var student = await context.Students.FindAsync(dto.StudentId);
                    if (student == null)
                    {
                        // Student not found, handle accordingly
                        return $"Student with ID {dto.StudentId} Not Found";
                    }

                    // Create or update the Evaluate entity for the student in the specified course group
                    var evaluate = await context.Evaluates
                        .Where(e => e.StudentId == dto.StudentId && context.CourseGroups.Any(cg => cg.Id == courseGroupId && cg.Id == e.CourseGroupId))
                        .FirstOrDefaultAsync();

                    if (evaluate == null)
                    {
                        // If the evaluation doesn't exist, create a new one
                       return "Student Doen't Exist in this Course Group";
                    }
                    else
                    {
                        // If the evaluation exists, update the grades
                        evaluate.MidtermGrade = dto.MidtermGrade;
                        evaluate.WeekTwelve = dto.WeekTwelve;
                        evaluate.AbsencePercent = dto.AbsencePercent;
                        evaluate.PreFinalGrade = dto.PreFinalGrade;
                        evaluate.FinalGrade = dto.FinalGrade;
                        evaluate.TotalGrade =  dto.MidtermGrade + dto.WeekTwelve + dto.PreFinalGrade +  dto.FinalGrade;
                        if (evaluate.FinalGrade < 30)
                        {
                            evaluate.LetterGrade = "F";
                        }
                        else
                        {
                            if (evaluate.TotalGrade >= 96)
                            {
                                evaluate.LetterGrade = "A+";
                                evaluate.PointsGrade = 4.0m;
                            }
                            else if (evaluate.TotalGrade >= 92 && evaluate.TotalGrade < 96)
                            {
                                evaluate.LetterGrade = "A";
                                evaluate.PointsGrade = 3.7m;
                            }
                            else if (evaluate.TotalGrade >= 88 && evaluate.TotalGrade < 92)
                            {
                                evaluate.LetterGrade = "A-";
                                evaluate.PointsGrade = 3.4m;
                            }
                            else if (evaluate.TotalGrade >= 84 && evaluate.TotalGrade < 88)
                            {
                                evaluate.LetterGrade = "B+";
                                evaluate.PointsGrade = 3.2m;
                            }
                            else if (evaluate.TotalGrade >= 80 && evaluate.TotalGrade < 84)
                            {
                                evaluate.LetterGrade = "B";
                                evaluate.PointsGrade = 3.0m;
                            }
                            else if (evaluate.TotalGrade >= 76 && evaluate.TotalGrade < 80)
                            {
                                evaluate.LetterGrade = "B-";
                                evaluate.PointsGrade = 2.8m;
                            }
                            else if (evaluate.TotalGrade >= 72 && evaluate.TotalGrade < 76)
                            {
                                evaluate.LetterGrade = "C+";
                                    evaluate.PointsGrade = 2.6m;
                            }
                            else if (evaluate.TotalGrade >= 68 && evaluate.TotalGrade < 72)
                            {
                                evaluate.LetterGrade = "C";
                                evaluate.PointsGrade = 2.4m;
                            }
                            else if (evaluate.TotalGrade >= 64 && evaluate.TotalGrade < 68)
                            {
                                evaluate.LetterGrade = "C-";
                                evaluate.PointsGrade = 2.2m;
                            }
                            else if (evaluate.TotalGrade >= 60 && evaluate.TotalGrade < 64)
                            {
                                evaluate.LetterGrade = "D+";
                                evaluate.PointsGrade = 2.0m;
                            }
                            else if (evaluate.TotalGrade >= 55 && evaluate.TotalGrade < 60)
                            {
                                evaluate.LetterGrade = "D";
                                evaluate.PointsGrade = 1.5m;
                            }
                            else if (evaluate.TotalGrade >= 50 && evaluate.TotalGrade < 55)
                            {
                                evaluate.LetterGrade = "D-";
                                evaluate.PointsGrade = 1.0m;
                            }
                            else
                            {
                                evaluate.LetterGrade = "F";
                                evaluate.PointsGrade = 0.0m;
                            }
                        }

                        context.Evaluates.Update(evaluate);
                    }
                }

                await context.SaveChangesAsync();

                return "Grades Added successfully";
            }
            catch (Exception ex)
            {
                // Handle exceptions accordingly
                return $"An Error Occurred: {ex.Message}";
            }
        }


        #endregion
        #region get absences
        public async Task<IEnumerable<AbsenceDto>> GetAbsences(string courseCode, string semesterTitle, string group)
        {
            var courseGroupId = context.CourseGroups
                .Where(cg => cg.CourseCode == courseCode && cg.SemesterTitle == semesterTitle && cg.Group == group)
                .Select(cg => cg.Id)
                .FirstOrDefault();

            var absences = await (
                from student in context.Students
                join absence in context.Records on student.Id equals absence.StudentId
                join courseGroup in context.CourseGroups on absence.CourseGroupId equals courseGroup.Id
                where courseGroup.Id == courseGroupId
                group absence.Week by new { student.Id, student.Name } into g
                select new AbsenceDto
                {
                    StudentId = g.Key.Id,
                    Name = g.Key.Name,
                    Weeks = g.ToList()
                }).ToListAsync();

            return absences;
        }

        #endregion
        #region Post Absences
        public async Task<string> PostAbsences(IEnumerable<AbsencePostDto> absencePostDtos, string courseCode, string semesterTitle, string group)
        {
            try
            {
                var courseGroupId = context.CourseGroups
                    .Where(cg => cg.CourseCode == courseCode && cg.SemesterTitle == semesterTitle && cg.Group == group)
                    .Select(cg => cg.Id)
                    .FirstOrDefault();

                if (courseGroupId == 0)
                {
                    return "Course Group Not Found";
                }

                foreach (var dto in absencePostDtos)
                {

                    // Check if the student with the given ID exists
                    var student = await context.Students.FindAsync(dto.StudentId);
                    if (student == null)
                    {
                        // Student not found, handle accordingly
                        return $"Student with ID {dto.StudentId} Not Found";
                    }
                    // Check if the week collection is not null and not empty
                   
                        foreach (var week in dto.Week)
                        {
                            // Check if the record exists for this student and course group for the current week
                            var record = await context.Records.FirstOrDefaultAsync(r => r.StudentId == dto.StudentId && r.CourseGroupId == courseGroupId && r.Week == week);

                            if (record == null)
                            {
                                // If the record doesn't exist, create a new one
                                record = new Record
                                {
                                    StudentId = dto.StudentId,
                                    CourseGroupId = courseGroupId,
                                    Week = week
                                };

                                await context.Records.AddAsync(record);
                            }
                            else
                            {
                                // If the record exists, update the week
                                record.Week = week;
                                context.Records.Update(record);
                            }
                        }
                    }
                await context.SaveChangesAsync();
                return "Absences Added successfully";
            }
            catch (Exception ex)
            {
                // Handle exceptions accordingly
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }


        #endregion
        #region Class List
        public async Task<IEnumerable<Student>> GetStudentsInCourseGroup(string courseCode, string semesterTitle, string group)
        {
            var courseGroupId = context.CourseGroups
              .Where(cg => cg.CourseCode == courseCode && cg.SemesterTitle == semesterTitle && cg.Group == group)
              .Select(cg => cg.Id).FirstOrDefault();
            //get the students in the course group
            var studentsInCourseGroup = await (
                from student in context.Students
                join enroll in context.Enrolls on student.Id equals enroll.StudentId
                where enroll.CourseGroupId == courseGroupId
                select new Student
                {
                    Id = student.Id,
                    Name = student.Name
                }
            ).ToListAsync();
            return studentsInCourseGroup;
        }

        #endregion
    }
}
