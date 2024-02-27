create table Department
(
   dept_id int primary key IDENTITY,
   dept_name  varchar(50) not null,
);

ALTER TABLE Student
ADD Email varchar(300) not null,
   Password varchar(50) not null

CREATE table Student
(
   Id nvarchar(450) primary key, ---
   Fname  varchar(50) not null,
   Lname  varchar(50) not null,
   DoB    date  not null,
   dept_id int  not null
   CONSTRAINT FK_Student_department
        FOREIGN KEY (dept_id)
        REFERENCES Department(dept_id)
		
);

ALTER TABLE Instructor
ADD Email varchar(300) not null,
   Password varchar(50) not null

create table Instructor
(
   Id nvarchar(450) primary key, ---
   ins_name  varchar(50) not null,
   Email varchar(300) not null,
   Password varchar(50) not null,
   dept_id int not null
   CONSTRAINT FK_Instructor_department
        FOREIGN KEY (dept_id)
        REFERENCES Department(dept_id)
		ON DELETE CASCADE
		ON UPDATE CASCADE
		
);

create table Course
(
   Course_id int primary key IDENTITY,
   course_name  varchar(50) not null,
   ins_id nvarchar(450) not null,
   dept_id int not null
   CONSTRAINT FK_Course_department
        FOREIGN KEY (dept_id)
        REFERENCES Department(dept_id)
		
		,
   CONSTRAINT   FK_Course_Instructor
        FOREIGN KEY (ins_id)
        REFERENCES Instructor(Id)
		
		
);

create table Topic
(
   Topic_id int primary key IDENTITY,
   Topic_name  varchar(50) not null,
   crs_id int not null
   CONSTRAINT FK_Topic_Course
        FOREIGN KEY (crs_id)
        REFERENCES Course(Course_id)
		
);
create table StudentCourses
(
   Std_id nvarchar(450),
   Course_id int
   CONSTRAINT FK_StudentCourses_Student
        FOREIGN KEY (Std_id)
        REFERENCES Student(Id)
		,

   CONSTRAINT   FK_StudentCourses_Course
        FOREIGN KEY (Course_id)
        REFERENCES Course(Course_id)
		,

   CONSTRAINT PK_StudentCourses PRIMARY KEY (Std_id, Course_id)
);

create table Exam
(
   Exam_id int IDENTITY(1,1) primary key,----------
   duration  int default 2, --
   Date date default GETDATE(),--
   total_marks int, --
   crs_id int not null,
   stud_id nvarchar(450), --
   score int ---
   CONSTRAINT FK_Exam_Student
        FOREIGN KEY (stud_id)
        REFERENCES Student(Id)
		,
   CONSTRAINT   FK_Exam_Course
        FOREIGN KEY (crs_id)
        REFERENCES Course(Course_id)
		
);


create table Question
(
   qest_id int primary key identity,
   text  varchar(300) not null,
   type  varchar(5) not null,
   Score int not null,     --weight
   level varchar(5) not null,
   crsID int  ------------------
   CONSTRAINT   FK_Question_Course
        FOREIGN KEY (crsID)
        REFERENCES Course(Course_id)
		
);

create table ExamQuestions
(
   exam_id int,
   question_id int 

   CONSTRAINT   FK_ExamQuestions_Question
        FOREIGN KEY (question_id)
        REFERENCES Question(qest_id)
		,

   CONSTRAINT FK_ExamQuestions_Exam
        FOREIGN KEY (exam_id)
        REFERENCES Exam(Exam_id)
		,
   CONSTRAINT PK_ExamQuestions PRIMARY KEY (exam_id, question_id)
);    

create table Choice
(
   choice_id int IDENTITY,
   text  varchar(300) not null,
   is_correct bit not null,
   question_id int 
   CONSTRAINT PK_Choice PRIMARY KEY (choice_id, question_id)--------
   CONSTRAINT FK_Choice_question
        FOREIGN KEY (question_id)
        REFERENCES Question(qest_id)
		
);



create table Answer
(
   stud_id nvarchar(450),
   question_id int,
   exam_id int ,
   choice_id int not null

   CONSTRAINT FK_Answer_Student
        FOREIGN KEY (stud_id)
        REFERENCES Student(Id)
		,

   CONSTRAINT   FK_Answer_Question
        FOREIGN KEY (question_id)
        REFERENCES Question(qest_id)
		,

   CONSTRAINT FK_Answer_Exam
        FOREIGN KEY (exam_id)
        REFERENCES Exam(Exam_id)
		,

   CONSTRAINT   FK_Answer_Choice
        FOREIGN KEY (choice_id, question_id)
        REFERENCES Choice(Choice_id, question_id)
		,

   CONSTRAINT PK_Answer PRIMARY KEY (stud_id, question_id, exam_id)
);

