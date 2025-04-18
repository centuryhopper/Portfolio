
CREATE TABLE LOGS (
    log_id SERIAL PRIMARY KEY,
    date_logged DATE NOT NULL,
    level VARCHAR(15) NOT NULL,
    message VARCHAR(256) NOT NULL
);

CREATE TABLE CONTACTTABLE(
    ID SERIAL PRIMARY KEY,
    NAME TEXT NOT NULL,
    EMAIL TEXT NOT NULL,
    SUBJECT TEXT NOT NULL,
    MESSAGE TEXT NOT NULL
);

INSERT INTO CONTACTTABLE (NAME,
EMAIL,
SUBJECT,
MESSAGE)
VALUES (
    'NAME',
    'EMAIL',
    'SUBJECT',
    'MESSAGE'
);



CREATE TABLE PROJECT_CARD(
    ID SERIAL PRIMARY KEY,
    IMGURL TEXT NOT NULL,
    TITLE TEXT NOT NULL,
    DESCRIPTION TEXT NOT NULL,
    PROJECTLINK TEXT NOT NULL,
    SOURCECODELINK TEXT NOT NULL
);

INSERT INTO PROJECT_CARD (
IMGURL,
TITLE,
DESCRIPTION,
PROJECTLINK,
SOURCECODELINK)
VALUES (
    'IMGURL',
    'TITLE',
    'DESCRIPTION',
    'PROJECTLINK',
    'SOURCECODELINK'
);


-- Create the blog table
CREATE TABLE BLOG (
    id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    date TIMESTAMP NOT NULL,
    preview_desc TEXT NOT NULL,
    route_name VARCHAR(255),
    full_desc TEXT NOT NULL
);

-- Create the video_url table
CREATE TABLE VIDEO_URL (
    id SERIAL PRIMARY KEY,
    blog_id INT NOT NULL,
    url TEXT NOT NULL,
    FOREIGN KEY (blog_id) REFERENCES blog(id) ON DELETE CASCADE
);


INSERT INTO BLOG (
title,
date,
preview_desc,
route_name,
full_desc)
VALUES (
    'title',
    CURRENT_TIMESTAMP,
    'preview_desc',
    'route_name',
    'full_desc'
);

INSERT INTO VIDEO_URL (
blog_id,
url
)
VALUES (
    1,
    'url'
);

CREATE TABLE skill (
    id SERIAL PRIMARY KEY,
    title TEXT NOT NULL
);

CREATE TABLE skill_Description (
    id SERIAL PRIMARY KEY,
    skill_id INT NOT NULL,
    description TEXT NOT NULL,
    FOREIGN KEY (skill_id) REFERENCES skill(id) ON DELETE CASCADE
);




-- tweaking value of serial primary keys
-- SELECT pg_get_serial_sequence('blog', 'id');
-- ALTER SEQUENCE your_sequence_name RESTART WITH 1;
-- SELECT nextval('blog_id_seq');
-- SELECT currval('public."blog_id_seq"');