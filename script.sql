CREATE DATABASE SocialMediaProject;
GO

USE SocialMediaProject;
GO

-- Drop tables if they exist
DROP TABLE IF EXISTS likes;
DROP TABLE IF EXISTS comments;
DROP TABLE IF EXISTS posts;
DROP TABLE IF EXISTS users;
GO

-- Users table
CREATE TABLE users(
    id INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(15) NOT NULL UNIQUE,
    email VARCHAR(254) NOT NULL UNIQUE,
    uName VARCHAR(15) NOT NULL,
    [password] NVARCHAR(MAX) NOT NULL,
    dateOfBirth DATE NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- Posts table
CREATE TABLE posts(
    id INT IDENTITY(1,1) PRIMARY KEY,
    userID INT NOT NULL,
    postDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [text] VARCHAR(250) NOT NULL,

    CONSTRAINT FK_Posts_User
        FOREIGN KEY (userID)
        REFERENCES users(id)
);
GO

-- Comments table
CREATE TABLE comments(
    id INT IDENTITY(1,1) PRIMARY KEY,
    postCommented INT NOT NULL,

    CONSTRAINT FK_Comment_Itself
        FOREIGN KEY (id)
        REFERENCES posts(id),

    CONSTRAINT FK_Commented_Post
        FOREIGN KEY (postCommented)
        REFERENCES posts(id)
);
GO

-- Likes table
CREATE TABLE likes(
    userID INT NOT NULL,
    postID INT NOT NULL,

    CONSTRAINT PK_Likes PRIMARY KEY(userID, postID),

    CONSTRAINT FK_Likes_User
        FOREIGN KEY (userID)
        REFERENCES users(id),

    CONSTRAINT FK_Likes_Post
        FOREIGN KEY (postID)
        REFERENCES posts(id)
);
GO

-- Function to count likes
CREATE OR ALTER FUNCTION likeNumber(@id INT)
RETURNS INT
AS
BEGIN
    DECLARE @num_likes INT;

    SELECT @num_likes = COUNT(*)
    FROM likes
    WHERE postID = @id;

    RETURN @num_likes;
END;
GO

-- Function to count comments
CREATE OR ALTER FUNCTION commentsNumber(@id INT)
RETURNS INT
AS
BEGIN
    DECLARE @num_comments INT;

    SELECT @num_comments = COUNT(*)
    FROM comments
    WHERE postCommented = @id;

    RETURN @num_comments;
END;
GO
