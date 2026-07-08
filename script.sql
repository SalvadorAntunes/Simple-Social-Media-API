DROP TABLE users CASCADE constraints;

CREATE TABLE users(
    id number(9,0)  DEFAULT idGenerator.NEXTVAL primary key,
    username varchar2(15) unique,
    email varchar2(254) unique,
    uName varchar2(15),
    password varchar2(15),
    dateOfBirth date
);

alter table users add constraint min_age check(FLOOR(MONTHS_BETWEEN(SYSDATE, dateOfBirth) / 12) >= 13);

create table posts(
    id number(9,0) DEFAULT idGenerator.NEXTVAL primary key,
    user number(9,0),
    postDate date,
    text varchar2(250)
);

alter table posts add constraint fk_posts_user foreign key (user) references users(id);

create table comments(
    id number(9,0) primary key,
    postCommented number(9,0)
);

alter table comments add constraint fk_comment foreign key (id) references posts(id);
alter table comments add constraint fk_comment foreign key (postCommented) references posts(id);

create table likes(
    userID number(9,0) primary key,
    postID number(9,0) primary key
);

alter table likes add constraint fk_user_like foreign key (userID) references users(id);
alter table likes add constraint fk_post_like foreign key (postID) references posts(id);

create or replace function likeNumber(id number) return number
is
    num_likes number;
begin
    select count(*) into num_likes
    from likes
    where postID = id 
    group by num_likes;

    return num_likes;
end likeNumber;
/

create or replace function commentsNumber(id number) return number
is
    num_comms number;
begin
    select count(*) into num_comms
    from comments
    where postCommented = id
    group by num_comms;

    return num_comms;
end commentsNumber;
/

create sequence idGenerator
start with 1
increment by 1;
