using CHS.BLL.Interfaces;
using CHS.BLL.StudentDtos;
using CHS.DAL;
using CHS.DAL.Entites;
using Microsoft.EntityFrameworkCore;

namespace CHS.BLL.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CreditHoursSystemContext context;

        public StudentRepository(CreditHoursSystemContext context)
        {
            this.context = context;
        }

        #region get student TimeTable  
        public async Task<List<TimeTableStudentDto>> GetStudentTimeTable(int studentId)
        {
            var timeTable = await context.Enrolls
                .Where(sc => sc.StudentId == studentId) // Filter by the student ID
                .Join(
                    context.CourseGroups,
                    sc => sc.CourseGroupId,
                    cg => cg.Id,
                    (sc, cg) => new { sc, cg }
                )
                .Join(
                    context.Courses,
                    j => j.cg.CourseCode,
                    c => c.CourseCode,
                    (j, c) => new { j.sc, j.cg, c }
                )
                .Join(
                    context.Lectures,
                    j => j.cg.Id,
                    l => l.CourseGroupId,
                    (j, l) => new { j.sc, j.cg, j.c, l }
                )
                .Join(
                    context.ClassRooms,
                    j => j.l.ClassRoomId,
                    cr => cr.Id,
                    (j, cr) => new { j.sc, j.cg, j.c, j.l, cr }
                )
                .Join(
                    context.Intervals,
                    j => j.l.IntervalIdFk,
                    i => i.Id,
                    (j, i) => new { j.sc, j.cg, j.c, j.l, j.cr, i }
                )
                .Join(
                    context.Instructors,
                    j => j.l.InstructorId,
                    ins => ins.Id,
                    (j, ins) => new TimeTableStudentDto
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
        #region get the courses that are available for the student to enroll in a semester
        public async Task<List<CourseDataDto>> GetCoursesBySemesterTitle(string semesterTitle, int studentId)
        {
            SaveExistingTrackerInfo(studentId, semesterTitle).Wait();

            var courseData = (from c in context.Courses // Select all courses from the Courses table in the database
                              join cg in context.CourseGroups on c.CourseCode equals cg.CourseCode // Join with CourseGroups table on CourseCode
                              join l in context.Lectures on cg.Id equals l.CourseGroupId // Join with Lectures table on CourseGroupId
                              join i in context.Intervals on l.IntervalIdFk equals i.Id // Join with Intervals table on IntervalIdFk
                              where cg.SemesterTitle == semesterTitle && l.Type == "Lecture" && // Filtering based on SemesterTitle and Lecture type
                              !context.Evaluates.Any(e => e.StudentId == studentId && e.FinalGrade >= 30 && e.TotalGrade >= 50 && e.CourseGroup.CourseCode == c.CourseCode) && // Excluding courses already evaluated with a grade above or equal to 30 for the specific student
                              (c.PrerequisiteCode == null || // Check if there is no prerequisite or if there is a prerequisite, ensure it is already evaluated with a grade above or equal to 30
                              !context.Courses.Any(c2 =>
                                  c2.CourseCode == c.PrerequisiteCode &&
                                  !context.Evaluates.Any(e2 =>
                                      e2.StudentId == studentId && e2.FinalGrade >= 30 && e2.TotalGrade >= 50 && e2.CourseGroup.CourseCode == c.CourseCode))) &&
                                      !context.Evaluates.Any(e => e.StudentId == studentId && e.LetterGrade == "U" && e.CourseGroup.CourseCode == c.CourseCode) // Exclude courses with LetterGrade "U"
                              select new CourseDataDto // Selecting specific data fields to return as CourseDataDto
                              {
                                  CourseCode = c.CourseCode,
                                  CourseTitle = c.CourseTitle,
                                  CourseGroups = cg.Group,
                                  CreditHours = c.CreditHours,
                                  PrerequisiteCourseCode = c.PrerequisiteCode,
                                  LectureDay = l.Day,
                                  LectureStartTime = i.StartInterval,
                                  LectureEndTime = i.EndInterval
                              }).ToList();

            return courseData;
        }
        #endregion
        #region function void to save existing tracker info and call it in the function GetCoursesBySemesterTitle to declare maximum hours for the student according to his accumulative GPA
        public async Task SaveExistingTrackerInfo(int studentId, string semesterTitle)
        {
            //get the latest tracker info for the student
            var latestTrackerInfo = await context.studentTrackers
                .Where(st => st.StudentId == studentId)
                .OrderByDescending(st => st.StudentId)
                .FirstOrDefaultAsync();

            var NewTrackerEntry = new StudentTracker();


            if (latestTrackerInfo == null)
            {
                // Create a new entry for the current semester
                NewTrackerEntry.StudentId = studentId;
                NewTrackerEntry.SemesterTitle = semesterTitle;
                NewTrackerEntry.Tracker = 1;
                NewTrackerEntry.MaximumHours = 21;
            }
            else
            {
                // Update MaximumHours based on accumulative GPA
                var student = await context.Students.FirstOrDefaultAsync(s => s.Id == studentId);

                if (student.Id == studentId && !semesterTitle.Contains("Summer") && latestTrackerInfo.SemesterTitle != semesterTitle)
                {
                    NewTrackerEntry.StudentId = studentId;
                    NewTrackerEntry.SemesterTitle = semesterTitle;
                    NewTrackerEntry.Tracker = latestTrackerInfo.Tracker + 1;

                    if (student.AccumulativeGpa >= 3)
                    {
                        NewTrackerEntry.MaximumHours = 21;
                    }
                    else if (student.AccumulativeGpa >= 2 && student.AccumulativeGpa < 3)
                    {
                        NewTrackerEntry.MaximumHours = 18;
                    }
                    else if (student.AccumulativeGpa >= 1 && student.AccumulativeGpa < 2)
                    {
                        NewTrackerEntry.MaximumHours = 18;
                    }
                    else if (student.AccumulativeGpa >= 1)
                    {
                        NewTrackerEntry.MaximumHours = 12;
                    }
                    context.studentTrackers.Add(NewTrackerEntry);
                }
                else if (student.Id != null && !semesterTitle.Contains("Summer") && latestTrackerInfo.SemesterTitle == semesterTitle)
                {
                    return;
                }
                else
                {
                    NewTrackerEntry.StudentId = studentId;
                    NewTrackerEntry.SemesterTitle = semesterTitle;
                    NewTrackerEntry.Tracker = latestTrackerInfo.Tracker + 1;
                    if (student.AccumulativeGpa >= 3)
                    {
                        NewTrackerEntry.MaximumHours = 9;
                    }
                    else if (student.AccumulativeGpa >= 0 && student.AccumulativeGpa < 3)
                    {
                        NewTrackerEntry.MaximumHours = 6;
                    }
                    context.studentTrackers.Add(NewTrackerEntry);
                }
            }
            // Save changes to the database
            await context.SaveChangesAsync();
        }
        #endregion
        //select one course or multiple course?????????????/// Dr/salaaahh
        #region Enroll in a course
        public async Task<string> EnrollStudentInCourseGroup(EnrollmentRequestDto request)
        {
            try
            {
                //check if the student exists
                var student = await context.Students.FindAsync(request.StudentId);
                if (student == null)
                    return "Student Not Found";

                //get the course group by the course code, semester title, and group
                var courseGroup = context.CourseGroups.FirstOrDefault(cg =>
                    cg.CourseCode == request.CourseCode &&
                    cg.SemesterTitle == request.SemesterTitle &&
                    cg.Group == request.Group);

                if (courseGroup == null)
                    return "Course group not found";

                //check if the course group is full
                if (courseGroup.CurrentCapacity >= courseGroup.Capacity)
                    return "Course is full";

                //check if the student is already enrolled in the course group
                var existingEnrollment = context.Enrolls
                    .FirstOrDefault(e => e.StudentId == request.StudentId && e.CourseGroupId == courseGroup.Id);
                if (existingEnrollment != null)
                    return "Enrolled before";

                //create a new enrollment and add it to the database
                var enrollment = new Enroll
                {
                    StudentId = request.StudentId,
                    CourseGroupId = courseGroup.Id
                };

                //create a new Evaluation for the student and add it to the database
                var evaluation = new Evaluate
                {
                    InstructorId = await context.Lectures
                    .Where(l => l.CourseGroupId == courseGroup.Id && l.Type == "Lecture")
                    .Select(l => l.InstructorId)
                    .FirstOrDefaultAsync(),
                    StudentId = request.StudentId,
                    CourseGroupId = courseGroup.Id,
                    PreFinalGrade = 0,
                    MidtermGrade = 0,
                    WeekTwelve = 0,
                    TotalGrade = 0,
                    AbsencePercent = 0,
                    LetterGrade = "U",
                    FinalGrade = 0
                };

                //increment the current capacity of the course group
                courseGroup.CurrentCapacity++;

                context.Enrolls.Add(enrollment);
                context.Evaluates.Add(evaluation);
                await context.SaveChangesAsync();

                return "Enrolled successfully";
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return ex.Message;
            }
        }
        #endregion
        #region get the Student's Tracker
        public async Task<StudentTrackerDto> GetStudentTracker(int studentId, string semesterTitle)
        {
            var studentTracker = await context.studentTrackers
                .Where(st => st.StudentId == studentId && st.SemesterTitle == semesterTitle)
                .Select(st => new StudentTrackerDto
                {
                    SemesterGPA = st.SemesterGPA,
                    RecordedHours = st.RecordedHours,
                    MaximumHours = st.MaximumHours
                })
                .FirstOrDefaultAsync();

            return studentTracker;
        }
        #endregion
        #region get enrolled courses by the student 
        public async Task<List<EnrolledCoursesDto>> GetEnrolledCourses(int studentId, string semesterTitle)
        {
            var enrolledCourses = await (from c in context.Courses
                                         join cg in context.CourseGroups on c.CourseCode equals cg.CourseCode
                                         join e in context.Enrolls on cg.Id equals e.CourseGroupId
                                         where e.StudentId == studentId && cg.SemesterTitle == semesterTitle
                                         select new EnrolledCoursesDto
                                         {
                                             CourseCode = c.CourseCode,
                                             CourseTitle = c.CourseTitle,
                                             CreditHours = c.CreditHours
                                         }).ToListAsync();

            return enrolledCourses;
        }
        #endregion
        #region withdraw a course that the student is enrolled in
        public async Task<string> WithdrawCourse(int studentId, string courseCode, string semesterTitle)
        {
            try
            {
                // Get the course group by the course code and semester title
                var courseGroup = context.CourseGroups.FirstOrDefault(cg =>
                    cg.CourseCode == courseCode &&
                    cg.SemesterTitle == semesterTitle);

                if (courseGroup == null)
                    return "Course group not found";

                // Check if the student is already enrolled in the course group
                var existingEnrollment = context.Enrolls
                    .FirstOrDefault(e => e.StudentId == studentId && e.CourseGroupId == courseGroup.Id);
                if (existingEnrollment == null)
                    return "Not enrolled in the course";

                // Update the evaluation to change the LetterGrade to "W" if exists
                var existingEvaluation = context.Evaluates
                    .FirstOrDefault(e => e.StudentId == studentId && e.CourseGroupId == courseGroup.Id);
                if (existingEvaluation != null)
                {
                    existingEvaluation.LetterGrade = "W";
                }

                // Remove the enrollment from the database
                context.Enrolls.Remove(existingEnrollment);

                // Decrement the current capacity of the course group
                courseGroup.CurrentCapacity--;

                await context.SaveChangesAsync();

                return "Withdrawn successfully";
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return ex.Message;
            }
        }

        #endregion
        #region get the student's results for a specific semester
        public async Task<StudentResultsDto> GetStudentResults(int studentId, string semesterTitle)
        {
            var studentResults = await context.Evaluates
                .Where(e => e.StudentId == studentId && e.CourseGroup.SemesterTitle == semesterTitle && e.LetterGrade != "W")
                .Select(e => new StudentListResultsDto
                {
                    CourseTitle = e.CourseGroup.Course.CourseTitle,
                    CreditHours = e.CourseGroup.Course.CreditHours,
                    MidTermGrade = e.MidtermGrade,
                    PreFinalGrade = e.PreFinalGrade,
                    WeekTwelve = e.WeekTwelve,
                    LetterGrade = e.LetterGrade,
                    GradePoint = e.PointsGrade
                })
                .ToListAsync();

            // Calculate the semester GPA by summing the grade points and dividing by the number of credit Hours Of courses where letter grade is not "U"
            var totalCreditHours = studentResults
             .Where(sr => sr.LetterGrade != "U")
             .Sum(sr => sr.CreditHours);

            // Calculate semesterGPA with a conditional check to avoid division by zero
            var semesterGPA = totalCreditHours != 0
                ? studentResults
                    .Where(sr => sr.LetterGrade != "U")
                    .Sum(sr => sr.GradePoint * sr.CreditHours) / totalCreditHours
                : 0; // If totalCreditHours is zero, set semesterGPA to 0



            // Save semesterGPA to the studentTracker table
            var existingTrackerEntry = await context.studentTrackers.FirstOrDefaultAsync(entry =>
                entry.StudentId == studentId && entry.SemesterTitle == semesterTitle);

            if (existingTrackerEntry == null)
            {
                var studentTrackerEntry = new StudentTracker
                {
                    StudentId = studentId,
                    SemesterTitle = semesterTitle,
                    SemesterGPA = semesterGPA,
                };

                context.studentTrackers.Add(studentTrackerEntry);
            }
            else
            {
                existingTrackerEntry.SemesterGPA = semesterGPA;
            }
            await context.SaveChangesAsync();


            //use the function to calculate the accumulative GPA 
            var accumulativeGPA = await CalculateAccumulativeGPA(studentId);
            // Retrieve the student entity from the database
            var student = await context.Students.FirstOrDefaultAsync(s => s.Id == studentId);

            if (student != null)
            {
                // Update the accumulative GPA property
                student.AccumulativeGpa = accumulativeGPA;

                // Save changes to the database
                await context.SaveChangesAsync();
            }

            return new StudentResultsDto
            {
                AccumulativeGPA = accumulativeGPA,
                SemesterGPA = semesterGPA,
                StudentListResults = studentResults
            };
        }
        #endregion
        #region function return accumulative GPA
        public async Task<decimal> CalculateAccumulativeGPA(int studentId)
        {
            var allStudentResults = await context.Evaluates
                .Where(e => e.StudentId == studentId && e.LetterGrade != "W") // Exclude withdrawn courses
                .Select(e => new
                {
                    CreditHours = e.CourseGroup.Course.CreditHours,
                    LetterGrade = e.LetterGrade,
                    GradePoint = e.PointsGrade
                })
                .ToListAsync();

            // Calculate accumulative GPA across all semesters
            var totalCreditHoursAllSemesters = allStudentResults
                .Where(sr => sr.LetterGrade != "U") // Exclude ungraded courses
                .Sum(sr => sr.CreditHours);

            // Check if totalCreditHoursAllSemesters is zero to avoid division by zero
            var accumulativeGPA = totalCreditHoursAllSemesters != 0
                ? allStudentResults
                    .Where(sr => sr.LetterGrade != "U") // Exclude ungraded courses
                    .Sum(sr => sr.GradePoint * sr.CreditHours) / totalCreditHoursAllSemesters
                : 0; // If totalCreditHoursAllSemesters is zero, set accumulativeGPA to 0

            return Math.Round(accumulativeGPA, 2);
        }
        #endregion
        #region Get The student Finished Courses and Unfinished courses for a specific student
        public async Task<StudentFinishedCoursesDto> GetStudentFinishedCoursesAndUnfinishedCourses(int studentId)
        {
            // Get the student's finished courses
            var studentFinishedCourses = await context.Evaluates
             .Where(e => e.StudentId == studentId && !(e.LetterGrade == "W" || e.LetterGrade == "U" || e.LetterGrade == "F"))
             .GroupBy(e => new { e.CourseGroup.Course.CourseCode, e.CourseGroup.Course.CourseTitle })
             .Select(g => new FinishedCourseDto
             {
                 CourseCode = g.Key.CourseCode,
                 CourseTitle = g.Key.CourseTitle,
                 PointsGrade = g.Max(e => e.PointsGrade),
                 LetterGrade = g.OrderBy(e => e.PointsGrade).LastOrDefault().LetterGrade,
                 CreditHours = g.First().CourseGroup.Course.CreditHours
             })
             .ToListAsync();


            // get the course codes of the finished courses
            var finishedCourseCodes = studentFinishedCourses.Select(f => f.CourseCode).ToList();
            // Get the student's unfinished courses
            var studentUnfinishedCourses = await context.Courses
                .Where(c => !finishedCourseCodes.Contains(c.CourseCode))
                .Select(c => new UnfinishedCourseDto
                {
                    CourseCode = c.CourseCode,
                    CourseTitle = c.CourseTitle,
                    CreditHours = c.CreditHours
                })
               .ToListAsync();


            return new StudentFinishedCoursesDto
            {
                FinishedCourses = studentFinishedCourses,
                UnfinishedCourses = studentUnfinishedCourses,
                FinishedCreditHours = studentFinishedCourses.Sum(c => c.CreditHours),
            };
        }
        #endregion
        #region get the data of the specific course to re-enroll in it
        public async Task <List<CourseDataDto>> GetCourseDataForReEnrollement(string courseCode, string semesterTitle, int studentId)
        {
            try
            {
                var courseData = await (from c in context.Courses // Select all courses from the Courses table in the database
                join cg in context.CourseGroups on c.CourseCode equals cg.CourseCode // Join with CourseGroups table on CourseCode
                join l in context.Lectures on cg.Id equals l.CourseGroupId // Join with Lectures table on CourseGroupId
                join i in context.Intervals on l.IntervalIdFk equals i.Id // Join with Intervals table on IntervalIdFk
                where c.CourseCode == courseCode && // Filter by course code
                cg.SemesterTitle == semesterTitle && l.Type == "Lecture" // Filter by semester title 
                select new CourseDataDto // Selecting specific data fields to return as CourseDataDto
                {
                    CourseCode = c.CourseCode,
                    CourseTitle = c.CourseTitle,
                    CourseGroups = cg.Group,
                    CreditHours = c.CreditHours,
                    PrerequisiteCourseCode = c.PrerequisiteCode,
                    LectureDay = l.Day,
                    LectureStartTime = i.StartInterval,
                    LectureEndTime = i.EndInterval
                }).ToListAsync();

                return courseData;
            }
            catch (Exception ex)
            {
                // Handle exception
                return null; // or throw the exception if you want to propagate it
            }
        }

        #endregion
        #region post re-enroll in a course
        public async Task<string> ReEnrollInCourse(EnrollmentRequestDto enrollmentRequestDto)
        {
            return await EnrollStudentInCourseGroup(enrollmentRequestDto);
        }
        #endregion

    }
}


