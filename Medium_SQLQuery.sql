create database Medium



    
go
create table registration(
user_id VARCHAR(10) PRIMARY KEY,
first_name VARCHAR(30),
middle_name VARCHAR(30),
last_name VARCHAR(30),
phone_number char(10),
email_id VARCHAR(50) not null,
gender VARCHAR(6),
date_of_birth date,
pass VARCHAR(10) not null,
photo_filename varchar(max),
bio varchar(100),
created_at DATETIME2 default SYSDATETIME(), 
user_role varchar(5) 
);
 

 
CREATE table blog(
    UserId varchar(10),
    BlogId int ,
   BlogTitle nvarchar(max),
    BlogImage varchar(max),
    BlogContent nvarchar(max),
    Blogtype varchar(10),
    creation_time DATETIME2,
    likes int default 0,
FOREIGN key (UserId) REFERENCES dbo.registration(user_id),
PRIMARY key (UserId, BlogId)
);


go
create table blog_likes(
    UserId varchar(10),
    BlogId int,
    LikerId varchar(10), --id of user who liked the post
    LikeDateTime datetime2 DEFAULT SYSDATETIME(),

PRIMARY key(UserId, BlogId,LikerId)
);
 
GO
create table followers(
    UserId VARCHAR(10),
    FollowingId varchar(10),
PRIMARY key (UserId, FollowingId)
);

GO
create table reviews(
    UserId varchar(10) primary key,
    UserImage varchar(max),
    UserBio varchar(100),
    FirstName varchar(100),
    LastName varchar(100),
    Comment varchar(200)
)

alter table reviews 
alter COLUMN Comment nvarchar(max)


GO
create PROCEDURE delete_follower
(@uid VARCHAR(10), @fid VARCHAR(10))
AS
DELETE from followers where UserId=@uid and FollowingId=@fid

 
go
create PROCEDURE get_followers
(@uid VARCHAR(10), @fid VARCHAR(10))
as
SELECT*from followers where UserId=@uid 
 

go
create PROCEDURE blog_like_insert
(@uid VARCHAR(10), @bid int,@lid varchar(10))
AS
insert into blog_likes (UserId, BlogId, LikerId)
VALUES (@uid, @bid, @lid) 
update blog
set likes=(select count(UserId) from blog_likes where BlogId=@bid and UserId=@uid)
where UserId=@uid and BlogId=@bid
 

go
create procedure dislike
(@uid varchar(10), @bid int, @lid varchar(10))
as
delete from blog_likes 
where UserId=@uid and BlogId=@bid and LikerId=@lid
 

go
create PROCEDURE user_login (
@id varchar(10),
@password varchar(10)
)
AS
select*from dbo.registration
where user_id=@id and pass=@password;
 
go
create PROCEDURE inc_likes(
    @id varchar(10), @bid int
)
as
update dbo.blog
set likes = likes+1
where UserId=@id and BlogId=@bid 
 
go
create PROCEDURE user_signup(
@user_id VARCHAR(10),
@first_name VARCHAR(30),
@middle_name VARCHAR(30),
@last_name VARCHAR(30),
@phone_number char(10),
@email_id VARCHAR(50) ,
@gender VARCHAR(6),
@date_of_birth date,
@pass VARCHAR(10),
@photo_filename varchar(max),
@bio varchar(100)
)
AS
insert into dbo.registration (user_id, first_name,middle_name,last_name,phone_number,email_id,gender,date_of_birth,pass,photo_filename,bio,created_at,user_role)
VALUES(@user_id, @first_name, @middle_name, @last_name, @phone_number, @email_id, @gender, @date_of_birth, @pass, @photo_filename, @bio, SYSDATETIME(), 'user')
 
 

 
 
GO
alter PROCEDURE blog_insert(
@uid varchar(10), @title nvarchar(50), @img VARCHAR(max), @content nvarchar(max), @type varchar(10))
as
if(not exists(select 1 from blog where UserId=@uid))
begin 
insert into blog  (UserId,BlogId,BlogTitle,BlogImage,BlogContent,Blogtype,creation_time,likes)
values(@uid,1,@title,@img,@content,@type,SYSDATETIME(),0) 
END
 
ELSE
BEGIN
insert into blog (UserId,BlogId,BlogTitle,BlogImage,BlogContent,Blogtype,creation_time,likes)
values(@uid,(select max(BlogId) from blog where UserId=@uid)+1,@title,@img,@content,@type,SYSDATETIME(),0) 
END
 
go
create procedure user_titles
(@uid VARCHAR(10))
AS
select * from blog where UserId=@uid
 
go
create procedure delete_blog
(@uid varchar(10), @bid int)
AS
delete from blog where UserId=@uid and BlogId=@bid
 
go
create PROCEDURE get_blog_for_update
(@uid varchar(10), @bid int)
as 
select * from blog where UserId=@uid and BlogId=@bid
 
go
ALTER PROCEDURE update_blog
(@uid varchar(10), @bid int, @btitle NVARCHAR(50),@bimage VARCHAR(max), @bcontent NVARCHAR(max),  @btype VARCHAR(10))
as
update blog 
set BlogTitle=@btitle, BlogImage=@bimage,BlogContent=@bcontent, Blogtype=@btype
where UserId=@uid and BlogId=@bid
 
go
create PROCEDURE update_profile
(@uidold varchar(10),@uidnew varchar(10), @fname varchar(30), @mname varchar(30), @lname varchar(30), @pnumber char(10),@eid varchar(50), @password varchar(10),@photo varchar(max), @bio varchar(100))
AS
update registration
set user_id=@uidnew, first_name=@fname, middle_name=@mname, last_name=@lname, phone_number=@pnumber, email_id=@eid, pass=@password, photo_filename=@photo, bio=@bio
where user_id=@uidold
 
go
create PROCEDURE check_userid
(@uid VARCHAR(10))
as 
SELECT user_id from registration where user_id=@uid
 
select * from registration
select * from blog
Select *from  blog_likes
select * from followers
select* from reviews
truncate table blog_likes
truncate table blog
truncate table registration
 
go
create PROCEDURE get_blogs
(@uid VARCHAR(10))
AS
SELECT UserId,BlogId, BlogTitle, BlogImage, BlogContent, Blogtype,likes
from blog  where UserId=@uid
union 
SELECT UserId,BlogId, BlogTitle, BlogImage, BlogContent, Blogtype,likes
from blog  where UserId in (select FollowingId from followers where UserId=@uid)
and Blogtype in ('public', 'Restricted')
UNION
SELECT UserId,BlogId, BlogTitle, BlogImage, BlogContent, Blogtype,likes
from blog  where UserId not in (select FollowingId from followers where UserId=@uid)
and Blogtype = 'public'
AND UserId != @uid
 
GO
create PROCEDURE get_user_blogs
(@uid VARCHAR(10))
AS
select * from blog where UserId=@uid
 
exec get_user_blogs 'utk1'


