
CREATE TABLE LOGS (
    log_id SERIAL PRIMARY KEY,
    date_logged DATE NOT NULL,
    level VARCHAR(15) NOT NULL,
    message VARCHAR(256) NOT NULL
);


-- tweaking value of serial primary keys
-- SELECT pg_get_serial_sequence('blog', 'id');
-- ALTER SEQUENCE your_sequence_name RESTART WITH 1;
-- SELECT nextval('blog_id_seq');
-- SELECT currval('public."blog_id_seq"');