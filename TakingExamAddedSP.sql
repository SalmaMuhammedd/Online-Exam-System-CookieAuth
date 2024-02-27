--Added sp for taking exam:
CREATE PROC SelectQuestionChoices @questionId int
AS
SELECT Choice.* from Choice, Question
where question_id = @questionId AND question_id = qest_id

GO


CREATE PROC SelectCourseByExamId @examId int
AS
SELECT Course.* FROM Exam, Course WHERE Course_id = crs_id AND Exam_id = @examId

GO

CREATE PROC SelectStudentByExamId @examId int
AS
SELECT Student.* FROM Exam, Student WHERE Id = stud_id AND Exam_id = @examId

GO

ALTER PROCEDURE [dbo].[sp_SelectStudentCoursesById] @id nvarchar(450)
AS
BEGIN
    SELECT Course.* FROM StudentCourses, Course  WHERE Std_id = @id AND Course.Course_id = StudentCourses.Course_id
END

GO
----------------------------------

CREATE PROCEDURE StudentGrades 
    @stdid nvarchar(450)
AS  
BEGIN
	SELECT * from Exam WHERE stud_id = @stdid AND score is not null
END
GO

StudentGrades st1

GO

CREATE PROC SelectNotTakenExamByStudentId @studId nvarchar(450)
AS
SELECT * FROM Exam WHERE stud_id = @studId  AND score is null

GO

SelectNotTakenExamByStudentId st2
