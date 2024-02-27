Alter proc getCoursesByInstructor @Ins_Id int
as
  select c.*
  from Course c,Instructor i
  where c.ins_id = i.Id and i.Id = @Ins_Id

GO
create proc getExamsofCourse @crs_id int
as
  select e.*
  from Course c,Exam e
  where Course_id = crs_id and Course_id = @crs_id


GO
alter proc getStudentInSpecificCourse @course_Id int
as
  select s.*
  from Student s , StudentCourses sc , Course c
  where s.Id = sc.Std_id and sc.Course_id = c.Course_id and c.Course_id = @course_Id



GO
alter PROCEDURE sp_UpdateExam
    @examID int,
	@duration int, 
	@date Date
AS
BEGIN TRY
    UPDATE Exam SET duration = @duration, Date = @date  WHERE Exam_id = @examID
END TRY
BEGIN CATCH
	SELECT 'Could not update this exam'
END CATCH

