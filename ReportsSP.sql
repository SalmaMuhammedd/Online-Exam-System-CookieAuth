CREATE PROC DepartmentStudents @deptId int
AS
SELECT * FROM Student WHERE dept_id = @deptId

GO

/*
2.Report that takes the student ID and returns the grades of the student in all courses. %
*/


CREATE PROC StudentGradePercentage @studId int
AS
SELECT (SUM(total_marks) / SUM(score) )*100 AS StudentGrade
FROM Exam WHERE stud_id = @studId

GO

/*
3.Report that takes the instructor ID and returns the name of the courses that he teaches and the number of student per course.
*/

CREATE PROC InstructorCourses @InstructorId int
AS
SELECT course_name, COUNT(Std_id) AS NumOfStudents
FROM Course, StudentCourses WHERE ins_id = @InstructorId AND Course.Course_id = StudentCourses.Course_id
GROUP BY course_name

GO

/*
4.Report that takes course ID and returns its topics
*/

CREATE PROC CoursesTopics @CourseId int
AS
SELECT Topic_name
FROM Course, Topic WHERE crs_id = Course_id

GO

/*
5.Report that takes exam number and returns the Questions in it and chocies [freeform report]
*/

CREATE PROC ExamQuestionsAndChoices @ExamId int
AS
SELECT Question.text, Choice.text
FROM Question, ExamQuestions, Choice
WHERE ExamQuestions.exam_id = @ExamId AND ExamQuestions.question_id = qest_id AND Choice.question_id = qest_id

GO

/*
6.Report that takes exam number and the student ID then returns the Questions in this exam with the student answers
*/
create proc sp_GetStudentAnswers @ExamID int, @StudentID int
as
    SELECT q.text AS Question, c.text AS StudentAnswer, c2.text AS CorrectAnswer
    FROM ExamQuestions eq, Question q, Answer a, Choice c, Choice c2
    WHERE eq.question_id = q.qest_id 
        AND eq.exam_id = a.exam_id 
        AND eq.question_id = a.question_id 
        AND a.choice_id = c.choice_id
        AND eq.exam_id = @ExamID
        AND a.stud_id = @StudentID
		AND c2.is_correct = 1;
GO
create Proc SP_ExamStudentAnswers @ExamId int  --not fully correct
as
	select Q.* ,Ans.choice_id
	from Question Q join ExamQuestions EQ
	on Q.qest_id=EQ.question_id
	join Answer Ans
	on Ans.question_id=Q.qest_id
	where EQ.exam_id=@ExamId