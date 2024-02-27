---Date, duration => default
--function selectQuesitons

create proc AddExam @dur int , @date date ,
@marks int , @crsid int , @stdid int , @score int
as
	insert into Exam values( @dur , @date , @marks , @crsid , @stdid , @score)

GO

CREATE PROCEDURE ExamGeneration
	@studId nvarchar(450), @courseID int, @trueFalseNo INT, @MCQNo INT 
AS
BEGIN
	DECLARE @score INT = 0;
	DECLARE @exam_id INT;
	 -- Create a new exam entry
	 
    INSERT INTO Exam(crs_id, stud_id) VALUES (@courseID, @studId);
	SET @exam_id = SCOPE_IDENTITY(); 

    -- Select random True/False questions and insert into ExamQuestions
    INSERT INTO ExamQuestions (question_id, exam_id)
    SELECT Top (@trueFalseNo) qest_id, @exam_id
    FROM Question
    WHERE crsID = @courseID AND type = 'TF'
    ORDER BY NEWID()

    -- Select random Multiple Choice questions and insert into ExamQuestions
    INSERT INTO ExamQuestions (question_id, exam_id)
    SELECT Top (@MCQNo) qest_id, @exam_id
    FROM Question
    WHERE type = 'MCQ' AND crsID = @courseID
    ORDER BY NEWID()

	--score as derived att
	SELECT @score += Score
	FROM Question, ExamQuestions
	WHERE qest_id = question_id AND exam_id = @exam_id;

	UPDATE Exam 
	SET total_marks = @score
	WHERE Exam_id = @exam_id;
END

GO

EXEC ExamGeneration 'st1', 1, 1, 1;
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
GO
CREATE PROCEDURE QuestionAnswer
	@examID int, @studID nvarchar(450), @questionId int, @answerId int 
AS
BEGIN
    -- Insert answers 
    INSERT INTO Answer (exam_id, stud_id, choice_id, question_id) VALUES (@examID, @studID, @answerId, @questionId)
    
END

GO
EXEC QuestionAnswer 1, 2, 3, 4
EXEC QuestionAnswer 1, 2, 6, 7
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE ExamCorrection 
    @stdid nvarchar(450),
    @examid INT 
AS  
BEGIN
    DECLARE @totalscore INT = 0
    DECLARE @score INT 

    DECLARE exam_cursor CURSOR FOR

        SELECT q.Score
        FROM Answer a
        INNER JOIN Choice c ON a.choice_id = c.choice_id
        INNER JOIN Question q ON c.question_id = q.qest_id
        WHERE a.stud_id = @stdid AND a.exam_id = @examid AND c.is_correct = 1

    OPEN exam_cursor

    FETCH NEXT FROM exam_cursor INTO @score

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @totalscore = @totalscore + @score
        FETCH NEXT FROM exam_cursor INTO @score
    END

    CLOSE exam_cursor
    DEALLOCATE exam_cursor

    UPDATE Exam 
    SET score = @totalscore
    WHERE Exam_id = @examid AND stud_id = @stdid
END
GO
EXEC ExamCorrection 2, 1