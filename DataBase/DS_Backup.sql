--
-- PostgreSQL database dump
--

-- Dumped from database version 16rc1
-- Dumped by pg_dump version 16rc1

-- Started on 2024-04-12 17:46:01

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 224 (class 1259 OID 18054)
-- Name: change_history; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.change_history (
    change_history_id integer NOT NULL,
    file_id integer NOT NULL,
    user_id integer NOT NULL,
    change_description text,
    date_change timestamp without time zone NOT NULL
);


ALTER TABLE public.change_history OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 18053)
-- Name: change_history_change_history_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.change_history ALTER COLUMN change_history_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.change_history_change_history_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 219 (class 1259 OID 18010)
-- Name: files; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.files (
    file_id integer NOT NULL,
    file_name character varying(255) NOT NULL,
    creation_date timestamp without time zone NOT NULL,
    virus_availiability boolean NOT NULL,
    virus_descrition text,
    uploader_id integer NOT NULL,
    folder_id integer NOT NULL
);


ALTER TABLE public.files OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 18009)
-- Name: files_file_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.files ALTER COLUMN file_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.files_file_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 217 (class 1259 OID 18002)
-- Name: folders; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.folders (
    folder_id integer NOT NULL,
    folder_name character varying(255) NOT NULL,
    folder_description text,
    creation_date timestamp without time zone NOT NULL
);


ALTER TABLE public.folders OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 18001)
-- Name: folders_folder_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.folders ALTER COLUMN folder_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.folders_folder_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 215 (class 1259 OID 17994)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    user_id integer NOT NULL,
    login character varying(255) NOT NULL,
    password character varying(255) NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 18027)
-- Name: users_folder; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users_folder (
    user_id integer NOT NULL,
    folder_id integer NOT NULL,
    role character varying(32) NOT NULL
);


ALTER TABLE public.users_folder OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 18043)
-- Name: users_mac; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users_mac (
    user_mac_id integer NOT NULL,
    user_id integer NOT NULL,
    mac character(12) NOT NULL
);


ALTER TABLE public.users_mac OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 18042)
-- Name: users_mac_user_mac_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.users_mac ALTER COLUMN user_mac_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.users_mac_user_mac_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 225 (class 1259 OID 18071)
-- Name: users_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.users ALTER COLUMN user_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.users_user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 4829 (class 0 OID 18054)
-- Dependencies: 224
-- Data for Name: change_history; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.change_history (change_history_id, file_id, user_id, change_description, date_change) FROM stdin;
\.


--
-- TOC entry 4824 (class 0 OID 18010)
-- Dependencies: 219
-- Data for Name: files; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.files (file_id, file_name, creation_date, virus_availiability, virus_descrition, uploader_id, folder_id) FROM stdin;
\.


--
-- TOC entry 4822 (class 0 OID 18002)
-- Dependencies: 217
-- Data for Name: folders; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.folders (folder_id, folder_name, folder_description, creation_date) FROM stdin;
\.


--
-- TOC entry 4820 (class 0 OID 17994)
-- Dependencies: 215
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (user_id, login, password) FROM stdin;
1	test	test
\.


--
-- TOC entry 4825 (class 0 OID 18027)
-- Dependencies: 220
-- Data for Name: users_folder; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users_folder (user_id, folder_id, role) FROM stdin;
\.


--
-- TOC entry 4827 (class 0 OID 18043)
-- Dependencies: 222
-- Data for Name: users_mac; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users_mac (user_mac_id, user_id, mac) FROM stdin;
\.


--
-- TOC entry 4836 (class 0 OID 0)
-- Dependencies: 223
-- Name: change_history_change_history_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.change_history_change_history_id_seq', 1, false);


--
-- TOC entry 4837 (class 0 OID 0)
-- Dependencies: 218
-- Name: files_file_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.files_file_id_seq', 1, false);


--
-- TOC entry 4838 (class 0 OID 0)
-- Dependencies: 216
-- Name: folders_folder_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.folders_folder_id_seq', 1, false);


--
-- TOC entry 4839 (class 0 OID 0)
-- Dependencies: 221
-- Name: users_mac_user_mac_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_mac_user_mac_id_seq', 1, false);


--
-- TOC entry 4840 (class 0 OID 0)
-- Dependencies: 225
-- Name: users_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_user_id_seq', 1, false);


--
-- TOC entry 4669 (class 2606 OID 18060)
-- Name: change_history change_history_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.change_history
    ADD CONSTRAINT change_history_pkey PRIMARY KEY (change_history_id);


--
-- TOC entry 4663 (class 2606 OID 18016)
-- Name: files files_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files
    ADD CONSTRAINT files_pkey PRIMARY KEY (file_id);


--
-- TOC entry 4661 (class 2606 OID 18008)
-- Name: folders folders_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.folders
    ADD CONSTRAINT folders_pkey PRIMARY KEY (folder_id);


--
-- TOC entry 4665 (class 2606 OID 18031)
-- Name: users_folder users_folder_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users_folder
    ADD CONSTRAINT users_folder_pk PRIMARY KEY (user_id, folder_id);


--
-- TOC entry 4667 (class 2606 OID 18047)
-- Name: users_mac users_mac_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users_mac
    ADD CONSTRAINT users_mac_pkey PRIMARY KEY (user_mac_id);


--
-- TOC entry 4659 (class 2606 OID 18000)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);


--
-- TOC entry 4675 (class 2606 OID 18061)
-- Name: change_history change_history_file_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.change_history
    ADD CONSTRAINT change_history_file_id_fkey FOREIGN KEY (file_id) REFERENCES public.files(file_id);


--
-- TOC entry 4676 (class 2606 OID 18066)
-- Name: change_history change_history_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.change_history
    ADD CONSTRAINT change_history_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id);


--
-- TOC entry 4670 (class 2606 OID 18022)
-- Name: files files_folder_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files
    ADD CONSTRAINT files_folder_id_fkey FOREIGN KEY (folder_id) REFERENCES public.folders(folder_id);


--
-- TOC entry 4671 (class 2606 OID 18017)
-- Name: files files_uploader_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files
    ADD CONSTRAINT files_uploader_id_fkey FOREIGN KEY (uploader_id) REFERENCES public.users(user_id);


--
-- TOC entry 4672 (class 2606 OID 18037)
-- Name: users_folder users_folder_folder_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users_folder
    ADD CONSTRAINT users_folder_folder_id_fkey FOREIGN KEY (folder_id) REFERENCES public.folders(folder_id);


--
-- TOC entry 4673 (class 2606 OID 18032)
-- Name: users_folder users_folder_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users_folder
    ADD CONSTRAINT users_folder_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id);


--
-- TOC entry 4674 (class 2606 OID 18048)
-- Name: users_mac users_mac_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users_mac
    ADD CONSTRAINT users_mac_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id);


-- Completed on 2024-04-12 17:46:01

--
-- PostgreSQL database dump complete
--

