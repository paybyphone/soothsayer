CREATE TABLE SAMPLE.things
(
  id NUMBER NOT NULL,
  name VARCHAR2(100) NOT NULL,
  CONSTRAINT things_pk PRIMARY KEY (id)
)
TABLESPACE SAMPLE
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 10M
    next 10M
    minextents 1
    maxextents unlimited
  );
