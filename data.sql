Insert INTO Course VALUES('oop', 'ins1', 1) --4
Insert INTO Course VALUES('C#', 'ins1', 1) --5
Insert INTO Course VALUES('html', 'ins1', 1) --6
-------------------------------------------------

INSERT INTO StudentCourses VALUES('stud1', 4)
INSERT INTO StudentCourses VALUES('stud1', 5)
INSERT INTO StudentCourses VALUES('stud1', 6)

----------------------------------------------------
/*
	[text]
      ,[type]
      ,[Score]
      ,[level]
      ,[crsID]
*/

INSERT INTO Question VALUES ('oop is fun', 'TF', 1, 1, 4)
INSERT INTO Question VALUES ('oop stands for', 'MCQ', 2, 3, 4)
INSERT INTO Question VALUES ('c# is fun', 'TF', 1, 1, 5)
INSERT INTO Question VALUES ('http stands for', 'MCQ', 1, 1, 5)
select * from Question
---------------------------------
INSERT INTO Choice VALUES('True', 1,6)
INSERT INTO Choice VALUES('false', 0,6)

INSERT INTO Choice VALUES('object oriented programming', 1,7)
INSERT INTO Choice VALUES('wrong answer', 0,7)
INSERT INTO Choice VALUES('wrong answer', 0,7)

INSERT INTO Choice VALUES('True',1, 8)
INSERT INTO Choice VALUES('false',0, 8)

INSERT INTO Choice VALUES('Hyper text transfer', 1,8)
INSERT INTO Choice VALUES('wrong answer', 0,8)
INSERT INTO Choice VALUES('wrong answer', 0,8)

