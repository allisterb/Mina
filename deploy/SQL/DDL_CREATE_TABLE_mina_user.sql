-- SEQUENCE: public.Mina_user__id_seq

-- DROP SEQUENCE public.Mina_user__id_seq;

CREATE SEQUENCE public.Mina_user__id_seq
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

ALTER SEQUENCE public.Mina_user__id_seq
    OWNER TO Mina;

-- Table: public.Mina_user

-- DROP TABLE public.Mina_user;

CREATE TABLE public.Mina_user
(
    _id integer NOT NULL DEFAULT nextval('Mina_user__id_seq'::regclass),
    user_name character varying(100) COLLATE pg_catalog."default" NOT NULL,
    last_logged_in timestamp without time zone,
    CONSTRAINT Mina_user_pkey PRIMARY KEY (_id)
)

TABLESPACE pg_default;

ALTER TABLE public.Mina_user
    OWNER to Mina;
